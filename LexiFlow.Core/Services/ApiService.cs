// LexiFlow.Core/Services/ApiService.cs (Complete implementation)

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LexiFlow.Core.Interfaces;
using LexiFlow.Core.Models;
using LexiFlow.Core.Models.Requests;
using LexiFlow.Core.Models.Responses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LexiFlow.Core.Services
{
    /// <summary>
    /// Implementation of API service
    /// </summary>
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;
        private readonly ApiSettings _apiSettings;
        private readonly ISecureStorage _secureStorage;
        private readonly JsonSerializerOptions _jsonOptions;

        private const string AccessTokenKey = "LexiFlow_AccessToken";
        private const string RefreshTokenKey = "LexiFlow_RefreshToken";

        public ApiService(
            HttpClient httpClient,
            IOptions<ApiSettings> apiSettings,
            ILogger<ApiService> logger,
            ISecureStorage secureStorage)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiSettings = apiSettings?.Value ?? throw new ArgumentNullException(nameof(apiSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _secureStorage = secureStorage ?? throw new ArgumentNullException(nameof(secureStorage));

            // Configure HTTP client
            _httpClient.BaseAddress = new Uri(_apiSettings.BaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Configure JSON options
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            // Setup access token if available
            var accessToken = _secureStorage.GetValue(AccessTokenKey);
            if (!string.IsNullOrEmpty(accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        /// <summary>
        /// Thực hiện đăng nhập và lấy token
        /// </summary>
        public async Task<ServiceResult<LoginResponse>> LoginAsync(string username, string password)
        {
            try
            {
                var loginRequest = new LoginRequest
                {
                    Username = username,
                    Password = password
                };

                var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest, _jsonOptions);

                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>(_jsonOptions);

                    if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.AccessToken))
                    {
                        // Save tokens to secure storage
                        _secureStorage.SetValue(AccessTokenKey, loginResponse.AccessToken);
                        _secureStorage.SetValue(RefreshTokenKey, loginResponse.RefreshToken);

                        // Update HTTP client authorization header
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.AccessToken);

                        return ServiceResult<LoginResponse>.Success(loginResponse);
                    }

                    return ServiceResult<LoginResponse>.Fail("Invalid response from server");
                }

                // Handle authentication errors
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return ServiceResult<LoginResponse>.Fail("Invalid username or password");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return ServiceResult<LoginResponse>.Fail($"Login failed: {response.StatusCode} - {errorContent}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error during login");
                return ServiceResult<LoginResponse>.Fail("Connection error. Please check your internet connection.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login");
                return ServiceResult<LoginResponse>.Fail($"Login error: {ex.Message}");
            }
        }

        /// <summary>
        /// Làm mới token
        /// </summary>
        public async Task<ServiceResult<string>> RefreshTokenAsync()
        {
            try
            {
                var refreshToken = _secureStorage.GetValue(RefreshTokenKey);
                if (string.IsNullOrEmpty(refreshToken))
                {
                    return ServiceResult<string>.Fail("No refresh token available");
                }

                var refreshRequest = new RefreshTokenRequest
                {
                    RefreshToken = refreshToken
                };

                var response = await _httpClient.PostAsJsonAsync("api/auth/refresh", refreshRequest, _jsonOptions);

                if (response.IsSuccessStatusCode)
                {
                    var refreshResponse = await response.Content.ReadFromJsonAsync<RefreshTokenResponse>(_jsonOptions);

                    if (refreshResponse != null && !string.IsNullOrEmpty(refreshResponse.AccessToken))
                    {
                        // Update tokens in secure storage
                        _secureStorage.SetValue(AccessTokenKey, refreshResponse.AccessToken);
                        if (!string.IsNullOrEmpty(refreshResponse.RefreshToken))
                        {
                            _secureStorage.SetValue(RefreshTokenKey, refreshResponse.RefreshToken);
                        }

                        // Update HTTP client authorization header
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", refreshResponse.AccessToken);

                        return ServiceResult<string>.Success(refreshResponse.AccessToken);
                    }

                    return ServiceResult<string>.Fail("Invalid response from server");
                }

                // Handle token refresh errors
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Clear tokens if refresh fails
                    ClearAccessToken();
                    return ServiceResult<string>.Fail("Session expired. Please log in again.");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return ServiceResult<string>.Fail($"Token refresh failed: {response.StatusCode} - {errorContent}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token");
                return ServiceResult<string>.Fail($"Token refresh error: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy token hiện tại
        /// </summary>
        public string GetCurrentToken()
        {
            return _secureStorage.GetValue(AccessTokenKey);
        }

        /// <summary>
        /// Xóa access token khi đăng xuất
        /// </summary>
        public void ClearAccessToken()
        {
            _secureStorage.DeleteValue(AccessTokenKey);
            _secureStorage.DeleteValue(RefreshTokenKey);
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        /// <summary>
        /// Lấy danh sách từ vựng từ API
        /// </summary>
        public async Task<ServiceResult<PagedResult<Vocabulary>>> GetVocabularyAsync(int page = 1, int pageSize = 20, DateTime? lastSync = null)
        {
            try
            {
                var url = $"api/vocabulary?page={page}&pageSize={pageSize}";
                if (lastSync.HasValue)
                {
                    url += $"&lastSync={lastSync.Value:o}";
                }

                return await SendAuthorizedRequestAsync<PagedResult<Vocabulary>>(HttpMethod.Get, url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabulary");
                return ServiceResult<PagedResult<Vocabulary>>.Fail($"Error getting vocabulary: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết từ vựng theo ID
        /// </summary>
        public async Task<ServiceResult<Vocabulary>> GetVocabularyByIdAsync(int id)
        {
            try
            {
                return await SendAuthorizedRequestAsync<Vocabulary>(HttpMethod.Get, $"api/vocabulary/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabulary by ID {Id}", id);
                return ServiceResult<Vocabulary>.Fail($"Error getting vocabulary: {ex.Message}");
            }
        }

        /// <summary>
        /// Tạo từ vựng mới
        /// </summary>
        public async Task<ServiceResult<Vocabulary>> CreateVocabularyAsync(CreateVocabularyRequest request)
        {
            try
            {
                return await SendAuthorizedRequestAsync<Vocabulary>(HttpMethod.Post, "api/vocabulary", request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vocabulary");
                return ServiceResult<Vocabulary>.Fail($"Error creating vocabulary: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật từ vựng
        /// </summary>
        public async Task<ServiceResult<Vocabulary>> UpdateVocabularyAsync(int id, UpdateVocabularyRequest request)
        {
            try
            {
                return await SendAuthorizedRequestAsync<Vocabulary>(HttpMethod.Put, $"api/vocabulary/{id}", request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vocabulary {Id}", id);
                return ServiceResult<Vocabulary>.Fail($"Error updating vocabulary: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa từ vựng
        /// </summary>
        public async Task<ServiceResult<bool>> DeleteVocabularyAsync(int id)
        {
            try
            {
                return await SendAuthorizedRequestAsync<bool>(HttpMethod.Delete, $"api/vocabulary/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vocabulary {Id}", id);
                return ServiceResult<bool>.Fail($"Error deleting vocabulary: {ex.Message}");
            }
        }

        /// <summary>
        /// Đồng bộ dữ liệu với server
        /// </summary>
        public async Task<ServiceResult<SyncResult>> SyncDataAsync(SyncRequest request)
        {
            try
            {
                return await SendAuthorizedRequestAsync<SyncResult>(HttpMethod.Post, "api/sync", request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error synchronizing data");
                return ServiceResult<SyncResult>.Fail($"Error synchronizing data: {ex.Message}");
            }
        }

        #region Helper Methods

        /// <summary>
        /// Send authorized request with automatic token refresh
        /// </summary>
        private async Task<ServiceResult<T>> SendAuthorizedRequestAsync<T>(HttpMethod method, string url, object content = null)
        {
            try
            {
                // First attempt
                var response = await SendRequestWithAuthAsync(method, url, content);

                // Check if unauthorized
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Try to refresh token
                    var refreshResult = await RefreshTokenAsync();
                    if (!refreshResult.Success)
                    {
                        return ServiceResult<T>.Fail("Session expired. Please log in again.", true);
                    }

                    // Retry with new token
                    response = await SendRequestWithAuthAsync(method, url, content);
                }

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
                    return ServiceResult<T>.Success(result);
                }

                // Handle API errors
                var errorContent = await response.Content.ReadAsStringAsync();
                return ServiceResult<T>.Fail($"API error: {response.StatusCode} - {errorContent}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error");
                return ServiceResult<T>.Fail("Connection error. Please check your internet connection.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error");
                return ServiceResult<T>.Fail($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Send HTTP request with authorization header
        /// </summary>
        private async Task<HttpResponseMessage> SendRequestWithAuthAsync(HttpMethod method, string url, object content = null)
        {
            var request = new HttpRequestMessage(method, url);

            // Add content if provided
            if (content != null)
            {
                var json = JsonSerializer.Serialize(content, _jsonOptions);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            // Add authorization header
            var accessToken = GetCurrentToken();
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            return await _httpClient.SendAsync(request);
        }

        #endregion
    }
}