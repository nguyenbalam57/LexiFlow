using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;

namespace LexiFlow.API.Configurations
{
    /// <summary>
    /// Configuration cho Swagger ?? tránh Content-Length mismatch errors
    /// </summary>
    public static class SwaggerConfiguration
    {
        /// <summary>
        /// Configure Swagger v?i settings optimal cho .NET 9
        /// </summary>
        public static IServiceCollection AddSwaggerWithCompressionFix(this IServiceCollection services, IHostEnvironment environment)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "LexiFlow API",
                    Version = "v1",
                    Description = "API for LexiFlow Japanese Learning Platform"
                });

                // JWT Authentication
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
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

                // KH?C PH?C: Optimize ?? tránh large payload issues
                if (environment.IsDevelopment())
                {
                    // Development: Disable problematic features
                    c.DocumentFilter<RemoveVerboseSchemas>();
                    c.SchemaFilter<SimplifyComplexSchemas>();
                }
            });

            return services;
        }

        /// <summary>
        /// Configure SwaggerUI v?i settings t?i ?u
        /// </summary>
        public static void UseOptimizedSwaggerUI(this IApplicationBuilder app, IHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseSwagger(); // Remove SerializeAsV2 property

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LexiFlow API v1");
                    c.RoutePrefix = string.Empty;
                    
                    // KH?C PH?C: T?i ?u ?? tránh Content-Length issues
                    c.ConfigObject.AdditionalItems.Clear(); // Clear all additional items
                    c.ConfigObject.AdditionalItems["docExpansion"] = "none"; // Collapse by default
                    c.ConfigObject.AdditionalItems["defaultModelsExpandDepth"] = 0; // Don't expand models
                    c.ConfigObject.AdditionalItems["defaultModelExpandDepth"] = 0;
                    c.ConfigObject.AdditionalItems["operationsSorter"] = "alpha";
                    c.ConfigObject.AdditionalItems["tagsSorter"] = "alpha";
                    c.ConfigObject.AdditionalItems["tryItOutEnabled"] = false; // Disable try it out
                    c.ConfigObject.AdditionalItems["filter"] = true;
                    
                    // Performance settings
                    c.ConfigObject.AdditionalItems["deepLinking"] = false;
                    c.ConfigObject.AdditionalItems["showRequestHeaders"] = false;
                    c.ConfigObject.AdditionalItems["showCommonExtensions"] = false;
                });
            }
        }
    }

    /// <summary>
    /// Document filter ?? remove verbose schemas
    /// </summary>
    public class RemoveVerboseSchemas : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // Remove complex schemas that might cause large payloads
            var schemasToRemove = swaggerDoc.Components.Schemas
                .Where(s => s.Key.Contains("Dictionary") || 
                           s.Key.Contains("ICollection") ||
                           s.Key.Contains("List"))
                .Select(s => s.Key)
                .ToList();

            foreach (var schemaKey in schemasToRemove)
            {
                swaggerDoc.Components.Schemas.Remove(schemaKey);
            }
        }
    }

    /// <summary>
    /// Schema filter ?? simplify complex schemas
    /// </summary>
    public class SimplifyComplexSchemas : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            // Simplify schemas that might be too large
            if (schema.Properties?.Count > 50)
            {
                // Keep only first 20 properties for display
                var importantProps = schema.Properties.Take(20).ToDictionary(p => p.Key, p => p.Value);
                schema.Properties.Clear();
                foreach (var prop in importantProps)
                {
                    schema.Properties.Add(prop.Key, prop.Value);
                }

                // Add note about truncation
                schema.Description = schema.Description + " (Schema truncated for performance)";
            }
        }
    }
}