using AspNetCoreRateLimit;
using LexiFlow.TranslationAPI.Configuration;
using LexiFlow.TranslationAPI.Middleware;
using LexiFlow.TranslationAPI.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();
builder.Host.UseWindowsService();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "LexiFlow Translation API",
        Version = "v1",
        Description = "Machine Translation API using Facebook M2M-100 model"
    });

    // Include XML comments
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Memory cache
builder.Services.AddMemoryCache();

// Rate limiting
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// HttpClient
builder.Services.AddHttpClient<ITranslationService, TranslationService>();
builder.Services.AddHttpClient<TranslationHealthCheck>();

// Custom services
builder.Services.Configure<TranslationServiceOptions>(
    builder.Configuration.GetSection("TranslationService"));
builder.Services.AddScoped<ITranslationService, TranslationService>();
builder.Services.AddSingleton<PythonServiceManager>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Health checks
builder.Services.AddHealthChecks()
    .AddCheck<TranslationHealthCheck>("translation_service", HealthStatus.Degraded, new[] { "external", "translation" })
    .AddCheck("self", () => HealthCheckResult.Healthy("API is running"), new[] { "api" });

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LexiFlow Translation API v1");
        c.RoutePrefix = string.Empty; // Swagger at root
        c.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseIpRateLimiting();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

// Start Python service
var pythonService = app.Services.GetRequiredService<PythonServiceManager>();
await pythonService.StartAsync();

Log.Information("LexiFlow Translation API started");

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}