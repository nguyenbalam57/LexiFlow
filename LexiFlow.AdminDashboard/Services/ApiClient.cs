using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.IO;

namespace LexiFlow.AdminDashboard.Services
{
    /// <summary>
    /// Enhanced implementation of API client for LexiFlow services
    /// </summary>
    public class ApiClient : IApiClient, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiClient> _logger;
        private readonly ApiSettings _settings;
        private readonly JsonSerializerOptions _jsonOptions;

        public string BaseUrl { get; set; }
        public bool IsConnected { get; private set; }

        public ApiClient(HttpClient httpClient, ILogger<ApiClient> logger, IOptions<ApiSettings> settings)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));

            BaseUrl = _settings.BaseUrl;
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(_settings.TimeoutSeconds);

            // Configure JSON serialization options
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = false
            };

            // Set default headers
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "LexiFlow-AdminDashboard/1.0");

            // Initialize connection status
            _ = Task.Run(CheckConnectionAsync);
        }

        #region Authentication

        /// <summary>
        /// Set authentication token for API requests
        /// </summary>
        /// <param name="token">JWT token</param>
        public void SetAuthToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
                _logger.LogInformation("Authentication token cleared");
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                _logger.LogInformation("Authentication token set");
            }
        }

        /// <summary>
        /// Clear authentication token
        /// </summary>
        public void ClearAuthToken()
        {
            SetAuthToken(null);
        }

        #endregion

        #region GET Methods

        public async Task<T> GetAsync<T>(string endpoint)
        {
            return await GetAsync<T>(endpoint, null);
        }

        public async Task<T> GetAsync<T>(string endpoint, Dictionary<string, string>? queryParams)
        {
            return await ExecuteWithRetryAsync(async () =>
            {
                var url = BuildUrlWithQuery(endpoint, queryParams);
                
                _logger.LogDebug("GET request to: {Url}", url);

                using var response = await _httpClient.GetAsync(url);
                return await ProcessResponseAsync<T>(response);
            });
        }

        /// <summary>
        /// Get with cancellation token support
        /// </summary>
        public async Task<T> GetAsync<T>(string endpoint, Dictionary<string, string>? queryParams, CancellationToken cancellationToken)
        {
            return await ExecuteWithRetryAsync(async () =>
            {
                var url = BuildUrlWithQuery(endpoint, queryParams);
                
                _logger.LogDebug("GET request to: {Url}", url);

                using var response = await _httpClient.GetAsync(url, cancellationToken);
                return await ProcessResponseAsync<T>(response);
            });
        }

        #endregion

        #region POST Methods

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            return await ExecuteWithRetryAsync(async () =>
            {
                _logger.LogDebug("POST request to: {Endpoint}", endpoint);

                var json = JsonSerializer.Serialize(data, _jsonOptions);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var response = await _httpClient.PostAsync(endpoint, content);
                return await ProcessResponseAsync<TResponse>(response);
            });
        }

        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            return await PostAsync<object, T>(endpoint, data);
        }

        public async Task PostAsync(string endpoint, object data)
        {
            await ExecuteWithRetryAsync(async () =>
            {
                _logger.LogDebug("POST request to: {Endpoint}", endpoint);

                var json = data != null ? JsonSerializer.Serialize(data, _jsonOptions) : "";
                using var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var response = await _httpClient.PostAsync(endpoint, content);
                await EnsureSuccessStatusCodeAsync(response);
                return true; // Return something for the generic method
            });
        }

        /// <summary>
        /// POST with cancellation token support
        /// </summary>
        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken)
        {
            return await ExecuteWithRetryAsync(async () =>
            {
                _logger.LogDebug("POST request to: {Endpoint}", endpoint);

                var json = JsonSerializer.Serialize(data, _jsonOptions);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var response = await _httpClient.PostAsync(endpoint, content, cancellationToken);
                return await ProcessResponseAsync<TResponse>(response);
            });
        }

        #endregion

        #region PUT Methods

        public async Task<T> PutAsync<T>(string endpoint, object data)
        {
            return await ExecuteWithRetryAsync(async () =>
            {
                _logger.LogDebug("PUT request to: {Endpoint}", endpoint);

                var json = JsonSerializer.Serialize(data, _jsonOptions);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var response = await _httpClient.PutAsync(endpoint, content);
                return await ProcessResponseAsync<T>(response);
            });
        }

        /// <summary>
        /// PUT with cancellation token support
        /// </summary>
        public async Task<T> PutAsync<T>(string endpoint, object data, CancellationToken cancellationToken)
        {
            return await ExecuteWithRetryAsync(async () =>
            {
                _logger.LogDebug("PUT request to: {Endpoint}", endpoint);

                var json = JsonSerializer.Serialize(data, _jsonOptions);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var response = await _httpClient.PutAsync(endpoint, content, cancellationToken);
                return await ProcessResponseAsync<T>(response);
            });
        }

        #endregion

        #region DELETE Methods

        public async Task DeleteAsync(string endpoint)
        {
            await ExecuteWithRetryAsync(async () =>
            {
                _logger.LogDebug("DELETE request to: {Endpoint}", endpoint);

                using var response = await _httpClient.DeleteAsync(endpoint);
                await EnsureSuccessStatusCodeAsync(response);
                return true; // Return something for the generic method
            });
        }

        /// <summary>
        /// DELETE with cancellation token support
        /// </summary>
        public async Task DeleteAsync(string endpoint, CancellationToken cancellationToken)
        {
            await ExecuteWithRetryAsync(async () =>
            {
                _logger.LogDebug("DELETE request to: {Endpoint}", endpoint);

                using var response = await _httpClient.DeleteAsync(endpoint, cancellationToken);
                await EnsureSuccessStatusCodeAsync(response);
                return true; // Return something for the generic method
            });
        }

        /// <summary>
        /// DELETE with response data
        /// </summary>
        public async Task<T> DeleteAsync<T>(string endpoint)
        {
            return await ExecuteWithRetryAsync(async () =>
            {
                _logger.LogDebug("DELETE request to: {Endpoint}", endpoint);

                using var response = await _httpClient.DeleteAsync(endpoint);
                return await ProcessResponseAsync<T>(response);
            });
        }

        #endregion

        #region PATCH Methods

        /// <summary>
        /// PATCH method for partial updates
        /// </summary>
        public async Task<T> PatchAsync<T>(string endpoint, object data)
        {
            return await ExecuteWithRetryAsync(async () =>
            {
                _logger.LogDebug("PATCH request to: {Endpoint}", endpoint);

                var json = JsonSerializer.Serialize(data, _jsonOptions);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var request = new HttpRequestMessage(HttpMethod.Patch, endpoint)
                {
                    Content = content
                };

                using var response = await _httpClient.SendAsync(request);
                return await ProcessResponseAsync<T>(response);
            });
        }

        #endregion

        #region File Upload/Download

        /// <summary>
        /// Upload file using multipart form data
        /// </summary>
        public async Task<T> UploadFileAsync<T>(string endpoint, string filePath, string fileParameterName = "file", Dictionary<string, string>? additionalData = null)
        {
            return await ExecuteWithRetryAsync(async () =>
            {
                _logger.LogDebug("File upload to: {Endpoint}, File: {FilePath}", endpoint, filePath);

                using var form = new MultipartFormDataContent();

                // Add file content
                var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(filePath));
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
                form.Add(fileContent, fileParameterName, Path.GetFileName(filePath));

                // Add additional data
                if (additionalData != null)
                {
                    foreach (var kvp in additionalData)
                    {
                        form.Add(new StringContent(kvp.Value), kvp.Key);
                    }
                }

                using var response = await _httpClient.PostAsync(endpoint, form);
                return await ProcessResponseAsync<T>(response);
            });
        }

        /// <summary>
        /// Download file
        /// </summary>
        public async Task<byte[]> DownloadFileAsync(string endpoint)
        {
            return await ExecuteWithRetryAsync(async () =>
            {
                _logger.LogDebug("File download from: {Endpoint}", endpoint);

                using var response = await _httpClient.GetAsync(endpoint);
                await EnsureSuccessStatusCodeAsync(response);
                return await response.Content.ReadAsByteArrayAsync();
            });
        }

        #endregion

        #region Health Check

        /// <summary>
        /// Check API health status with fallback options
        /// </summary>
        public async Task<bool> CheckHealthAsync()
        {
            try
            {
                // Try HTTPS first
                using var response = await _httpClient.GetAsync("/health");
                IsConnected = response.IsSuccessStatusCode;
                return IsConnected;
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("SSL") || ex.Message.Contains("certificate"))
            {
                _logger.LogWarning(ex, "SSL/TLS error during health check, attempting HTTP fallback");
                return await CheckHealthWithHttpFallbackAsync();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Health check failed");
                IsConnected = false;
                return false;
            }
        }

        /// <summary>
        /// Fallback health check using HTTP instead of HTTPS
        /// </summary>
        private async Task<bool> CheckHealthWithHttpFallbackAsync()
        {
            try
            {
                // Create temporary HTTP client for fallback
                var httpBaseUrl = BaseUrl.Replace("https://", "http://");
                using var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(httpBaseUrl);
                httpClient.Timeout = TimeSpan.FromSeconds(10);

                using var response = await httpClient.GetAsync("/health");
                IsConnected = response.IsSuccessStatusCode;

                if (IsConnected)
                {
                    _logger.LogInformation("Health check successful using HTTP fallback");
                }

                return IsConnected;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "HTTP fallback health check also failed");
                IsConnected = false;
                return false;
            }
        }

        /// <summary>
        /// Get detailed health information
        /// </summary>
        public async Task<T> GetHealthAsync<T>()
        {
            return await GetAsync<T>("/health");
        }

        #endregion

        #region Private Helper Methods

        private async Task CheckConnectionAsync()
        {
            try
            {
                IsConnected = await CheckHealthAsync();
                _logger.LogInformation("API connection status: {Status}", IsConnected ? "Connected" : "Disconnected");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking initial connection");
                IsConnected = false;
            }
        }

        private string BuildUrlWithQuery(string endpoint, Dictionary<string, string>? queryParams)
        {
            if (queryParams == null || queryParams.Count == 0)
                return endpoint;

            var query = string.Join("&", queryParams.Select(kvp => 
                $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
            
            return $"{endpoint}?{query}";
        }

        private async Task<T> ProcessResponseAsync<T>(HttpResponseMessage response)
        {
            await EnsureSuccessStatusCodeAsync(response);

            var content = await response.Content.ReadAsStringAsync();
            
            if (string.IsNullOrEmpty(content))
            {
                return default(T);
            }

            try
            {
                return JsonSerializer.Deserialize<T>(content, _jsonOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize response content: {Content}", content);
                throw new InvalidOperationException($"Failed to deserialize API response: {ex.Message}", ex);
            }
        }

        private async Task EnsureSuccessStatusCodeAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                IsConnected = true;
                return;
            }

            IsConnected = false;
            var content = await response.Content.ReadAsStringAsync();
            
            _logger.LogError("API request failed. Status: {StatusCode}, Content: {Content}", 
                response.StatusCode, content);

            // Try to parse error response
            try
            {
                var errorResponse = JsonSerializer.Deserialize<ApiErrorResponse>(content, _jsonOptions);
                throw new ApiException(response.StatusCode, errorResponse?.Message ?? "API request failed", errorResponse);
            }
            catch (JsonException)
            {
                // If we can't parse the error response, throw a generic exception
                throw new ApiException(response.StatusCode, $"API request failed: {response.StatusCode}", content);
            }
        }

        private async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation)
        {
            if (!_settings.EnableRetry)
            {
                return await operation();
            }

            var retryCount = 0;
            var baseDelay = TimeSpan.FromSeconds(1);

            while (retryCount <= _settings.MaxRetryAttempts)
            {
                try
                {
                    return await operation();
                }
                catch (Exception ex) when (ShouldRetry(ex, retryCount))
                {
                    retryCount++;
                    var delay = TimeSpan.FromMilliseconds(baseDelay.TotalMilliseconds * Math.Pow(2, retryCount - 1));
                    
                    _logger.LogWarning(ex, "API operation failed, retrying in {Delay}ms (attempt {Attempt}/{MaxAttempts})", 
                        delay.TotalMilliseconds, retryCount, _settings.MaxRetryAttempts);

                    await Task.Delay(delay);
                }
            }

            // Final attempt without retry handling
            return await operation();
        }

        private static bool ShouldRetry(Exception ex, int retryCount)
        {
            if (retryCount >= 3) // Max 3 retries
                return false;

            return ex switch
            {
                HttpRequestException => true,
                TaskCanceledException => true,
                ApiException apiEx when apiEx.StatusCode >= System.Net.HttpStatusCode.InternalServerError => true,
                _ => false
            };
        }

        #endregion

        #region IDisposable

        private bool _disposed = false;

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
                    _httpClient?.Dispose();
                }
                _disposed = true;
            }
        }

        #endregion
    }

    #region Exception Classes

    /// <summary>
    /// Exception thrown when API requests fail
    /// </summary>
    public class ApiException : Exception
    {
        public System.Net.HttpStatusCode StatusCode { get; }
        public ApiErrorResponse? ErrorResponse { get; }
        public string? ResponseContent { get; }

        public ApiException(System.Net.HttpStatusCode statusCode, string message, ApiErrorResponse? errorResponse = null)
            : base(message)
        {
            StatusCode = statusCode;
            ErrorResponse = errorResponse;
        }

        public ApiException(System.Net.HttpStatusCode statusCode, string message, string? responseContent)
            : base(message)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
        }

        public ApiException(System.Net.HttpStatusCode statusCode, string message, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }

    /// <summary>
    /// API error response model
    /// </summary>
    public class ApiErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public string? Detail { get; set; }
        public Dictionary<string, string[]>? Errors { get; set; }
        public string? TraceId { get; set; }
    }

    #endregion
}