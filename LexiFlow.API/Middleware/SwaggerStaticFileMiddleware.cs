using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace LexiFlow.API.Middleware
{
    /// <summary>
    /// Simple middleware ?? serve Swagger static files không compression
    /// Approach ??n gi?n nh?t ?? tránh Content-Length mismatch
    /// </summary>
    public class SwaggerStaticFileMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SwaggerStaticFileMiddleware> _logger;

        public SwaggerStaticFileMiddleware(RequestDelegate next, ILogger<SwaggerStaticFileMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            // KH?C PH?C: Ch? handle Swagger UI static files
            if (!string.IsNullOrEmpty(path) && IsSwaggerStaticFile(path))
            {
                // Force no compression
                context.Request.Headers.Remove("Accept-Encoding");
                context.Request.Headers["Accept-Encoding"] = "identity";
                
                // Set response headers to prevent compression
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                    context.Response.Headers["Pragma"] = "no-cache";
                    context.Response.Headers["Expires"] = "0";
                    context.Response.Headers.Remove("Content-Encoding");
                    
                    return Task.CompletedTask;
                });

                _logger.LogDebug("Serving Swagger static file without compression: {Path}", path);
            }

            await _next(context);
        }

        /// <summary>
        /// Ki?m tra xem có ph?i Swagger static file không
        /// </summary>
        private static bool IsSwaggerStaticFile(string path)
        {
            return (path.StartsWith("/swagger") || path.Equals("/")) &&
                   (path.EndsWith(".js") || 
                    path.EndsWith(".css") || 
                    path.EndsWith(".html") || 
                    path.EndsWith(".json") ||
                    path.EndsWith(".png") || 
                    path.EndsWith(".ico") ||
                    path.Contains("swagger-ui"));
        }
    }

    /// <summary>
    /// Extension method
    /// </summary>
    public static class SwaggerStaticFileMiddlewareExtensions
    {
        public static IApplicationBuilder UseSwaggerStaticFileFix(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SwaggerStaticFileMiddleware>();
        }
    }
}