using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using LexiFlow.Core.Entities;
using LexiFlow.Core.Interfaces;
using LexiFlow.Core.Models;
using LexiFlow.Core.Models.Requests;
using LexiFlow.Core.Models.Responses;

namespace LexiFlow.Core.Services
{
    /// <summary>
    /// Triển khai dịch vụ API cho giao tiếp với server LexiFlow
    /// </summary>
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IAppSettingsService _appSettingsService;
        private readonly ILogger<ApiService> _logger;
        private string _baseUrl;
        private string _accessToken;

        public ApiService(IAppSettingsService appSettingsService, ILogger<ApiService> logger)
        {
            _appSettingsService = appSettingsService;
            _logger = logger;
            _httpClient = new HttpClient();
            InitializeHttpClient();
        }

        private void InitializeHttpClient()
        {
            // Khởi tạo các cài đặt cơ bản cho HttpClient
            _baseUrl = _appSettingsService.ApiBaseUrl;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.Timeout = TimeSpan.FromSeconds(30);

            // Lấy access token từ cài đặt nếu có
            _accessToken = _appSettingsService.AccessToken;
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
            _appSettingsService.AccessToken = token;
        }

        /// <summary>
        /// Xóa access token khi đăng xuất
        /// </summary>
        public void ClearAccessToken()
        {
            _accessToken = null;
            _httpClient.DefaultRequestHeaders.Authorization = null;
            _appSettingsService.AccessToken = null;
        }

        /// <summary>
        /// Thực hiện đăng nhập và lấy token
        /// </summary>
        public async Task<ApiResponse<LoginResponse>> LoginAsync(string username, string password)
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
                    }

                    return ApiResponse<LoginResponse>.Success(loginResponse);
                }
                else
                {
                    // Xử lý các mã lỗi HTTP
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return ApiResponse<LoginResponse>.Fail("Thông tin đăng nhập không hợp lệ");
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        return ApiResponse<LoginResponse>.Fail($"Lỗi đăng nhập: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Lỗi kết nối khi đăng nhập");
                return ApiResponse<LoginResponse>.Fail("Lỗi kết nối đến server. Vui lòng kiểm tra kết nối mạng.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi đăng nhập");
                return ApiResponse<LoginResponse>.Fail($"Lỗi không xác định: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy danh sách từ vựng từ API
        /// </summary>
        public async Task<ApiResponse<PagedResult<Vocabulary>>> GetVocabularyAsync(int page = 1, int pageSize = 20)
        {
            try
            {
                // Kiểm tra token trước khi thực hiện request
                if (string.IsNullOrEmpty(_accessToken))
                {
                    return ApiResponse<PagedResult<Vocabulary>>.Fail("Không có quyền truy cập. Vui lòng đăng nhập lại.");
                }

                var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/vocabulary?page={page}&pageSize={pageSize}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<VocabularyListResponse>(
                        content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (result != null)
                    {
                        var pagedResult = new PagedResult<Vocabulary>
                        {
                            Items = result.Data,
                            Page = result.Pagination.Page,
                            PageSize = result.Pagination.PageSize,
                            TotalCount = result.Pagination.TotalCount,
                            TotalPages = result.Pagination.TotalPages
                        };
                        return ApiResponse<PagedResult<Vocabulary>>.Success(pagedResult);
                    }
                    else
                    {
                        return ApiResponse<PagedResult<Vocabulary>>.Fail("Không thể phân tích dữ liệu từ server");
                    }
                }
                else
                {
                    // Xử lý trường hợp token hết hạn
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        ClearAccessToken();
                        return ApiResponse<PagedResult<Vocabulary>>.Fail("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        return ApiResponse<PagedResult<Vocabulary>>.Fail($"Lỗi lấy dữ liệu: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Lỗi kết nối khi lấy danh sách từ vựng");
                return ApiResponse<PagedResult<Vocabulary>>.Fail("Lỗi kết nối đến server. Vui lòng kiểm tra kết nối mạng.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi lấy danh sách từ vựng");
                return ApiResponse<PagedResult<Vocabulary>>.Fail($"Lỗi không xác định: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết từ vựng theo ID
        /// </summary>
        public async Task<ApiResponse<Vocabulary>> GetVocabularyByIdAsync(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    return ApiResponse<Vocabulary>.Fail("Không có quyền truy cập. Vui lòng đăng nhập lại.");
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
                        return ApiResponse<Vocabulary>.Success(result.Data);
                    }
                    else
                    {
                        return ApiResponse<Vocabulary>.Fail("Không tìm thấy từ vựng yêu cầu");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return ApiResponse<Vocabulary>.Fail("Không tìm thấy từ vựng với ID đã cho");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ClearAccessToken();
                    return ApiResponse<Vocabulary>.Fail("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ApiResponse<Vocabulary>.Fail($"Lỗi lấy dữ liệu: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Lỗi kết nối khi lấy thông tin từ vựng");
                return ApiResponse<Vocabulary>.Fail("Lỗi kết nối đến server. Vui lòng kiểm tra kết nối mạng.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi lấy thông tin từ vựng");
                return ApiResponse<Vocabulary>.Fail($"Lỗi không xác định: {ex.Message}");
            }
        }

        /// <summary>
        /// Tạo từ vựng mới
        /// </summary>
        public async Task<ApiResponse<Vocabulary>> CreateVocabularyAsync(CreateVocabularyRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    return ApiResponse<Vocabulary>.Fail("Không có quyền truy cập. Vui lòng đăng nhập lại.");
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
                        return ApiResponse<Vocabulary>.Success(result.Data);
                    }
                    else
                    {
                        return ApiResponse<Vocabulary>.Fail("Không thể phân tích dữ liệu phản hồi từ server");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ClearAccessToken();
                    return ApiResponse<Vocabulary>.Fail("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ApiResponse<Vocabulary>.Fail($"Dữ liệu không hợp lệ: {errorContent}");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ApiResponse<Vocabulary>.Fail($"Lỗi tạo từ vựng: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Lỗi kết nối khi tạo từ vựng mới");
                return ApiResponse<Vocabulary>.Fail("Lỗi kết nối đến server. Vui lòng kiểm tra kết nối mạng.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi tạo từ vựng mới");
                return ApiResponse<Vocabulary>.Fail($"Lỗi không xác định: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật từ vựng
        /// </summary>
        public async Task<ApiResponse<Vocabulary>> UpdateVocabularyAsync(int id, UpdateVocabularyRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    return ApiResponse<Vocabulary>.Fail("Không có quyền truy cập. Vui lòng đăng nhập lại.");
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
                        return ApiResponse<Vocabulary>.Success(result.Data);
                    }
                    else
                    {
                        return ApiResponse<Vocabulary>.Fail("Không thể phân tích dữ liệu phản hồi từ server");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return ApiResponse<Vocabulary>.Fail("Không tìm thấy từ vựng với ID đã cho");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ClearAccessToken();
                    return ApiResponse<Vocabulary>.Fail("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ApiResponse<Vocabulary>.Fail($"Dữ liệu không hợp lệ: {errorContent}");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ApiResponse<Vocabulary>.Fail($"Lỗi cập nhật từ vựng: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Lỗi kết nối khi cập nhật từ vựng");
                return ApiResponse<Vocabulary>.Fail("Lỗi kết nối đến server. Vui lòng kiểm tra kết nối mạng.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi cập nhật từ vựng");
                return ApiResponse<Vocabulary>.Fail($"Lỗi không xác định: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa từ vựng
        /// </summary>
        public async Task<ApiResponse<bool>> DeleteVocabularyAsync(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    return ApiResponse<bool>.Fail("Không có quyền truy cập. Vui lòng đăng nhập lại.");
                }

                var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/v1/vocabulary/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return ApiResponse<bool>.Success(true);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return ApiResponse<bool>.Fail("Không tìm thấy từ vựng với ID đã cho");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ClearAccessToken();
                    return ApiResponse<bool>.Fail("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ApiResponse<bool>.Fail($"Lỗi xóa từ vựng: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Lỗi kết nối khi xóa từ vựng");
                return ApiResponse<bool>.Fail("Lỗi kết nối đến server. Vui lòng kiểm tra kết nối mạng.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi xóa từ vựng");
                return ApiResponse<bool>.Fail($"Lỗi không xác định: {ex.Message}");
            }
        }

        /// <summary>
        /// Đồng bộ hóa dữ liệu với server
        /// </summary>
        public async Task<ApiResponse<SyncResult>> SyncDataAsync(SyncRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    return ApiResponse<SyncResult>.Fail("Không có quyền truy cập. Vui lòng đăng nhập lại.");
                }

                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/sync", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<SyncResponse>(
                        responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (result != null && result.Data != null)
                    {
                        return ApiResponse<SyncResult>.Success(result.Data);
                    }
                    else
                    {
                        return ApiResponse<SyncResult>.Fail("Không thể phân tích dữ liệu phản hồi từ server");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ClearAccessToken();
                    return ApiResponse<SyncResult>.Fail("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.", true);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ApiResponse<SyncResult>.Fail($"Lỗi đồng bộ dữ liệu: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Lỗi kết nối khi đồng bộ dữ liệu");
                return ApiResponse<SyncResult>.Fail("Lỗi kết nối đến server. Vui lòng kiểm tra kết nối mạng.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi đồng bộ dữ liệu");
                return ApiResponse<SyncResult>.Fail($"Lỗi không xác định: {ex.Message}");
            }
        }

        /// <summary>
        /// Làm mới token
        /// </summary>
        public async Task<ApiResponse<string>> RefreshTokenAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    return ApiResponse<string>.Fail("Không có token để làm mới");
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
                        return ApiResponse<string>.Success(result.Token);
                    }
                    else
                    {
                        ClearAccessToken();
                        return ApiResponse<string>.Fail("Không thể phân tích token mới từ phản hồi server");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ClearAccessToken();
                    return ApiResponse<string>.Fail("Token không hợp lệ hoặc đã hết hạn. Vui lòng đăng nhập lại.", true);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ApiResponse<string>.Fail($"Lỗi làm mới token: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Lỗi kết nối khi làm mới token");
                return ApiResponse<string>.Fail("Lỗi kết nối đến server. Vui lòng kiểm tra kết nối mạng.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi làm mới token");
                return ApiResponse<string>.Fail($"Lỗi không xác định: {ex.Message}");
            }
        }
    }
}
