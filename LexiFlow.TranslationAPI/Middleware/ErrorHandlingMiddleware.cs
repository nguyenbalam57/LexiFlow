using System.Net;
using System.Security;
using System.Text.Json;

namespace LexiFlow.TranslationAPI.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, message, details) = GetErrorResponse(exception);
            context.Response.StatusCode = (int)statusCode;

            var response = new ErrorResponse
            {
                Message = message,
                Details = details,
                StatusCode = (int)statusCode,
                Timestamp = DateTime.UtcNow,
                Path = context.Request.Path,
                Method = context.Request.Method,
                TraceId = context.TraceIdentifier
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var jsonResponse = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(jsonResponse);
        }

        private static (HttpStatusCode statusCode, string message, string? details) GetErrorResponse(Exception exception)
        {
            // Sắp xếp lại thứ tự từ specific đến general để tránh unreachable pattern
            return exception switch
            {
                // Specific exceptions first
                ArgumentNullException argNullEx => (
                    HttpStatusCode.BadRequest,
                    "Missing required parameter",
                    $"Parameter '{argNullEx.ParamName}' cannot be null"
                ),
                ArgumentException argEx => (
                    HttpStatusCode.BadRequest,
                    "Invalid request parameters",
                    argEx.Message
                ),
                UnauthorizedAccessException => (
                    HttpStatusCode.Unauthorized,
                    "Unauthorized access",
                    "Access to the requested resource is denied"
                ),
                TaskCanceledException tcEx when tcEx.InnerException is TimeoutException => (
                    HttpStatusCode.RequestTimeout,
                    "Request timeout",
                    "The request took too long to process"
                ),
                TaskCanceledException => (
                    HttpStatusCode.RequestTimeout,
                    "Request cancelled or timed out",
                    "The operation was cancelled or exceeded the timeout period"
                ),
                TimeoutException => (
                    HttpStatusCode.RequestTimeout,
                    "Request timeout",
                    "The request took too long to process"
                ),
                HttpRequestException httpEx => (
                    HttpStatusCode.ServiceUnavailable,
                    "External service unavailable",
                    httpEx.Message
                ),
                NotImplementedException => (
                    HttpStatusCode.NotImplemented,
                    "Feature not implemented",
                    "The requested feature is not yet implemented"
                ),
                InvalidOperationException invOpEx => (
                    HttpStatusCode.Conflict,
                    "Invalid operation",
                    invOpEx.Message
                ),
                FileNotFoundException fileNotFoundEx => (
                    HttpStatusCode.NotFound,
                    "Resource not found",
                    fileNotFoundEx.Message
                ),
                DirectoryNotFoundException dirNotFoundEx => (
                    HttpStatusCode.NotFound,
                    "Directory not found",
                    dirNotFoundEx.Message
                ),
                SecurityException secEx => (
                    HttpStatusCode.Forbidden,
                    "Security error",
                    "Access denied due to security restrictions"
                ),
                NotSupportedException notSupportedEx => (
                    HttpStatusCode.NotImplemented,
                    "Operation not supported",
                    notSupportedEx.Message
                ),
                FormatException formatEx => (
                    HttpStatusCode.BadRequest,
                    "Invalid format",
                    formatEx.Message
                ),
                OverflowException overflowEx => (
                    HttpStatusCode.BadRequest,
                    "Value overflow",
                    overflowEx.Message
                ),
                // General exception last
                _ => (
                    HttpStatusCode.InternalServerError,
                    "An internal server error occurred",
                    "Please try again later or contact support if the problem persists"
                )
            };
        }
    }

    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; }
        public string Path { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string TraceId { get; set; } = string.Empty;
        public string? StackTrace { get; set; }
        public Dictionary<string, object>? AdditionalData { get; set; }
    }

    // Extension methods cho ErrorResponse
    public static class ErrorResponseExtensions
    {
        public static ErrorResponse WithStackTrace(this ErrorResponse response, Exception exception, bool includeStackTrace = false)
        {
            if (includeStackTrace && exception.StackTrace != null)
            {
                response.StackTrace = exception.StackTrace;
            }
            return response;
        }

        public static ErrorResponse WithAdditionalData(this ErrorResponse response, string key, object value)
        {
            response.AdditionalData ??= new Dictionary<string, object>();
            response.AdditionalData[key] = value;
            return response;
        }
    }

    // Custom exceptions for the application
    public class TranslationServiceException : Exception
    {
        public TranslationServiceException(string message) : base(message) { }
        public TranslationServiceException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class ModelLoadException : TranslationServiceException
    {
        public ModelLoadException(string message) : base(message) { }
        public ModelLoadException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class UnsupportedLanguageException : TranslationServiceException
    {
        public string LanguageCode { get; }

        public UnsupportedLanguageException(string languageCode)
            : base($"Language '{languageCode}' is not supported")
        {
            LanguageCode = languageCode;
        }
    }
}
