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
        private readonly HttpClient _httpClient;
        private string _accessToken = string.Empty;
        private DateTime _tokenExpiration = DateTime.MinValue;
        private string _baseUrl;

        public ApiService(ILogger<ApiService> logger, IAppSettingsService appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
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

            // Lấy access token từ cài đặt nếu có
            _accessToken = _appSettings.AccessToken ?? string.Empty;
            if (!string.IsNullOrEmpty(_accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _accessToken);
            }
        }

        /// <summary>
        /// Thiết lập access token cho các yêu cầu tiếp theo
        /// </summary>
        public void SetAccessToken(string token)
        {
            _accessToken = token;
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            _appSettings.AccessToken = token;
        }

        /// <summary>
        /// Xóa access token khi đăng xuất
        /// </summary>
        public void ClearAccessToken()
        {
            _accessToken = string.Empty;
            _httpClient.DefaultRequestHeaders.Authorization = null;
            _appSettings.AccessToken = string.Empty;
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
                        SetAccessToken(loginResponse.Token);
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
        /// Lấy danh sách từ vựng từ API
        /// </summary>
        public async Task<ServiceResult<PagedResult<Vocabulary>>> GetVocabularyAsync(int page = 1, int pageSize = 20, DateTime? lastSync = null)
        {
            try
            {
                // Kiểm tra token trước khi thực hiện request
                if (string.IsNullOrEmpty(_accessToken))
                {
                    return ServiceResult<PagedResult<Vocabulary>>.Fail("Không có quyền truy cập. Vui lòng đăng nhập lại.");
                }

                string url = $"{_baseUrl}/api/v1/vocabulary?page={page}&pageSize={pageSize}";
                if (lastSync.HasValue)
                {
                    url += $"&lastSync={lastSync.Value:yyyy-MM-ddTHH:mm:ss}";
                }

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<VocabularyListResponse>(
                        content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (result != null && result.Data != null)
                    {
                        var pagedResult = new PagedResult<Vocabulary>
                        {
                            Items = result.Data,
                            Page = result.Pagination.Page,
                            PageSize = result.Pagination.PageSize,
                            TotalCount = result.Pagination.TotalCount,
                            TotalPages = result.Pagination.TotalPages
                        };
                        return ServiceResult<PagedResult<Vocabulary>>.Success(pagedResult);
                    }
                    else
                    {
                        return ServiceResult<PagedResult<Vocabulary>>.Fail("Không thể phân tích dữ liệu từ server");
                    }
                }
                else
                {
                    // Xử lý trường hợp token hết hạn
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        ClearAccessToken();
                        return ServiceResult<PagedResult<Vocabulary>>.Fail("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        return ServiceResult<PagedResult<Vocabulary>>.Fail($"Lỗi lấy dữ liệu: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Lỗi kết nối khi lấy danh sách từ vựng");
                return ServiceResult<PagedResult<Vocabulary>>.Fail("Lỗi kết nối đến server. Vui lòng kiểm tra kết nối mạng.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi lấy danh sách từ vựng");
                return ServiceResult<PagedResult<Vocabulary>>.Fail($"Lỗi không xác định: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết từ vựng theo ID
        /// </summary>
        public async Task<ServiceResult<Vocabulary>> GetVocabularyByIdAsync(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    return ServiceResult<Vocabulary>.Fail("Không có quyền truy cập. Vui lòng đăng nhập lại.");
                }

                var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/vocabulary/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<VocabularyResponse>(
                        content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (result != null && result.Data != null)
                    {
                        return ServiceResult<Vocabulary>.Success(result.Data);
                    }
                    else
                    {
                        return ServiceResult<Vocabulary>.Fail("Không tìm thấy từ vựng yêu cầu");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return ServiceResult<Vocabulary>.Fail("Không tìm thấy từ vựng với ID đã cho");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ClearAccessToken();
                    return ServiceResult<Vocabulary>.Fail("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ServiceResult<Vocabulary>.Fail($"Lỗi lấy dữ liệu: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Lỗi kết nối khi lấy thông tin từ vựng");
                return ServiceResult<Vocabulary>.Fail("Lỗi kết nối đến server. Vui lòng kiểm tra kết nối mạng.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi lấy thông tin từ vựng");
                return ServiceResult<Vocabulary>.Fail($"Lỗi không xác định: {ex.Message}");
            }
        }

        /// <summary>
        /// Tạo từ vựng mới
        /// </summary>
        public async Task<ServiceResult<Vocabulary>> CreateVocabularyAsync(CreateVocabularyRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    return ServiceResult<Vocabulary>.Fail("Không có quyền truy cập. Vui lòng đăng nhập lại.");
                }

                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/vocabulary", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<VocabularyResponse>(
                        responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (result != null && result.Data != null)
                    {
                        return ServiceResult<Vocabulary>.Success(result.Data);
                    }
                    else
                    {
                        return ServiceResult<Vocabulary>.Fail("Không thể phân tích dữ liệu phản hồi từ server");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ClearAccessToken();
                    return ServiceResult<Vocabulary>.Fail("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ServiceResult<Vocabulary>.Fail($"Dữ liệu không hợp lệ: {errorContent}");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ServiceResult<Vocabulary>.Fail($"Lỗi tạo từ vựng: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Lỗi kết nối khi tạo từ vựng mới");
                return ServiceResult<Vocabulary>.Fail("Lỗi kết nối đến server. Vui lòng kiểm tra kết nối mạng.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi tạo từ vựng mới");
                return ServiceResult<Vocabulary>.Fail($"Lỗi không xác định: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật từ vựng
        /// </summary>
        public async Task<ServiceResult<Vocabulary>> UpdateVocabularyAsync(int id, UpdateVocabularyRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    return ServiceResult<Vocabulary>.Fail("Không có quyền truy cập. Vui lòng đăng nhập lại.");
                }

                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PutAsync($"{_baseUrl}/api/v1/vocabulary/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<VocabularyResponse>(
                        responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (result != null && result.Data != null)
                    {
                        return ServiceResult<Vocabulary>.Success(result.Data);
                    }
                    else
                    {
                        return ServiceResult<Vocabulary>.Fail("Không thể phân tích dữ liệu phản hồi từ server");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return ServiceResult<Vocabulary>.Fail("Không tìm thấy từ vựng với ID đã cho");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ClearAccessToken();
                    return ServiceResult<Vocabulary>.Fail("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ServiceResult<Vocabulary>.Fail($"Dữ liệu không hợp lệ: {errorContent}");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ServiceResult<Vocabulary>.Fail($"Lỗi cập nhật từ vựng: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Lỗi kết nối khi cập nhật từ vựng");
                return ServiceResult<Vocabulary>.Fail("Lỗi kết nối đến server. Vui lòng kiểm tra kết nối mạng.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi cập nhật từ vựng");
                return ServiceResult<Vocabulary>.Fail($"Lỗi không xác định: {ex.Message}");
            }
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
                        SetAccessToken(result.Token);
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