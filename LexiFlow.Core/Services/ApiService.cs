using LexiFlow.Core.Entities;
using LexiFlow.Core.Interfaces;
using LexiFlow.Core.Models;
using LexiFlow.Core.Models.Requests;
using LexiFlow.Core.Models.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LexiFlow.Core.Services
{
    /// <summary>
    /// Triển khai dịch vụ API cho giao tiếp với server LexiFlow
    /// </summary>
    public class ApiService : IApiService
    {
        private readonly ILogger<ApiService> _logger;
        private readonly IAppSettingsService _appSettings;
        private readonly TokenManager _tokenManager;
        private readonly HttpClient _httpClient;
        private string _accessToken = string.Empty;
        private string _baseUrl;

        public ApiService(
            ILogger<ApiService> logger,
            IAppSettingsService appSettings,
            TokenManager tokenManager)
        {
            _logger = logger;
            _appSettings = appSettings;
            _tokenManager = tokenManager;
            _httpClient = new HttpClient();
            InitializeHttpClient();
        }

        private void InitializeHttpClient()
        {
            // Khởi tạo các cài đặt cơ bản cho HttpClient
            _baseUrl = _appSettings.ApiUrl;
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.Timeout = TimeSpan.FromSeconds(30);

            // Khôi phục token từ bộ nhớ an toàn
            if (_tokenManager.IsTokenValid())
            {
                _accessToken = _tokenManager.GetCurrentToken();
                if (!string.IsNullOrEmpty(_accessToken))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", _accessToken);
                    _logger.LogInformation("Token đã được khôi phục từ bộ nhớ an toàn");
                }
            }
            else
            {
                _logger.LogInformation("Không có token hợp lệ trong bộ nhớ");
            }
        }

        /// <summary>
        /// Thiết lập access token cho các yêu cầu tiếp theo
        /// </summary>
        public void SetAccessToken(string token, DateTime expiresAt)
        {
            if (string.IsNullOrEmpty(token))
            {
                ClearAccessToken();
                return;
            }

            _accessToken = token;
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            // Lưu token vào bộ nhớ an toàn
            _tokenManager.SaveToken(token, expiresAt);
            _logger.LogInformation("Token mới đã được thiết lập và lưu trữ");
        }

        /// <summary>
        /// Xóa access token khi đăng xuất
        /// </summary>
        public void ClearAccessToken()
        {
            _accessToken = string.Empty;
            _httpClient.DefaultRequestHeaders.Authorization = null;
            _tokenManager.ClearToken();
            _logger.LogInformation("Token đã được xóa");
        }

        /// <summary>
        /// Lấy token hiện tại
        /// </summary>
        public string GetCurrentToken()
        {
            return _accessToken;
        }

        /// <summary>
        /// Thực hiện đăng nhập và lấy token
        /// </summary>
        public async Task<ServiceResult<LoginResponse>> LoginAsync(string username, string password)
        {
            try
            {
                var request = new LoginRequest
                {
                    Username = username,
                    Password = password
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonSerializer.Deserialize<LoginResponse>(
                        responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                    {
                        // Lưu token cho các request tiếp theo
                        SetAccessToken(
                            loginResponse.Token,
                            loginResponse.ExpiresAt ?? DateTime.Now.AddHours(8));

                        return ServiceResult<LoginResponse>.Success(loginResponse);
                    }
                    else
                    {
                        return ServiceResult<LoginResponse>.Fail("Không thể phân tích dữ liệu đăng nhập từ server");
                    }
                }
                else
                {
                    // Xử lý các mã lỗi HTTP
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return ServiceResult<LoginResponse>.Fail("Thông tin đăng nhập không hợp lệ");
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        return ServiceResult<LoginResponse>.Fail($"Lỗi đăng nhập: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Lỗi kết nối khi đăng nhập");
                return ServiceResult<LoginResponse>.Fail("Lỗi kết nối đến server. Vui lòng kiểm tra kết nối mạng.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi đăng nhập");
                return ServiceResult<LoginResponse>.Fail($"Lỗi không xác định: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý các yêu cầu HTTP chung với xử lý token tự động
        /// </summary>
        private async Task<ServiceResult<T>> SendRequestWithTokenHandlingAsync<T>(Func<Task<HttpResponseMessage>> requestFunc)
        {
            try
            {
                // Kiểm tra token
                if (string.IsNullOrEmpty(_accessToken))
                {
                    return ServiceResult<T>.Fail("Không có quyền truy cập. Vui lòng đăng nhập lại.");
                }

                // Thực hiện yêu cầu
                var response = await requestFunc();

                // Kiểm tra trạng thái
                if (response.IsSuccessStatusCode)
                {
                    // Xử lý phản hồi thành công
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<T>(
                        content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return ServiceResult<T>.Success(result);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Thử làm mới token một lần
                    var refreshResult = await RefreshTokenAsync();
                    if (refreshResult.SuccessResult)
                    {
                        // Thử lại yêu cầu ban đầu
                        response = await requestFunc();
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonSerializer.Deserialize<T>(
                                content,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                            return ServiceResult<T>.Success(result);
                        }
                    }

                    // Nếu vẫn thất bại sau khi làm mới token
                    ClearAccessToken();
                    return ServiceResult<T>.Fail("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
                }
                else
                {
                    // Xử lý các lỗi khác
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ServiceResult<T>.Fail($"Lỗi API: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Lỗi kết nối HTTP");
                return ServiceResult<T>.Fail("Lỗi kết nối đến server. Vui lòng kiểm tra kết nối mạng.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định");
                return ServiceResult<T>.Fail($"Lỗi không xác định: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy danh sách từ vựng từ API
        /// </summary>
        public async Task<ServiceResult<PagedResult<Vocabulary>>> GetVocabularyAsync(int page = 1, int pageSize = 20, DateTime? lastSync = null)
        {
            string url = $"{_baseUrl}/api/v1/vocabulary?page={page}&pageSize={pageSize}";
            if (lastSync.HasValue)
            {
                url += $"&lastSync={lastSync.Value:yyyy-MM-ddTHH:mm:ss}";
            }

            var result = await SendRequestWithTokenHandlingAsync<VocabularyListResponse>(
                () => _httpClient.GetAsync(url));

            if (result.SuccessResult && result.Data != null)
            {
                var pagedResult = new PagedResult<Vocabulary>
                {
                    Items = result.Data.Data,
                    Page = result.Data.Pagination.Page,
                    PageSize = result.Data.Pagination.PageSize,
                    TotalCount = result.Data.Pagination.TotalCount,
                    TotalPages = result.Data.Pagination.TotalPages
                };
                return ServiceResult<PagedResult<Vocabulary>>.Success(pagedResult);
            }

            return ServiceResult<PagedResult<Vocabulary>>.Fail(result.Message, result.SessionExpired);
        }

        /// <summary>
        /// Lấy thông tin chi tiết từ vựng theo ID
        /// </summary>
        public async Task<ServiceResult<Vocabulary>> GetVocabularyByIdAsync(int id)
        {
            var result = await SendRequestWithTokenHandlingAsync<VocabularyResponse>(
                () => _httpClient.GetAsync($"{_baseUrl}/api/v1/vocabulary/{id}"));

            if (result.SuccessResult && result.Data != null && result.Data.Data != null)
            {
                return ServiceResult<Vocabulary>.Success(result.Data.Data);
            }

            return ServiceResult<Vocabulary>.Fail(result.Message, result.SessionExpired);
        }

        /// <summary>
        /// Tạo từ vựng mới
        /// </summary>
        public async Task<ServiceResult<Vocabulary>> CreateVocabularyAsync(CreateVocabularyRequest request)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var result = await SendRequestWithTokenHandlingAsync<VocabularyResponse>(
                () => _httpClient.PostAsync($"{_baseUrl}/api/v1/vocabulary", content));

            if (result.SuccessResult && result.Data != null && result.Data.Data != null)
            {
                return ServiceResult<Vocabulary>.Success(result.Data.Data);
            }

            return ServiceResult<Vocabulary>.Fail(result.Message, result.SessionExpired);
        }

        /// <summary>
        /// Cập nhật từ vựng
        /// </summary>
        public async Task<ServiceResult<Vocabulary>> UpdateVocabularyAsync(int id, UpdateVocabularyRequest request)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var result = await SendRequestWithTokenHandlingAsync<VocabularyResponse>(
                () => _httpClient.PutAsync($"{_baseUrl}/api/v1/vocabulary/{id}", content));

            if (result.SuccessResult && result.Data != null && result.Data.Data != null)
            {
                return ServiceResult<Vocabulary>.Success(result.Data.Data);
            }

            return ServiceResult<Vocabulary>.Fail(result.Message, result.SessionExpired);
        }

        /// <summary>
        /// Xóa từ vựng
        /// </summary>
        public async Task<ServiceResult<bool>> DeleteVocabularyAsync(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    return ServiceResult<bool>.Fail("Không có quyền truy cập. Vui lòng đăng nhập lại.");
                }

                var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/v1/vocabulary/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return ServiceResult<bool>.Success(true);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return ServiceResult<bool>.Fail("Không tìm thấy từ vựng với ID đã cho");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Thử làm mới token một lần
                    var refreshResult = await RefreshTokenAsync();
                    if (refreshResult.SuccessResult)
                    {
                        // Thử lại yêu cầu ban đầu
                        response = await _httpClient.DeleteAsync($"{_baseUrl}/api/v1/vocabulary/{id}");
                        if (response.IsSuccessStatusCode)
                        {
                            return ServiceResult<bool>.Success(true);
                        }
                    }

                    ClearAccessToken();
                    return ServiceResult<bool>.Fail("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ServiceResult<bool>.Fail($"Lỗi xóa từ vựng: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Lỗi kết nối khi xóa từ vựng");
                return ServiceResult<bool>.Fail("Lỗi kết nối đến server. Vui lòng kiểm tra kết nối mạng.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi xóa từ vựng");
                return ServiceResult<bool>.Fail($"Lỗi không xác định: {ex.Message}");
            }
        }

        /// <summary>
        /// Làm mới token
        /// </summary>
        public async Task<ServiceResult<string>> RefreshTokenAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    return ServiceResult<string>.Fail("Không có token để làm mới");
                }

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/auth/refresh", null);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<RefreshTokenResponse>(
                        responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (result != null && !string.IsNullOrEmpty(result.Token))
                    {
                        // Cập nhật token mới
                        SetAccessToken(result.Token, result.ExpiresAt ?? DateTime.Now.AddHours(8));
                        return ServiceResult<string>.Success(result.Token);
                    }
                    else
                    {
                        ClearAccessToken();
                        return ServiceResult<string>.Fail("Không thể phân tích token mới từ phản hồi server");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ClearAccessToken();
                    return ServiceResult<string>.Fail("Token không hợp lệ hoặc đã hết hạn. Vui lòng đăng nhập lại.", true);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ServiceResult<string>.Fail($"Lỗi làm mới token: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Lỗi kết nối khi làm mới token");
                return ServiceResult<string>.Fail("Lỗi kết nối đến server. Vui lòng kiểm tra kết nối mạng.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi làm mới token");
                return ServiceResult<string>.Fail($"Lỗi không xác định: {ex.Message}");
            }
        }
    }
}