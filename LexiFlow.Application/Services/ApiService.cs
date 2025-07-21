using LexiFlow.Core.Entities;
using LexiFlow.Core.Interfaces;
using LexiFlow.Core.Models;
using LexiFlow.Core.Models.Requests;
using LexiFlow.Core.Models.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LexiFlow.App.Services
{
    public class ApiService : IApiService
    {
        private readonly ILogger<ApiService> _logger;
        private readonly IAppSettingsService _appSettings;
        private readonly HttpClient _httpClient;
        private string _accessToken = string.Empty;
        private DateTime _tokenExpiration = DateTime.MinValue;

        public ApiService(ILogger<ApiService> logger, IAppSettingsService appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_appSettings.ApiUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ServiceResult<LoginResponse>> LoginAsync(string username, string password)
        {
            try
            {
                // Create login request
                var loginData = new
                {
                    Username = username,
                    Password = password
                };

                // Serialize and send request
                var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("auth/login", content);

                // Check for successful response
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize response
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var responseData = await JsonSerializer.DeserializeAsync<LoginResponse>(
                        await response.Content.ReadAsStreamAsync(), options);

                    if (responseData != null && responseData.Success)
                    {
                        // Store token
                        _accessToken = responseData.Token ?? string.Empty;
                        _tokenExpiration = responseData.ExpiresAt ?? DateTime.Now.AddHours(1);

                        // Set authorization header for future requests
                        _httpClient.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", _accessToken);

                        return ServiceResult<LoginResponse>.Success(responseData);
                    }
                }

                // Handle unsuccessful login
                return ServiceResult<LoginResponse>.Fail("Invalid username or password");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login attempt");
                return ServiceResult<LoginResponse>.Fail($"Login error: {ex.Message}");
            }
        }

        public async Task<ServiceResult<string>> RefreshTokenAsync()
        {
            try
            {
                // Ensure we have a token to refresh
                if (string.IsNullOrEmpty(_accessToken))
                {
                    return ServiceResult<string>.Fail("No active session to refresh");
                }

                // Send refresh token request
                var response = await _httpClient.PostAsync("auth/refresh", null);

                // Check for successful response
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize response
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var responseData = await JsonSerializer.DeserializeAsync<dynamic>(
                        await response.Content.ReadAsStreamAsync(), options);

                    // Update token
                    if (responseData != null)
                    {
                        _accessToken = responseData.token.ToString();
                        _tokenExpiration = DateTime.Parse(responseData.expiresAt.ToString());

                        // Update authorization header
                        _httpClient.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", _accessToken);

                        return ServiceResult<string>.Success(_accessToken);
                    }
                }

                // Handle unsuccessful refresh
                ClearAccessToken();
                return ServiceResult<string>.Fail("Failed to refresh session", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token");
                ClearAccessToken();
                return ServiceResult<string>.Fail($"Refresh error: {ex.Message}", true);
            }
        }

        public string GetCurrentToken()
        {
            // Check if token is expired
            if (!string.IsNullOrEmpty(_accessToken) && _tokenExpiration > DateTime.Now)
            {
                return _accessToken;
            }

            return string.Empty;
        }

        public void ClearAccessToken()
        {
            _accessToken = string.Empty;
            _tokenExpiration = DateTime.MinValue;
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<ServiceResult<UserDto>> GetUserProfileAsync()
        {
            // Implement user profile retrieval
            return await Task.FromResult(ServiceResult<UserDto>.Fail("Not implemented"));
        }

        public async Task<ServiceResult<PagedResult<Vocabulary>>> GetVocabularyAsync(int page = 1, int pageSize = 20, DateTime? lastSync = null)
        {
            // Implement vocabulary list retrieval
            return await Task.FromResult(ServiceResult<PagedResult<Vocabulary>>.Fail("Not implemented"));
        }

        public async Task<ServiceResult<Vocabulary>> GetVocabularyByIdAsync(int id)
        {
            // Implement vocabulary detail retrieval
            return await Task.FromResult(ServiceResult<Vocabulary>.Fail("Not implemented"));
        }

        public async Task<ServiceResult<Vocabulary>> CreateVocabularyAsync(CreateVocabularyRequest request)
        {
            // Implement vocabulary creation
            return await Task.FromResult(ServiceResult<Vocabulary>.Fail("Not implemented"));
        }

        public async Task<ServiceResult<Vocabulary>> UpdateVocabularyAsync(int id, UpdateVocabularyRequest request)
        {
            // Implement vocabulary update
            return await Task.FromResult(ServiceResult<Vocabulary>.Fail("Not implemented"));
        }

        public async Task<ServiceResult<bool>> DeleteVocabularyAsync(int id)
        {
            // Implement vocabulary deletion
            return await Task.FromResult(ServiceResult<bool>.Fail("Not implemented"));
        }
    }
}
