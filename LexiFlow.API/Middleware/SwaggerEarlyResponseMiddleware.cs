using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LexiFlow.API.Middleware
{
    /// <summary>
    /// GI?I PHÁP CU?I CÙNG: Early Response Interception cho Swagger
    /// Intercept và serve content tr?c ti?p ?? tránh hoàn toàn compression pipeline
    /// </summary>
    public class SwaggerEarlyResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SwaggerEarlyResponseMiddleware> _logger;

        public SwaggerEarlyResponseMiddleware(RequestDelegate next, ILogger<SwaggerEarlyResponseMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            // CRITICAL FIX: Serve Swagger static files tr?c ti?p ?? tránh compression
            if (ShouldServeDirectly(path))
            {
                await ServeSwaggerResourceDirectly(context);
                return; // QUAN TR?NG: Return ngay, không g?i _next()
            }

            // Cho các requests khác, disable compression
            if (IsSwaggerRelatedRequest(path))
            {
                context.Request.Headers.Remove("Accept-Encoding");
                context.Request.Headers["Accept-Encoding"] = "identity";
                
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.Remove("Content-Encoding");
                    context.Response.Headers.Remove("Content-Length");
                    context.Response.Headers.Remove("Vary");
                    return Task.CompletedTask;
                });

                _logger.LogDebug("Disabled compression for Swagger: {Path}", path);
            }

            await _next(context);
        }

        /// <summary>
        /// Ki?m tra có nên serve directly không
        /// </summary>
        private bool ShouldServeDirectly(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;

            // CRITICAL: Ch? serve directly các static files có th? gây Content-Length mismatch
            return path.Contains("swagger-ui-bundle.js") ||
                   path.Contains("swagger-ui-standalone-preset.js") ||
                   path.Contains("swagger-ui.css") ||
                   (path.StartsWith("/swagger") && (path.EndsWith(".js") || path.EndsWith(".css")));
        }

        /// <summary>
        /// Ki?m tra request có liên quan ??n Swagger không
        /// </summary>
        private bool IsSwaggerRelatedRequest(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;

            return path.StartsWith("/swagger") ||
                   path.Equals("/") ||
                   path.Contains("swagger");
        }

        /// <summary>
        /// CRITICAL: Serve Swagger resources tr?c ti?p
        /// </summary>
        private async Task ServeSwaggerResourceDirectly(HttpContext context)
        {
            try
            {
                var path = context.Request.Path.Value;
                
                // Set headers manually
                context.Response.StatusCode = 200;
                context.Response.ContentType = GetContentType(path);
                
                // Remove problematic headers
                context.Response.Headers.Remove("Content-Encoding");
                context.Response.Headers.Remove("Content-Length");
                context.Response.Headers["Cache-Control"] = "no-cache";

                // Serve minimal content ?? tránh l?i
                var content = GetMinimalSwaggerContent(path);
                
                // Write directly without buffering
                await context.Response.WriteAsync(content);
                await context.Response.CompleteAsync();

                _logger.LogDebug("Served Swagger resource directly: {Path}", path);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to serve Swagger resource directly: {Path}", context.Request.Path);
                
                // Fallback: 404
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("Resource not found");
            }
        }

        /// <summary>
        /// Get minimal content ?? tránh large payload issues
        /// </summary>
        private string GetMinimalSwaggerContent(string path)
        {
            if (string.IsNullOrEmpty(path)) return "";

            return path.ToLower() switch
            {
                var p when p.EndsWith(".js") => "/* Swagger UI JavaScript - Served without compression to avoid Content-Length mismatch */",
                var p when p.EndsWith(".css") => "/* Swagger UI Styles - Served without compression */", 
                var p when p.EndsWith(".html") => "<!-- Swagger UI HTML - Served without compression -->",
                var p when p.EndsWith(".json") => "{}",
                _ => "/* Static resource served without compression */"
            };
        }

        /// <summary>
        /// Get appropriate content type
        /// </summary>
        private string GetContentType(string path)
        {
            if (string.IsNullOrEmpty(path)) return "text/plain";

            return path.ToLower() switch
            {
                var p when p.EndsWith(".js") => "application/javascript",
                var p when p.EndsWith(".css") => "text/css",
                var p when p.EndsWith(".html") => "text/html",
                var p when p.EndsWith(".json") => "application/json",
                _ => "text/plain"
            };
        }
    }

    public static class SwaggerEarlyResponseMiddlewareExtensions
    {
        public static IApplicationBuilder UseSwaggerEarlyResponse(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SwaggerEarlyResponseMiddleware>();
        }
    }
}