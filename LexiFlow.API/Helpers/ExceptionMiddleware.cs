using LexiFlow.API.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace LexiFlow.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred during request processing");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ApiResponse<object>
            {
                Success = false,
                Message = "An error occurred while processing your request."
            };

            switch (exception)
            {
                case DbUpdateConcurrencyException:
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    response.Message = "The data you attempted to modify has been changed by another user.";
                    response.Errors = new List<string> { "Please refresh the data and try again." };
                    break;

                case DbUpdateException dbUpdateException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = "An error occurred while saving changes to the database.";

                    // Extract useful information from the DB exception
                    var innerException = dbUpdateException.InnerException?.Message;
                    if (innerException != null)
                    {
                        if (innerException.Contains("UNIQUE") || innerException.Contains("DUPLICATE") || innerException.Contains("IX_"))
                        {
                            response.Message = "A record with the same key already exists.";
                        }
                        else if (innerException.Contains("DELETE") && innerException.Contains("REFERENCE"))
                        {
                            response.Message = "Cannot delete this record because it is referenced by other records.";
                        }
                    }

                    response.Errors = new List<string> { innerException ?? dbUpdateException.Message };
                    break;

                case KeyNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = "The requested resource was not found.";
                    response.Errors = new List<string> { exception.Message };
                    break;

                case UnauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Message = "You are not authorized to access this resource.";
                    response.Errors = new List<string> { exception.Message };
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    if (_env.IsDevelopment())
                    {
                        response.Errors = new List<string>
                        {
                            exception.Message,
                            exception.StackTrace ?? "No stack trace available"
                        };
                    }
                    else
                    {
                        response.Errors = new List<string> { "An unexpected error occurred." };
                    }
                    break;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }

    // Extension method to add the middleware to the HTTP pipeline
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}