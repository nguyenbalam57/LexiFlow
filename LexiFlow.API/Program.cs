using Asp.Versioning;
using LexiFlow.API.Extensions;
using LexiFlow.API.Middleware;
using LexiFlow.API.Services;
using LexiFlow.API.Hubs;
using LexiFlow.Infrastructure.Data;
using LexiFlow.Infrastructure.Data.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Database Configuration - Use LexiFlowContext instead of ApplicationDbContext
builder.Services.AddDbContext<LexiFlowContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/lexiflow-api-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

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

// Register Analytics Services
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IAnalyticsHubService, AnalyticsHubService>();

builder.Services.AddEndpointsApiExplorer();

// 3. Configure Swagger with JWT Authentication
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = builder.Configuration["API:Title"] ?? "LexiFlow API",
        Version = "v1",
        Description = builder.Configuration["API:Description"] ?? "API for LexiFlow Japanese Vocabulary Learning Application with Real-time Analytics",
        Contact = new OpenApiContact
        {
            Name = builder.Configuration["API:Contact:Name"] ?? "LexiFlow Team",
            Email = builder.Configuration["API:Contact:Email"] ?? "support@lexiflow.app",
            Url = new Uri(builder.Configuration["API:Contact:Url"] ?? "https://lexiflow.app")
        }
    });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
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

    // Include XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

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

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LexiFlow API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
        c.ConfigObject.AdditionalItems["disableCompression"] = true;
    });
    //app.UseDeveloperExceptionPage();
}
else
{
    // Use HTTPS Redirection in production
    if (builder.Configuration.GetValue<bool>("Security:RequireHttps", true))
    {
        app.UseHttpsRedirection();
    }
}

// Add error handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

// Use Serilog request logging
app.UseSerilogRequestLogging();

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

// Seed database - TEMPORARILY DISABLED
await app.Services.SeedDatabaseAsync();

// Apply database migrations on startup in development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<LexiFlowContext>();
        dbContext.Database.Migrate();
        Log.Information("Database migrations applied successfully");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while applying database migrations");
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