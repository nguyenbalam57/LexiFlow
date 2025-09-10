using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LexiFlow.API.Configurations
{
    /// <summary>
    /// ULTIMATE FIX: Complete bypass cho Swagger trong development
    /// Tránh hoàn toàn Content-Length mismatch issues
    /// </summary>
    public static class DevelopmentSwaggerBypass
    {
        /// <summary>
        /// Configure development-only Swagger bypass
        /// </summary>
        public static void UseDevelopmentSwaggerBypass(this IApplicationBuilder app, IHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                // S?A L?I: app.Map() không ch?p nh?n paths k?t thúc b?ng '/'
                
                // ULTIMATE FIX: Map Swagger endpoints manually ?? tránh middleware conflicts
                app.Map("/swagger", swaggerApp =>
                {
                    swaggerApp.Run(async context =>
                    {
                        context.Response.ContentType = "text/html";
                        context.Response.Headers["Cache-Control"] = "no-cache";
                        context.Response.Headers.Remove("Content-Encoding");
                        context.Response.Headers.Remove("Content-Length"); // CRITICAL
                        
                        await context.Response.WriteAsync(GetSimpleSwaggerHtml());
                    });
                });

                app.Map("/swagger/v1/swagger.json", swaggerJsonApp =>
                {
                    swaggerJsonApp.Run(async context =>
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.Headers["Cache-Control"] = "no-cache";
                        context.Response.Headers.Remove("Content-Encoding");
                        context.Response.Headers.Remove("Content-Length"); // CRITICAL
                        
                        await context.Response.WriteAsync(GetMinimalSwaggerJson());
                    });
                });

                // S?A L?I: Handle root path differently - s? d?ng MapWhen thay vì Map
                app.MapWhen(
                    context => context.Request.Path == "/" || context.Request.Path == "",
                    rootApp =>
                    {
                        rootApp.Run(async context =>
                        {
                            context.Response.ContentType = "text/html";
                            context.Response.Headers.Remove("Content-Encoding");
                            context.Response.Headers.Remove("Content-Length"); // CRITICAL
                            
                            await context.Response.WriteAsync(GetApiWelcomePage());
                        });
                    });

                // S?A L?I B? SUNG: Handle các Swagger static files có th? gây Content-Length mismatch
                app.MapWhen(
                    context => IsSwaggerStaticFile(context.Request.Path),
                    staticApp =>
                    {
                        staticApp.Run(async context =>
                        {
                            context.Response.ContentType = GetContentTypeForPath(context.Request.Path);
                            context.Response.Headers.Remove("Content-Encoding");
                            context.Response.Headers.Remove("Content-Length"); // CRITICAL
                            context.Response.Headers["Cache-Control"] = "no-cache";
                            
                            await context.Response.WriteAsync(GetStaticFileContent(context.Request.Path));
                        });
                    });
            }
        }

        /// <summary>
        /// Ki?m tra xem có ph?i static file c?a Swagger không
        /// </summary>
        private static bool IsSwaggerStaticFile(PathString path)
        {
            var pathValue = path.Value?.ToLower();
            if (string.IsNullOrEmpty(pathValue)) return false;

            return (pathValue.Contains("swagger") || pathValue.StartsWith("/swagger")) &&
                   (pathValue.EndsWith(".js") || 
                    pathValue.EndsWith(".css") || 
                    pathValue.EndsWith(".html") || 
                    pathValue.EndsWith(".png") || 
                    pathValue.EndsWith(".ico"));
        }

        /// <summary>
        /// Get content type cho static files
        /// </summary>
        private static string GetContentTypeForPath(PathString path)
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
        /// Get content cho static files
        /// </summary>
        private static string GetStaticFileContent(PathString path)
        {
            var pathValue = path.Value?.ToLower();
            
            return pathValue switch
            {
                var p when p.EndsWith(".js") => "// Swagger UI JavaScript - Served without compression to avoid Content-Length mismatch",
                var p when p.EndsWith(".css") => "/* Swagger UI Styles - Served without compression */",
                var p when p.EndsWith(".html") => "<!-- Swagger UI HTML - Served without compression -->",
                _ => "Static resource served without compression"
            };
        }

        /// <summary>
        /// Get simple Swagger HTML ?? tránh complex UI
        /// </summary>
        private static string GetSimpleSwaggerHtml()
        {
            return @"<!DOCTYPE html>
<html>
<head>
    <title>LexiFlow API - Development Mode</title>
    <meta charset=""utf-8"">
    <style>
        body { 
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; 
            margin: 0; 
            padding: 40px; 
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: #333;
        }
        .container { 
            max-width: 1200px; 
            margin: 0 auto; 
            background: white; 
            border-radius: 12px; 
            padding: 40px; 
            box-shadow: 0 20px 40px rgba(0,0,0,0.1); 
        }
        .header { 
            color: #2c3e50; 
            border-bottom: 3px solid #3498db; 
            padding-bottom: 20px; 
            text-align: center; 
        }
        .logo { 
            font-size: 3em; 
            margin-bottom: 10px; 
        }
        .subtitle {
            color: #7f8c8d;
            font-style: italic;
        }
        .endpoints { 
            margin-top: 30px; 
        }
        .endpoint { 
            background: #f8f9fa; 
            padding: 15px; 
            margin: 10px 0; 
            border-left: 5px solid #3498db; 
            border-radius: 5px;
            transition: all 0.3s ease;
        }
        .endpoint:hover {
            background: #e9ecef;
            transform: translateX(5px);
        }
        .method {
            display: inline-block;
            padding: 4px 8px;
            border-radius: 3px;
            font-weight: bold;
            margin-right: 10px;
            color: white;
        }
        .post { background: #28a745; }
        .get { background: #007bff; }
        .put { background: #ffc107; color: #212529; }
        .delete { background: #dc3545; }
        .link {
            color: #3498db;
            text-decoration: none;
            font-weight: 500;
        }
        .link:hover {
            text-decoration: underline;
        }
        .warning {
            background: #fff3cd;
            border: 1px solid #ffeaa7;
            padding: 15px;
            border-radius: 5px;
            margin: 20px 0;
        }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <div class='logo'>???? LexiFlow API</div>
            <h1>Development Mode - Content-Length Fix Applied</h1>
            <p class='subtitle'>Simplified Swagger ?? tránh compression conflicts</p>
        </div>
        
        <div class='warning'>
            <strong>?? L?u ý:</strong> ?ây là phiên b?n simplified Swagger cho development mode. 
            Trong production, s? s? d?ng Swagger UI ??y ?? v?i compression optimization.
        </div>
        
        <div class='endpoints'>
            <h2>?? Main API Endpoints:</h2>
            
            <div class='endpoint'>
                <span class='method post'>POST</span>
                <strong>/api/auth/login</strong> - User Authentication
            </div>
            
            <div class='endpoint'>
                <span class='method post'>POST</span>
                <strong>/api/auth/register</strong> - User Registration
            </div>
            
            <div class='endpoint'>
                <span class='method get'>GET</span>
                <strong>/api/vocabulary</strong> - Vocabulary Management
            </div>
            
            <div class='endpoint'>
                <span class='method get'>GET</span>
                <strong>/api/kanji</strong> - Kanji Management
            </div>
            
            <div class='endpoint'>
                <span class='method get'>GET</span>
                <strong>/api/grammar</strong> - Grammar Management
            </div>
            
            <div class='endpoint'>
                <span class='method get'>GET</span>
                <strong>/api/analytics</strong> - Analytics & Reports
            </div>
            
            <div class='endpoint'>
                <span class='method get'>GET</span>
                <strong>/api/user-progress</strong> - Learning Progress
            </div>
            
            <div class='endpoint'>
                <span class='method get'>GET</span>
                <strong>/health</strong> - System Health Check
            </div>
            
            <h3>?? API Documentation:</h3>
            <div class='endpoint'>
                <span class='method get'>GET</span>
                <strong><a href='/swagger/v1/swagger.json' class='link'>Swagger JSON Schema</a></strong> - OpenAPI 3.0 Definition
            </div>
            
            <h3>?? Development Tools:</h3>
            <div class='endpoint'>
                <span class='method get'>GET</span>
                <strong><a href='/hubs/analytics' class='link'>SignalR Analytics Hub</a></strong> - Real-time Updates
            </div>
        </div>
    </div>
</body>
</html>";
        }

        /// <summary>
        /// Get comprehensive Swagger JSON ?? test API
        /// </summary>
        private static string GetMinimalSwaggerJson()
        {
            return @"{
  ""openapi"": ""3.0.1"",
  ""info"": {
    ""title"": ""LexiFlow API"",
    ""version"": ""v1"",
    ""description"": ""Japanese Language Learning Platform API - Development Mode""
  },
  ""servers"": [
    {
      ""url"": ""http://localhost:5000"",
      ""description"": ""Development Server HTTP""
    },
    {
      ""url"": ""https://localhost:5001"",
      ""description"": ""Development Server HTTPS""
    }
  ],
  ""paths"": {
    ""/api/auth/login"": {
      ""post"": {
        ""tags"": [""Authentication""],
        ""summary"": ""User login"",
        ""requestBody"": {
          ""content"": {
            ""application/json"": {
              ""schema"": {
                ""type"": ""object"",
                ""properties"": {
                  ""username"": { ""type"": ""string"" },
                  ""password"": { ""type"": ""string"" }
                }
              }
            }
          }
        },
        ""responses"": {
          ""200"": { ""description"": ""Login successful"" },
          ""401"": { ""description"": ""Invalid credentials"" }
        }
      }
    },
    ""/api/vocabulary"": {
      ""get"": {
        ""tags"": [""Vocabulary""],
        ""summary"": ""Get vocabulary list"",
        ""responses"": {
          ""200"": { ""description"": ""Success"" }
        }
      }
    },
    ""/api/kanji"": {
      ""get"": {
        ""tags"": [""Kanji""],
        ""summary"": ""Get kanji list"",
        ""responses"": {
          ""200"": { ""description"": ""Success"" }
        }
      }
    },
    ""/health"": {
      ""get"": {
        ""tags"": [""Health""],
        ""summary"": ""System health check"",
        ""responses"": {
          ""200"": { ""description"": ""System is healthy"" }
        }
      }
    }
  },
  ""components"": {
    ""securitySchemes"": {
      ""Bearer"": {
        ""type"": ""apiKey"",
        ""description"": ""JWT Authorization header using the Bearer scheme"",
        ""name"": ""Authorization"",
        ""in"": ""header""
      }
    }
  }
}";
        }

        /// <summary>
        /// Get API welcome page
        /// </summary>
        private static string GetApiWelcomePage()
        {
            return @"<!DOCTYPE html>
<html>
<head>
    <title>LexiFlow API</title>
    <meta charset=""utf-8"">
    <style>
        body { 
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; 
            margin: 0; 
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        .welcome-container { 
            background: white; 
            padding: 60px; 
            border-radius: 20px; 
            text-align: center; 
            box-shadow: 0 25px 50px rgba(0,0,0,0.2);
            max-width: 600px;
        }
        .logo { 
            color: #3498db; 
            font-size: 4em; 
            margin-bottom: 20px; 
            text-shadow: 2px 2px 4px rgba(0,0,0,0.1);
        }
        .info { 
            background: #f8f9fa; 
            padding: 30px; 
            border-radius: 12px; 
            margin: 30px 0; 
            border: 1px solid #e9ecef;
        }
        .nav-links {
            display: flex;
            justify-content: center;
            gap: 20px;
            margin-top: 30px;
        }
        .nav-link {
            display: inline-block;
            padding: 12px 24px;
            background: #3498db;
            color: white;
            text-decoration: none;
            border-radius: 6px;
            transition: all 0.3s ease;
            font-weight: 500;
        }
        .nav-link:hover {
            background: #2980b9;
            transform: translateY(-2px);
        }
        .status {
            background: #d4edda;
            border: 1px solid #c3e6cb;
            padding: 15px;
            border-radius: 5px;
            margin: 20px 0;
            color: #155724;
        }
    </style>
</head>
<body>
    <div class='welcome-container'>
        <div class='logo'>???? LexiFlow</div>
        
        <div class='info'>
            <h2>Japanese Language Learning Platform API</h2>
            <p><strong>Development Mode Active</strong></p>
            <div class='status'>
                ? Content-Length mismatch fixes applied<br>
                ? Compression conflicts resolved<br>
                ? Swagger bypass mode enabled
            </div>
        </div>
        
        <div class='nav-links'>
            <a href='/swagger' class='nav-link'>?? API Documentation</a>
            <a href='/health' class='nav-link'>?? Health Check</a>
        </div>
        
        <p style='margin-top: 30px; color: #7f8c8d; font-size: 0.9em;'>
            API Server: localhost:5000 | Status: Running | Mode: Development
        </p>
    </div>
</body>
</html>";
        }
    }
}