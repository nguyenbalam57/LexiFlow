using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using LexiFlow.AdminDashboard.Models;
using Microsoft.Extensions.Logging;

namespace LexiFlow.AdminDashboard.Services
{
    /// <summary>
    /// Handles HTTP errors from API calls and provides standardized error handling
    /// </summary>
    public class ApiErrorHandler
    {
        private readonly ILogger _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiErrorHandler(ILogger logger)
        {
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        /// <summary>
        /// Handle HTTP exceptions and extract error information
        /// </summary>
        public async Task<ApiErrorResult> HandleHttpExceptionAsync(HttpRequestException ex, string requestUri)
        {
            var result = new ApiErrorResult
            {
                Message = $"Error communicating with the server: {ex.Message}",
                StatusCode = ex.StatusCode ?? HttpStatusCode.InternalServerError,
                ErrorCode = "ApiCommunicationError"
            };

            _logger.LogError(ex, "HTTP error occurred while calling {RequestUri}. Status code: {StatusCode}",
                requestUri, result.StatusCode);

            // Add error details based on status code
            result.Message = GetErrorMessageByStatusCode(result.StatusCode, result.Message);

            return result;
        }

        /// <summary>
        /// Extract API error details from HTTP response message
        /// </summary>
        public async Task<ApiErrorResult> ExtractApiErrorAsync(HttpResponseMessage response, string requestUri)
        {
            var result = new ApiErrorResult
            {
                Message = $"Error from API: {response.ReasonPhrase}",
                StatusCode = response.StatusCode,
                ErrorCode = "ApiError"
            };

            try
            {
                // Try to extract detailed error information from the response
                if (response.Content != null)
                {
                    var apiError = await response.Content.ReadFromJsonAsync<ApiError>(_jsonOptions);

                    if (apiError != null)
                    {
                        result.ErrorCode = apiError.Code;
                        result.Message = apiError.Message;
                        result.Details = apiError.Details;
                        result.ValidationErrors = apiError.ValidationErrors;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to parse API error response from {RequestUri}", requestUri);
            }

            _logger.LogError("API error occurred while calling {RequestUri}. Status code: {StatusCode}, Error code: {ErrorCode}, Message: {Message}",
                requestUri, result.StatusCode, result.ErrorCode, result.Message);

            return result;
        }

        /// <summary>
        /// Get a user-friendly error message based on HTTP status code
        /// </summary>
        private string GetErrorMessageByStatusCode(HttpStatusCode statusCode, string defaultMessage)
        {
            return statusCode switch
            {
                HttpStatusCode.Unauthorized => "Your session has expired or you don't have permission to access this resource. Please log in again.",
                HttpStatusCode.Forbidden => "You don't have permission to access this resource. Please contact your administrator.",
                HttpStatusCode.NotFound => "The requested resource was not found on the server.",
                HttpStatusCode.BadRequest => "The server could not process the request. Please check your input and try again.",
                HttpStatusCode.InternalServerError => "An error occurred on the server. Please try again later or contact support if the problem persists.",
                HttpStatusCode.ServiceUnavailable => "The service is currently unavailable. Please try again later.",
                HttpStatusCode.GatewayTimeout => "The server took too long to respond. Please try again later.",
                _ => defaultMessage
            };
        }

        /// <summary>
        /// Create a network error result
        /// </summary>
        public ApiErrorResult CreateNetworkErrorResult(string message, Exception? exception = null)
        {
            var errorMessage = $"Network error: {message}";

            if (exception != null)
            {
                _logger.LogError(exception, errorMessage);
            }
            else
            {
                _logger.LogError(errorMessage);
            }

            return new ApiErrorResult
            {
                Message = errorMessage,
                StatusCode = HttpStatusCode.ServiceUnavailable,
                ErrorCode = "NetworkError"
            };
        }
    }

    /// <summary>
    /// Represents an API error result
    /// </summary>
    public class ApiErrorResult
    {
        public string Message { get; set; } = string.Empty;
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;
        public string ErrorCode { get; set; } = string.Empty;
        public string? Details { get; set; }
        public List<ValidationError>? ValidationErrors { get; set; }

        public bool IsAuthenticationError =>
            StatusCode == HttpStatusCode.Unauthorized ||
            StatusCode == HttpStatusCode.Forbidden;

        public bool IsNetworkError =>
            ErrorCode == "NetworkError" ||
            StatusCode == HttpStatusCode.ServiceUnavailable ||
            StatusCode == HttpStatusCode.GatewayTimeout;

        public bool IsValidationError =>
            ValidationErrors != null && ValidationErrors.Count > 0;

        public string GetFormattedValidationErrors()
        {
            if (ValidationErrors == null || ValidationErrors.Count == 0)
            {
                return string.Empty;
            }

            return string.Join("\n", ValidationErrors.Select(e => $"• {e.Field}: {e.Message}"));
        }
    }
}