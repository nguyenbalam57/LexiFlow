using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace LexiFlow.AdminDashboard.Services
{
    /// <summary>
    /// Class for handling API errors
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
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        /// <summary>
        /// Handle HTTP exceptions and return standardized error result
        /// </summary>
        public ApiErrorResult HandleHttpException(Exception ex, string requestUri)
        {
            var result = new ApiErrorResult
            {
                Message = "Could not connect to the server. Please check your internet connection and try again.",
                StatusCode = ex is HttpRequestException httpEx ? httpEx.StatusCode ?? HttpStatusCode.InternalServerError : HttpStatusCode.InternalServerError,
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
                HttpStatusCode.BadGateway => "The server encountered a temporary error. Please try again later.",
                HttpStatusCode.ServiceUnavailable => "The service is currently unavailable. Please try again later.",
                HttpStatusCode.GatewayTimeout => "The server did not respond in time. Please try again later.",
                _ => defaultMessage
            };
        }
    }

    /// <summary>
    /// API error result
    /// </summary>
    public class ApiErrorResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public ValidationError[] ValidationErrors { get; set; }
    }

    /// <summary>
    /// API error
    /// </summary>
    public class ApiError
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public ValidationError[] ValidationErrors { get; set; }
    }

    /// <summary>
    /// Validation error
    /// </summary>
    public class ValidationError
    {
        public string Field { get; set; }
        public string Message { get; set; }
    }
}