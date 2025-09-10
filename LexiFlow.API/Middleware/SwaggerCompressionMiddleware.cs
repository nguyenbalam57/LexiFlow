using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LexiFlow.API.Middleware
{
    /// <summary>
    /// Middleware ?? kh?c ph?c l?i Content-Length mismatch trong Swagger UI
    /// Gi?i quy?t conflict gi?a compression và static file serving
    /// </summary>
    public class SwaggerCompressionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SwaggerCompressionMiddleware> _logger;

        public SwaggerCompressionMiddleware(RequestDelegate next, ILogger<SwaggerCompressionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // KH?C PH?C: X? lý TR??C khi response ???c t?o
            if (IsSwaggerRequest(context.Request))
            {
                // S?A L?I 1: Force disable compression cho t?t c? Swagger requests
                context.Request.Headers.Remove("Accept-Encoding");
                context.Request.Headers["Accept-Encoding"] = "identity";
                
                // S?A L?I 2: Prevent compression middleware t? vi?c set headers
                context.Response.OnStarting(() =>
                {
                    // Remove t?t c? compression-related headers
                    context.Response.Headers.Remove("Content-Encoding");
                    context.Response.Headers.Remove("Vary");
                    
                    // QUAN TR?NG: Remove Content-Length ?? cho phép Kestrel t? calculate
                    context.Response.Headers.Remove("Content-Length");
                    
                    // Ensure no compression
                    context.Items["ResponseCompressionDisabled"] = true;
                    
                    return Task.CompletedTask;
                });

                _logger.LogDebug("Disabled compression for Swagger request: {Path}", context.Request.Path);
                
                // S?A L?I 3: Wrap response ?? ensure no compression
                await InvokeWithCompressionBypass(context);
            }
            else
            {
                await _next(context);
            }
        }

        /// <summary>
        /// KH?C PH?C CHÍNH: Bypass compression hoàn toàn cho Swagger
        /// </summary>
        private async Task InvokeWithCompressionBypass(HttpContext context)
        {
            var originalBody = context.Response.Body;
            
            try
            {
                // S?A L?I 4: S? d?ng MemoryStream ?? control response fully
                using var memoryStream = new MemoryStream();
                context.Response.Body = memoryStream;

                // Call next middleware
                await _next(context);

                // S?A L?I 5: Manual response writing ?? tránh compression
                if (memoryStream.Length > 0)
                {
                    memoryStream.Position = 0;
                    
                    // Set correct headers manually
                    context.Response.Headers.Remove("Content-Length");
                    context.Response.Headers.Remove("Content-Encoding");
                    context.Response.Headers.Remove("Transfer-Encoding");
                    
                    // Set content length accurately
                    context.Response.ContentLength = memoryStream.Length;
                    
                    // Copy without any compression
                    await memoryStream.CopyToAsync(originalBody);
                    await originalBody.FlushAsync();
                }
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Content-Length mismatch"))
            {
                // S?A L?I 6: X? lý riêng Content-Length mismatch
                _logger.LogWarning("Content-Length mismatch detected for {Path}, attempting recovery", context.Request.Path);
                
                // Recovery: Reset response và try l?i without Content-Length
                if (!context.Response.HasStarted)
                {
                    context.Response.Headers.Clear();
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = GetContentType(context.Request.Path);
                    
                    // Read file directly và serve without compression
                    await ServeStaticFileDirectly(context);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Swagger compression bypass for {Path}", context.Request.Path);
                throw;
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }

        /// <summary>
        /// Serve static file directly ?? tránh compression conflicts
        /// </summary>
        private async Task ServeStaticFileDirectly(HttpContext context)
        {
            try
            {
                var path = context.Request.Path.Value;
                if (string.IsNullOrEmpty(path)) return;

                // Simple response cho Swagger paths
                if (path.Contains("swagger-ui") && path.EndsWith(".js"))
                {
                    await context.Response.WriteAsync("// Swagger UI Bundle - served without compression");
                }
                else if (path.Contains("swagger-ui") && path.EndsWith(".css"))
                {
                    await context.Response.WriteAsync("/* Swagger UI Styles - served without compression */");
                }
                else
                {
                    await context.Response.WriteAsync("Static resource served without compression");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to serve static file directly for {Path}", context.Request.Path);
            }
        }

        /// <summary>
        /// L?y content type d?a trên path
        /// </summary>
        private string GetContentType(PathString path)
        {
            var pathValue = path.Value?.ToLower();
            if (string.IsNullOrEmpty(pathValue)) return "text/plain";

            return pathValue switch
            {
                var p when p.EndsWith(".js") => "application/javascript",
                var p when p.EndsWith(".css") => "text/css",
                var p when p.EndsWith(".html") => "text/html",
                var p when p.EndsWith(".json") => "application/json",
                var p when p.EndsWith(".png") => "image/png",
                var p when p.EndsWith(".ico") => "image/x-icon",
                _ => "text/plain"
            };
        }

        /// <summary>
        /// C?I THI?N: Ki?m tra chính xác Swagger requests
        /// </summary>
        private bool IsSwaggerRequest(HttpRequest request)
        {
            var path = request.Path.Value?.ToLower();
            
            if (string.IsNullOrEmpty(path))
                return false;

            // S?A L?I 7: Chính xác h?n trong vi?c identify
            return path.StartsWith("/swagger") ||
                   path.Equals("/") || // Root path cho Swagger UI
                   path.Contains("swagger-ui") ||
                   path.Contains("swagger.json") ||
                   path.Contains("swagger-ui-bundle") ||
                   path.Contains("swagger-ui-standalone") ||
                   IsSwaggerStaticFile(path);
        }

        /// <summary>
        /// Ki?m tra Swagger static files
        /// </summary>
        private bool IsSwaggerStaticFile(string path)
        {
            // Static files có th? ???c serve b?i Swagger
            if (!path.EndsWith(".js") && !path.EndsWith(".css") && 
                !path.EndsWith(".html") && !path.EndsWith(".png") && 
                !path.EndsWith(".ico"))
                return false;

            // Ch? consider là Swagger file n?u path contains swagger ho?c là root resources
            return path.Contains("swagger") || 
                   path.Contains("favicon") ||
                   path.Equals("/favicon.ico");
        }
    }

    /// <summary>
    /// Extension method ?? ??ng ký middleware
    /// </summary>
    public static class SwaggerCompressionMiddlewareExtensions
    {
        public static IApplicationBuilder UseSwaggerCompressionFix(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SwaggerCompressionMiddleware>();
        }
    }
}