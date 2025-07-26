using LexiFlow.API.Models.Responses;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LexiFlow.API.Middleware
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred");

            var response = new ApiResponse
            {
                Success = false,
                Message = "An unexpected error occurred. Please try again later."
            };

            var statusCode = StatusCodes.Status500InternalServerError;

            // Customize status code and message based on exception type
            if (exception is UnauthorizedAccessException)
            {
                statusCode = StatusCodes.Status401Unauthorized;
                response.Message = "Unauthorized access";
            }
            else if (exception is DbUpdateConcurrencyException)
            {
                statusCode = StatusCodes.Status409Conflict;
                response.Message = "The resource has been modified by another user. Please refresh and try again.";
            }
            else if (exception is DbUpdateException)
            {
                statusCode = StatusCodes.Status400BadRequest;
                response.Message = "Database update error. Please check your input.";
            }
            else if (exception is KeyNotFoundException)
            {
                statusCode = StatusCodes.Status404NotFound;
                response.Message = "The requested resource was not found.";
            }

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }
}