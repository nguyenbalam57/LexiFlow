using Asp.Versioning;
using LexiFlow.API.Extensions;
using LexiFlow.API.Middleware;
using LexiFlow.API.Services.Vocabulary;
using LexiFlow.Infrastructure.Data;
using LexiFlow.Infrastructure.Data.Seed;
using LexiFlow.Infrastructure.Data.Repositories.Vocabulary;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ===== CẤU HÌNH CƠ SỞ DỮ LIỆU =====
// Sử dụng LexiFlowContext với SQL Server
builder.Services.AddDbContext<LexiFlowContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

    // Cấu hình cho môi trường development
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging(false);
        options.EnableDetailedErrors(true);
    }

    // Giảm thiểu EF Core warnings
    options.ConfigureWarnings(warnings =>
    {
        warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.CoreEventId.NavigationBaseIncludeIgnored);
        warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.ForeignKeyPropertiesMappedToUnrelatedTables);
    });
});

// ===== CẤU HÌNH LOGGING =====
// Thiết lập Serilog cho việc ghi log
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/lexiflow-api-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// ===== CẤU HÌNH CONTROLLERS =====
// Thêm controllers với cấu hình JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// ===== CẤU HÌNH API VERSIONING =====
// Hỗ trợ phiên bản hóa API
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// ===== CẤU HÌNH VALIDATION =====
// Đăng ký FluentValidation services
builder.Services.AddValidationServices();

// ===== ĐĂNG KÝ SERVICES & REPOSITORIES =====
// Repository và Service cho Vocabulary
builder.Services.AddScoped<IVocabularyRepository, VocabularyRepository>();
builder.Services.AddScoped<IVocabularyService, LexiFlow.API.Services.Vocabulary.VocabularyService>();

// Swagger API Explorer
builder.Services.AddEndpointsApiExplorer();

// ===== CẤU HÌNH SWAGGER (ENHANCED) =====
// Swagger với custom styling và enhanced features
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LexiFlow API",
        Version = "v1",
        Description = "API cho ứng dụng học tiếng Nhật LexiFlow",
        Contact = new OpenApiContact
        {
            Name = "LexiFlow Development Team",
            Email = "dev@lexiflow.com"
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    // Cấu hình JWT Authentication cho Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \n\nNhập 'Bearer' [space] và token của bạn trong text input bên dưới.\n\nVí dụ: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });

    // Enable XML comments if available
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// ===== CẤU HÌNH JWT AUTHENTICATION =====
// Xác thực JWT để bảo vệ API
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
                throw new InvalidOperationException("JWT Key chưa được cấu hình")))
        };
    });

builder.Services.AddAuthorization();

// ===== CẤU HÌNH CORS =====
// Cross-Origin Resource Sharing cho client applications
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("CORS:AllowedOrigins").Get<string[]>() ?? new[] { "*" })
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// ===== CẤU HÌNH HEALTH CHECKS =====
// Kiểm tra sức khỏe ứng dụng
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        name: "database",
        tags: new[] { "database" });

// HTTP Client factory cho external API calls
builder.Services.AddHttpClient();

// ===== XÂY DỰNG ỨNG DỤNG =====
var app = builder.Build();

// Logger để ghi log trong pipeline
var logger = app.Services.GetRequiredService<ILogger<Program>>();

// ===== CẤU HÌNH PIPELINE - ĐÃ SỬA LỖI CONTENT-LENGTH =====

// 1. Error handling middleware phải đặt đầu tiên
app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    // 2. Swagger đặt sớm, TRƯỚC static files và logging
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LexiFlow API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "LexiFlow API - Japanese Learning Platform";
        c.DisplayRequestDuration();
        c.EnableTryItOutByDefault();
        c.ShowExtensions();
        c.EnableValidator();

        // BỎ INJECT CSS/JS VÀO SWAGGER UI để tránh xung đột
        // c.InjectStylesheet("/css/lexiflow-api.css");
        // c.InjectJavascript("/js/lexiflow-api.js");

        // Custom configuration for better UX
        c.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object>
        {
            ["activated"] = true,
            ["theme"] = "agate"
        };
    });

    logger.LogInformation("Development mode: Enhanced Swagger UI enabled at /swagger");
}
else
{
    // Production: Standard Swagger
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LexiFlow API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "LexiFlow API Documentation";
        // Tạm thời bỏ inject CSS trong production
        // c.InjectStylesheet("/css/lexiflow-api.css");
    });

    // Enable HTTPS redirection in production
    if (builder.Configuration.GetValue<bool>("Security:RequireHttps", true))
    {
        app.UseHttpsRedirection();
    }

    logger.LogInformation("Production mode: Standard Swagger UI");
}

// 3. Static files với cấu hình đơn giản hơn
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Đơn giản hóa cache headers để tránh xung đột
        var cachePeriod = app.Environment.IsDevelopment() ? "3600" : "86400"; // 1h dev, 1 day prod
        ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}");
    }
});

// 4. Default files
app.UseDefaultFiles();

// 5. Request logging với cấu hình tối ưu để tránh xung đột với Swagger
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    options.GetLevel = (httpContext, elapsed, ex) =>
    {
        var path = httpContext.Request.Path.Value?.ToLower();

        // TRÁNH LOG SWAGGER REQUESTS để giảm xung đột
        if (path != null && (path.StartsWith("/swagger") ||
                           path.Contains("swagger.json") ||
                           path.EndsWith(".css") ||
                           path.EndsWith(".js") ||
                           path.EndsWith(".html") ||
                           path.EndsWith(".ico") ||
                           path.EndsWith(".png")))
        {
            return Serilog.Events.LogEventLevel.Verbose; // Thay đổi từ Debug sang Verbose
        }

        if (ex != null)
            return Serilog.Events.LogEventLevel.Error;

        if (httpContext.Response.StatusCode >= 400)
            return Serilog.Events.LogEventLevel.Warning;

        return Serilog.Events.LogEventLevel.Information;
    };

    // BỎ QUA một số requests để giảm xung đột
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        var path = httpContext.Request.Path.Value?.ToLower();
        if (path != null && path.StartsWith("/swagger"))
        {
            return; // Không enrich cho Swagger requests
        }

        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
    };
});

// 6. CORS
app.UseCors("CorsPolicy");

// 7. Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// 8. Map endpoints cuối cùng
app.MapControllers();
app.MapHealthChecks("/health");

// ===== KHỞI TẠO DATABASE =====
// Khởi tạo database với xử lý lỗi cải thiện
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbLogger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var dbContext = scope.ServiceProvider.GetRequiredService<LexiFlowContext>();

        dbLogger.LogInformation("Kiểm tra trạng thái database...");

        // Kiểm tra kết nối database
        await dbContext.Database.CanConnectAsync();
        dbLogger.LogInformation("Kết nối database thành công. Kiểm tra migrations...");

        // Áp dụng pending migrations nếu có
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            dbLogger.LogInformation("Đang áp dụng {Count} migrations...", pendingMigrations.Count());
            await dbContext.Database.MigrateAsync();
            dbLogger.LogInformation("Migrations đã được áp dụng thành công");
        }
        else
        {
            dbLogger.LogInformation("Database đã được cập nhật");
        }

        // Khởi tạo dữ liệu mẫu
        await app.Services.SeedDatabaseAsync();
    }
    catch (Exception ex)
    {
        var dbLogger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        dbLogger.LogError(ex, "Lỗi khi khởi tạo database");

        // Không throw exception để app vẫn có thể khởi động
        // throw; // Comment để app không crash khi database có vấn đề
    }
}

// ===== KHỞI ĐỘNG ỨNG DỤNG =====
try
{
    Log.Information("🌸 Khởi động LexiFlow API - Enhanced Mode với Static Files...");
    Log.Information("🏠 Trang chủ: http://localhost:5117/");
    Log.Information("📚 Swagger UI: http://localhost:5117/swagger");
    Log.Information("💚 Health Check: http://localhost:5117/health");
    Log.Information("Ctrl + C : Shutdown API");
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "⌛ Ứng dụng bị dừng bất ngờ");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}