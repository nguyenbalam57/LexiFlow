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

            // Nếu có token, thêm vào header
            var tokenData = _tokenManager.GetToken();
            if (tokenData != null && !string.IsNullOrEmpty(tokenData.AccessToken))
            {
                _accessToken = tokenData.AccessToken;
                SetAuthorizationHeader();
            }
        }

        private void SetAuthorizationHeader()
        {
            if (!string.IsNullOrEmpty(_accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _accessToken);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        /// <summary>
        /// Lấy token hiện tại
        /// </summary>
        public string GetCurrentToken()
        {
            return _accessToken;
        }

        /// <summary>
        /// Xóa access token khi đăng xuất
        /// </summary>
        public void ClearAccessToken()
        {
            _accessToken = string.Empty;
            _httpClient.DefaultRequestHeaders.Authorization = null;
            _tokenManager.SaveToken(string.Empty, DateTime.MinValue);
        }

        /// <summary>
        /// Thực hiện đăng nhập và lấy token
        /// </summary>
        public async Task<ServiceResult<LoginResponse>> LoginAsync(string username, string password)
        {
            try
            {
                // Tạo dữ liệu đăng nhập
                var loginData = new
                {
                    Username = username,
                    Password = password
                };

                // Chuyển đổi thành JSON và gửi yêu cầu
                var content = new StringContent(
                    JsonSerializer.Serialize(loginData),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/auth/login", content);

                // Kiểm tra phản hồi thành công
                if (response.IsSuccessStatusCode)
                {
                    // Đọc dữ liệu phản hồi
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonSerializer.Deserialize<LoginResponse>(
                        responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                    {
                        // Lưu token
                        _accessToken = loginResponse.Token;
                        SetAuthorizationHeader();

                        // Lưu token vào TokenManager
                        DateTime expiresAt = loginResponse.ExpiresAt ?? DateTime.UtcNow.AddHours(1);
                        _tokenManager.SaveToken(_accessToken, expiresAt);

                        return ServiceResult<LoginResponse>.Success(loginResponse);
                    }
                    else
                    {
                        return ServiceResult<LoginResponse>.Fail("Phản hồi đăng nhập không hợp lệ");
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ServiceResult<LoginResponse>.Fail($"Lỗi đăng nhập: {response.StatusCode} - {errorContent}");
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
        /// Làm mới token
        /// </summary>
        public async Task<ServiceResult<string>> RefreshTokenAsync()
        {
            try
            {
                var tokenData = _tokenManager.GetToken();
                if (tokenData == null)
                {
                    return ServiceResult<string>.Fail("Không có token để làm mới");
                }

                var refreshData = new
                {
                    Token = _accessToken
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(refreshData),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/auth/refresh", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var refreshResponse = JsonSerializer.Deserialize<RefreshTokenResponse>(
                        responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (refreshResponse != null && !string.IsNullOrEmpty(refreshResponse.Token))
                    {
                        // Cập nhật token mới
                        _accessToken = refreshResponse.Token;
                        SetAuthorizationHeader();

                        // Lưu token mới
                        DateTime expiresAt = refreshResponse.ExpiresAt ?? DateTime.UtcNow.AddHours(1);
                        _tokenManager.SaveToken(_accessToken, expiresAt);

                        return ServiceResult<string>.Success(_accessToken);
                    }
                    else
                    {
                        return ServiceResult<string>.Fail("Phản hồi làm mới token không hợp lệ");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ClearAccessToken();
                    return ServiceResult<string>.Fail("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
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
            var result = await SendRequestWithTokenHandlingAsync<ApiResponse<Vocabulary>>(
                () => _httpClient.GetAsync($"{_baseUrl}/api/v1/vocabulary/{id}"));

            if (result.SuccessResult && result.Data != null)
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
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    return ServiceResult<Vocabulary>.Fail("Không có quyền truy cập. Vui lòng đăng nhập lại.");
                }

                var requestContent = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/vocabulary", requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<Vocabulary>>(
                        content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (apiResponse != null && apiResponse.SuccessResponse)
                    {
                        return ServiceResult<Vocabulary>.Success(apiResponse.Data);
                    }
                    else
                    {
                        return ServiceResult<Vocabulary>.Fail(apiResponse?.Message ?? "Phản hồi không hợp lệ");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Thử làm mới token một lần
                    var refreshResult = await RefreshTokenAsync();
                    if (refreshResult.SuccessResult)
                    {
                        // Thử lại yêu cầu ban đầu
                        var retryRequestContent = new StringContent(
                            JsonSerializer.Serialize(request),
                            Encoding.UTF8,
                            "application/json");

                        response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/vocabulary", retryRequestContent);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var apiResponse = JsonSerializer.Deserialize<ApiResponse<Vocabulary>>(
                                content,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                            if (apiResponse != null && apiResponse.SuccessResponse)
                            {
                                return ServiceResult<Vocabulary>.Success(apiResponse.Data);
                            }
                        }
                    }

                    ClearAccessToken();
                    return ServiceResult<Vocabulary>.Fail("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ServiceResult<Vocabulary>.Fail($"Lỗi tạo từ vựng: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Lỗi kết nối khi tạo từ vựng");
                return ServiceResult<Vocabulary>.Fail("Lỗi kết nối đến server. Vui lòng kiểm tra kết nối mạng.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi tạo từ vựng");
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

                var requestContent = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PutAsync($"{_baseUrl}/api/v1/vocabulary/{id}", requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<Vocabulary>>(
                        content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (apiResponse != null && apiResponse.SuccessResponse)
                    {
                        return ServiceResult<Vocabulary>.Success(apiResponse.Data);
                    }
                    else
                    {
                        return ServiceResult<Vocabulary>.Fail(apiResponse?.Message ?? "Phản hồi không hợp lệ");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Thử làm mới token
                    var refreshResult = await RefreshTokenAsync();
                    if (refreshResult.SuccessResult)
                    {
                        // Thử lại yêu cầu ban đầu
                        var retryRequestContent = new StringContent(
                            JsonSerializer.Serialize(request),
                            Encoding.UTF8,
                            "application/json");

                        response = await _httpClient.PutAsync($"{_baseUrl}/api/v1/vocabulary/{id}", retryRequestContent);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var apiResponse = JsonSerializer.Deserialize<ApiResponse<Vocabulary>>(
                                content,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                            if (apiResponse != null && apiResponse.SuccessResponse)
                            {
                                return ServiceResult<Vocabulary>.Success(apiResponse.Data);
                            }
                        }
                    }

                    ClearAccessToken();
                    return ServiceResult<Vocabulary>.Fail("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
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
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Thử làm mới token
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
        /// Đồng bộ dữ liệu với server
        /// </summary>
        public async Task<ServiceResult<SyncResult>> SyncDataAsync(SyncRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    return ServiceResult<SyncResult>.Fail("Không có quyền truy cập. Vui lòng đăng nhập lại.");
                }

                var requestContent = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/sync", requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<SyncResult>(
                        content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return ServiceResult<SyncResult>.Success(result);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Thử làm mới token một lần
                    var refreshResult = await RefreshTokenAsync();
                    if (refreshResult.SuccessResult)
                    {
                        // Thử lại yêu cầu ban đầu
                        var retryRequestContent = new StringContent(
                            JsonSerializer.Serialize(request),
                            Encoding.UTF8,
                            "application/json");

                        response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/sync", retryRequestContent);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonSerializer.Deserialize<SyncResult>(
                                content,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                            return ServiceResult<SyncResult>.Success(result);
                        }
                    }

                    ClearAccessToken();
                    return ServiceResult<SyncResult>.Fail("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ServiceResult<SyncResult>.Fail($"Lỗi đồng bộ: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Lỗi kết nối khi đồng bộ dữ liệu");
                return ServiceResult<SyncResult>.Fail("Lỗi kết nối đến server. Vui lòng kiểm tra kết nối mạng.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi đồng bộ dữ liệu");
                return ServiceResult<SyncResult>.Fail($"Lỗi không xác định: {ex.Message}");
            }
        }

    }
}