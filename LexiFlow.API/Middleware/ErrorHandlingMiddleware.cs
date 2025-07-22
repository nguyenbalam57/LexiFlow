using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using LexiFlow.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LexiFlow.API.Middleware
{
    /// <summary>
    /// Middleware for handling exceptions and returning standardized error responses
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Invokes the middleware
        /// </summary>
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

        /// <summary>
        /// Handles an exception and returns a standardized error response
        /// </summary>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Log the exception
            _logger.LogError(exception, "An unhandled exception occurred");

            // Set response content type
            context.Response.ContentType = "application/json";

            // Set response status code and create response object
            var response = new ApiResponse { Success = false };

            switch (exception)
            {
                case UnauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Message = "Unauthorized access";
                    break;

                case ArgumentException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = exception.Message;
                    break;

                case InvalidOperationException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = exception.Message;
                    break;

                case KeyNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = exception.Message;
                    break;

                case DbConcurrencyException:
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    response.Message = "The record you attempted to edit was modified by another user. Please refresh and try again.";
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = "An error occurred while processing your request";
                    break;
            }

            // Serialize and write the response
            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }

    /// <summary>
    /// Exception for database concurrency errors
    /// </summary>
    public class DbConcurrencyException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DbConcurrencyException() : base("A concurrency error occurred") { }

        /// <summary>
        /// Constructor
        /// </summary>
        public DbConcurrencyException(string message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        public DbConcurrencyException(string message, Exception innerException) : base(message, innerException) { }
    }
}