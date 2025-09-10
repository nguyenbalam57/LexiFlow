using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace LexiFlow.API.Configurations
{
    /// <summary>
    /// GI?I PH�P CU?I C�NG: Minimal Swagger configuration ?? tr�nh Content-Length issues
    /// </summary>
    public static class MinimalSwaggerConfiguration
    {
        /// <summary>
        /// Configure Swagger v?i settings t?i thi?u ?? tr�nh conflicts
        /// </summary>
        public static IServiceCollection AddMinimalSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "LexiFlow API",
                    Version = "v1",
                    Description = "LexiFlow API Documentation"
                });

                // Basic JWT Authentication
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }

        /// <summary>
        /// Configure minimal SwaggerUI ?? tr�nh compression conflicts
        /// </summary>
        public static void UseMinimalSwaggerUI(this IApplicationBuilder app, IHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LexiFlow API v1");
                    c.RoutePrefix = string.Empty;
                    
                    // CRITICAL: Minimal configuration ?? tr�nh large payloads
                    c.ConfigObject.AdditionalItems["docExpansion"] = "none";
                    c.ConfigObject.AdditionalItems["defaultModelsExpandDepth"] = -1; // Hide models
                    c.ConfigObject.AdditionalItems["defaultModelExpandDepth"] = -1;
                    c.ConfigObject.AdditionalItems["tryItOutEnabled"] = false;
                    
                    // Disable c�c features c� th? g�y large responses
                    c.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Example);
                });
            }
        }
    }
}