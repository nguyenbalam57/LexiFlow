using LexiFlow.Core.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LexiFlow.AdminDashboard.Services
{
    public class ApiClientOptions
    {
        public string BaseUrl { get; set; } = "http://localhost:5117";
        public int TimeoutSeconds { get; set; } = 30;
        public bool UseAuthToken { get; set; } = true;
        public string AuthTokenKey { get; set; } = "LexiFlow.AuthToken";
    }

    public interface IApiClient
    {
        Task<T?> GetAsync<T>(string endpoint, Dictionary<string, string>? queryParams = null);
        Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data);
        Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data);
        Task<bool> DeleteAsync(string endpoint, int id);
        Task<bool> PatchAsync<T>(string endpoint, T data);
        Task<Stream> DownloadFileAsync(string endpoint);
        Task<bool> UploadFileAsync(string endpoint, Stream fileStream, string fileName, string contentType);
        void SetAuthToken(string token);
        void ClearAuthToken();
    }

    public class ApiClient : IApiClient, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiClient> _logger;
        private readonly ApiClientOptions _options;
        private readonly JsonSerializerOptions _jsonOptions;
        private bool _disposed = false;

        public ApiClient(IOptions<ApiClientOptions> options, ILogger<ApiClient> logger)
        {
            _options = options.Value;
            _logger = logger;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_options.BaseUrl),
                Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds)
            };

            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                PropertyNameCaseInsensitive = true
            };

            // Load auth token if available
            if (_options.UseAuthToken)
            {
                var token = LoadAuthToken();
                if (!string.IsNullOrEmpty(token))
                {
                    SetAuthToken(token);
                }
            }
        }

        public async Task<T?> GetAsync<T>(string endpoint, Dictionary<string, string>? queryParams = null)
        {
            try
            {
                var requestUri = BuildRequestUri(endpoint, queryParams);
                _logger.LogDebug("GET request to {RequestUri}", requestUri);

                var response = await _httpClient.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error occurred while calling {Endpoint}", endpoint);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calling {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            try
            {
                _logger.LogDebug("POST request to {Endpoint}", endpoint);

                var response = await _httpClient.PostAsJsonAsync(endpoint, data, _jsonOptions);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error occurred while posting to {Endpoint}", endpoint);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while posting to {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            try
            {
                _logger.LogDebug("PUT request to {Endpoint}", endpoint);

                var response = await _httpClient.PutAsJsonAsync(endpoint, data, _jsonOptions);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error occurred while putting to {Endpoint}", endpoint);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while putting to {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string endpoint, int id)
        {
            try
            {
                var requestUri = $"{endpoint}/{id}";
                _logger.LogDebug("DELETE request to {RequestUri}", requestUri);

                var response = await _httpClient.DeleteAsync(requestUri);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error occurred while deleting {Endpoint}/{Id}", endpoint, id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting {Endpoint}/{Id}", endpoint, id);
                throw;
            }
        }

        public async Task<bool> PatchAsync<T>(string endpoint, T data)
        {
            try
            {
                _logger.LogDebug("PATCH request to {Endpoint}", endpoint);

                var content = new StringContent(
                    JsonSerializer.Serialize(data, _jsonOptions),
                    Encoding.UTF8,
                    "application/json");

                var request = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint)
                {
                    Content = content
                };

                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error occurred while patching {Endpoint}", endpoint);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while patching {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task<Stream> DownloadFileAsync(string endpoint)
        {
            try
            {
                _logger.LogDebug("Downloading file from {Endpoint}", endpoint);

                var response = await _httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStreamAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error occurred while downloading file from {Endpoint}", endpoint);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while downloading file from {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task<bool> UploadFileAsync(string endpoint, Stream fileStream, string fileName, string contentType)
        {
            try
            {
                _logger.LogDebug("Uploading file {FileName} to {Endpoint}", fileName, endpoint);

                using var content = new MultipartFormDataContent();
                using var fileContent = new StreamContent(fileStream);

                fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                content.Add(fileContent, "file", fileName);

                var response = await _httpClient.PostAsync(endpoint, content);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error occurred while uploading file to {Endpoint}", endpoint);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while uploading file to {Endpoint}", endpoint);
                throw;
            }
        }

        public void SetAuthToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Attempted to set empty auth token");
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            SaveAuthToken(token);
            _logger.LogDebug("Auth token set successfully");
        }

        public void ClearAuthToken()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            SaveAuthToken(string.Empty);
            _logger.LogDebug("Auth token cleared");
        }

        private string BuildRequestUri(string endpoint, Dictionary<string, string>? queryParams)
        {
            if (queryParams == null || queryParams.Count == 0)
            {
                return endpoint;
            }

            var queryString = string.Join("&", queryParams.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));
            return $"{endpoint}?{queryString}";
        }

        private string LoadAuthToken()
        {
            try
            {
                // In a real implementation, this would be from secure storage
                // For demo purposes, using a simple approach
                return Environment.GetEnvironmentVariable(_options.AuthTokenKey) ?? string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading auth token");
                return string.Empty;
            }
        }

        private void SaveAuthToken(string token)
        {
            try
            {
                // In a real implementation, this would save to secure storage
                // For demo purposes, using a simple approach
                Environment.SetEnvironmentVariable(_options.AuthTokenKey, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving auth token");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _httpClient.Dispose();
                }

                _disposed = true;
            }
        }
    }
}