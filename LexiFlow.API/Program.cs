using Asp.Versioning;
using LexiFlow.API.Extensions;
using LexiFlow.API.Middleware;
using LexiFlow.API.Services;
using LexiFlow.API.Services.Vocabulary;
using LexiFlow.API.Hubs;
using LexiFlow.API.Configurations;
using LexiFlow.Infrastructure.Data;
using LexiFlow.Infrastructure.Data.Seed;
using LexiFlow.Infrastructure.Data.Repositories.Vocabulary;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;
using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// 1. Database Configuration - Use LexiFlowContext instead of ApplicationDbContext
builder.Services.AddDbContext<LexiFlowContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    
    // S?A L?I: Gi?m thi?u EF Core warnings - ??n gi?n hóa
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging(false);
        options.EnableDetailedErrors(true);
    }
    
    // C?u hình ?? gi?m warnings - b? các event ID không t?n t?i
    options.ConfigureWarnings(warnings =>
    {
        warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.CoreEventId.NavigationBaseIncludeIgnored);
        warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.ForeignKeyPropertiesMappedToUnrelatedTables);
    });
});

// 2. Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/lexiflow-api-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// S?A L?I CRITICAL: Always register ResponseCompression services, nh?ng ch? USE trong production
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    
    // Ch? compress JSON responses t? API
    options.MimeTypes = new[]
    {
        "application/json",
        "text/json"
    };
});

// Configure compression levels ?? tránh Content-Length issues
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// Add SignalR for real-time analytics
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
});

// Add API versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Add FluentValidation
builder.Services.AddValidationServices();

// Register Vocabulary Services và Repositories
builder.Services.AddScoped<IVocabularyRepository, VocabularyRepository>();
builder.Services.AddScoped<IVocabularyService, LexiFlow.API.Services.Vocabulary.VocabularyService>();

// TODO: Fix SyncService namespace conflicts later
// Register Analytics Services
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IAnalyticsHubService, AnalyticsHubService>();

builder.Services.AddEndpointsApiExplorer();

// 3. ULTIMATE FIX: Minimal Swagger registration ?? tránh Content-Length issues
builder.Services.AddMinimalSwagger();

// 4. Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ??
                throw new InvalidOperationException("JWT Key is not configured")))
        };

        // Configure SignalR authentication
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

// Add Authorization
builder.Services.AddAuthorization();

// 5. Configure CORS for client applications including SignalR
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("CORS:AllowedOrigins").Get<string[]>() ?? new[] { "*" })
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Required for SignalR
    });

    // Add LANPolicy for local network access (mobile apps)
    options.AddPolicy("LANPolicy", policy =>
    {
        policy.WithOrigins("http://192.168.*.*", "http://10.*.*.*")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), name: "database", tags: new[] { "database" })
    .AddCheck("signalr", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("SignalR is running"));

// Configure HTTP Client factory
builder.Services.AddHttpClient();

// Add Memory Cache for analytics caching
builder.Services.AddMemoryCache();

var app = builder.Build();

// Create logger ?? s? d?ng trong pipeline configuration
var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    // GI?I PHÁP CU?I CÙNG: Early response interception
    // ??t ? v? trí ??u tiên ?? intercept tr??c khi Swagger middleware x? lý
    app.UseSwaggerEarlyResponse();
    
    // S?A L?I: S? d?ng minimal Swagger configuration
    app.UseMinimalSwaggerUI(app.Environment);
    
    // CRITICAL: KHÔNG S? D?NG ResponseCompression middleware trong development
    // Services ?ã ???c register nh?ng middleware không ???c s? d?ng
    logger.LogInformation("Development mode: Response compression services registered but middleware DISABLED, Early response interception ENABLED");
}
else
{
    // Production: Enable compression nh?ng exclude Swagger hoàn toàn
    app.UseWhen(
        context => {
            var path = context.Request.Path.Value?.ToLower();
            return !string.IsNullOrEmpty(path) &&
                   !path.StartsWith("/swagger") &&
                   !path.Contains("swagger") &&
                   !path.Equals("/");
        },
        appBuilder => appBuilder.UseResponseCompression()
    );
    
    app.UseMinimalSwaggerUI(app.Environment);
    
    // Use HTTPS Redirection in production
    if (builder.Configuration.GetValue<bool>("Security:RequireHttps", true))
    {
        app.UseHttpsRedirection();
    }
    
    logger.LogInformation("Production mode: Conditional response compression ENABLED");
}

// Configure pipeline - ULTIMATE SWAGGER & COMPRESSION FIX
if (app.Environment.IsDevelopment())
{
    // GI?I PHÁP CU?I CÙNG: Early response interception
    // ??t ? v? trí ??u tiên ?? intercept tr??c khi Swagger middleware x? lý
    app.UseSwaggerEarlyResponse();
    
    // S?A L?I: S? d?ng minimal Swagger configuration
    app.UseMinimalSwaggerUI(app.Environment);
    
    // CRITICAL: KHÔNG S? D?NG ResponseCompression middleware trong development
    // Services ?ã ???c register nh?ng middleware không ???c s? d?ng
    logger.LogInformation("Development mode: Response compression services registered but middleware DISABLED, Early response interception ENABLED");
}
else
{
    // Production: Enable compression nh?ng exclude Swagger hoàn toàn
    app.UseWhen(
        context => {
            var path = context.Request.Path.Value?.ToLower();
            return !string.IsNullOrEmpty(path) &&
                   !path.StartsWith("/swagger") &&
                   !path.Contains("swagger") &&
                   !path.Equals("/");
        },
        appBuilder => appBuilder.UseResponseCompression()
    );
    
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LexiFlow API v1");
        c.RoutePrefix = "swagger";
    });
    
    // Use HTTPS Redirection in production
    if (builder.Configuration.GetValue<bool>("Security:RequireHttps", true))
    {
        app.UseHttpsRedirection();
    }
    
    logger.LogInformation("Production mode: Normal Swagger with conditional compression");
}

// S?A L?I: Improved conditional Response Compression - ch? cho API endpoints
app.UseWhen(
    context => {
        var path = context.Request.Path.Value?.ToLower();
        return !string.IsNullOrEmpty(path) &&
               path.StartsWith("/api/") &&
               !path.StartsWith("/swagger");
    },
    appBuilder => appBuilder.UseResponseCompression()
);

// Add error handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

// Enable static files BEFORE Serilog request logging ?? tránh spam logs cho static files
app.UseStaticFiles();

// Use Serilog request logging
app.UseSerilogRequestLogging(options =>
{
    // S?a l?i: Gi?m log level cho static files ?? tránh spam logs
    options.MessageTemplate = "Handled {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    options.GetLevel = (httpContext, elapsed, ex) =>
    {
        // Gi?m log level cho static files và Swagger
        var path = httpContext.Request.Path.Value?.ToLower();
        if (path != null && (path.StartsWith("/swagger") || 
                           path.EndsWith(".css") || 
                           path.EndsWith(".js") || 
                           path.EndsWith(".html") ||
                           path.EndsWith(".ico")))
        {
            return Serilog.Events.LogEventLevel.Debug;
        }
        return ex != null ? Serilog.Events.LogEventLevel.Error : Serilog.Events.LogEventLevel.Information;
    };
});

// Use CORS
app.UseCors("CorsPolicy");

// Use Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapControllers();
app.MapHealthChecks("/health");

// Map SignalR Hub
app.MapHub<AnalyticsHub>("/hubs/analytics");

// Database initialization v?i improved error handling
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbLogger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var dbContext = scope.ServiceProvider.GetRequiredService<LexiFlowContext>();
        
        dbLogger.LogInformation("Checking database status...");
        
        // Check database connection
        await dbContext.Database.CanConnectAsync();
        dbLogger.LogInformation("Database connection established. Checking for migrations...");
        
        // Apply pending migrations if any
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            dbLogger.LogInformation("Applying {Count} pending migrations...", pendingMigrations.Count());
            await dbContext.Database.MigrateAsync();
            dbLogger.LogInformation("Database migrations applied successfully");
        }
        else
        {
            dbLogger.LogInformation("Database ?ã ???c c?p nh?t.");
        }
        
        // Seed database
        await app.Services.SeedDatabaseAsync();
        
    }
    catch (Exception ex)
    {
        var dbLogger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        dbLogger.LogError(ex, "An error occurred while initializing database");
        
        // Không throw exception ?? app v?n có th? start
        // throw; // Comment out ?? app không crash khi database có v?n ??
    }
}

try
{
    Log.Information("Starting LexiFlow API with Real-time Analytics");
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}