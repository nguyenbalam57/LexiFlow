using LexiFlow.AdminDashboard.Services;
using LexiFlow.AdminDashboard.Services.Implementation;
using LexiFlow.AdminDashboard.ViewModels;
using LexiFlow.AdminDashboard.ViewModels.LoginViewModels;
using LexiFlow.AdminDashboard.Views.LoginViews;
using LexiFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Media.Animation;

namespace LexiFlow.AdminDashboard;

/// <summary>
/// Interaction logic for App.xaml with Dependency Injection
/// </summary>
public partial class App : Application
{
    private IHost? _host;
    public static IServiceProvider ServiceProvider { get; private set; } = null!;
    public static IConfiguration Configuration { get; private set; } = null!;

    protected override async void OnStartup(StartupEventArgs e)
    {
        try
        {
            // Build host with dependency injection
            _host = CreateHostBuilder().Build();
            ServiceProvider = _host.Services;
            Configuration = _host.Services.GetRequiredService<IConfiguration>();

            // Start the host
            await _host.StartAsync();

            // Apply UI settings from configuration
            ApplyUISettings();

            // Initialize database if needed
            await InitializeDatabaseAsync();

            // Show login window with proper DI
            var login = ServiceProvider.GetRequiredService<LoginView>();
            login.Show();

            base.OnStartup(e);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Application startup failed: {ex.Message}\n\nStackTrace:\n{ex.StackTrace}", "Error", 
                MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown(1);
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_host != null)
        {
            await _host.StopAsync();
            _host.Dispose();
        }

        base.OnExit(e);
    }

    private static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration((context, config) =>
            {
                // Clear any existing sources
                config.Sources.Clear();

                // Add configuration files with proper error handling
                var appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
                var devSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), $"appsettings.{context.HostingEnvironment.EnvironmentName}.json");

                // Add base configuration file
                if (File.Exists(appSettingsPath))
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                }
                else
                {
                    // Create a minimal configuration if appsettings.json doesn't exist
                    CreateDefaultAppSettings(appSettingsPath);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                }

                // Add environment-specific configuration
                if (File.Exists(devSettingsPath))
                {
                    config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", 
                        optional: true, reloadOnChange: true);
                }

                // Add other sources
                config.AddEnvironmentVariables();
                
                // Add command line if available
                var args = Environment.GetCommandLineArgs();
                if (args.Length > 1)
                {
                    config.AddCommandLine(args);
                }
            })
            .ConfigureServices((context, services) =>
            {
                ConfigureServices(services, context.Configuration);
            })
            .ConfigureLogging((context, logging) =>
            {
                // Configure logging from appsettings.json
                logging.ClearProviders();
                
                try
                {
                    logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                }
                catch
                {
                    // Use default logging if configuration fails
                    logging.SetMinimumLevel(LogLevel.Information);
                }
                
                logging.AddConsole();
                logging.AddDebug();
                logging.AddEventSourceLogger();
                
                // Set minimum level from configuration or default to Information
                var logLevel = context.Configuration.GetValue<LogLevel?>("Logging:LogLevel:Default") ?? LogLevel.Information;
                logging.SetMinimumLevel(logLevel);
            });
    }

    private static void CreateDefaultAppSettings(string filePath)
    {
        var defaultSettings = @"{
  ""Logging"": {
    ""LogLevel"": {
      ""Default"": ""Information"",
      ""Microsoft"": ""Warning"",
      ""Microsoft.Hosting.Lifetime"": ""Information"",
      ""LexiFlow"": ""Debug""
    }
  },
  ""ApiSettings"": {
    ""BaseUrl"": ""https://localhost:5117"",
    ""TimeoutSeconds"": 30,
    ""EnableRetry"": true,
    ""MaxRetryAttempts"": 3,
    ""UseAuthToken"": true,
    ""AuthTokenKey"": ""LexiFlow.AuthToken""
  },
  ""DatabaseSettings"": {
    ""EnableSensitiveDataLogging"": false,
    ""CommandTimeoutSeconds"": 30
  },
  ""CacheSettings"": {
    ""EnableCaching"": true,
    ""DefaultCacheExpirationMinutes"": 30,
    ""MaxCacheSize"": 1000,
    ""LogCacheOperations"": true
  },
  ""ApiEndpoints"": {
    ""Dashboard"": ""api/admin/dashboard"",
    ""Users"": ""api/admin/users"",
    ""Vocabulary"": ""api/vocabulary"",
    ""Categories"": ""api/categories"",
    ""Roles"": ""api/admin/roles"",
    ""Activities"": ""api/admin/activities"",
    ""Settings"": ""api/admin/settings"",
    ""Database"": ""api/admin/database"",
    ""Logs"": ""api/admin/logs""
  },
  ""UI"": {
    ""Theme"": ""Light"",
    ""Language"": ""vi-VN"",
    ""AutoRefreshInterval"": 30,
    ""PageSize"": 50,
    ""EnableAnimations"": true,
    ""ShowDebugInfo"": false
  },
  ""Features"": {
    ""EnableRealTimeUpdates"": true,
    ""EnableOfflineMode"": false,
    ""EnableAdvancedAnalytics"": true,
    ""EnableBulkOperations"": true,
    ""EnableExportImport"": true,
    ""EnableUserManagement"": true,
    ""EnableRoleManagement"": true,
    ""EnableSystemSettings"": true,
    ""EnableDatabaseTools"": true,
    ""EnableActivityMonitoring"": true,
    ""EnablePerformanceMonitoring"": true,
    ""EnableContentManagement"": true,
    ""EnableVocabularyManagement"": true,
    ""EnableKanjiManagement"": true,
    ""EnableGrammarManagement"": true
  },
  ""ApiSync"": {
    ""MaxBatchSize"": 100,
    ""AutoSyncIntervalMinutes"": 5,
    ""EnableBackgroundSync"": true,
    ""SyncOnStartup"": true,
    ""Direction"": ""Both"",
    ""RetryCount"": 3,
    ""RetryDelaySeconds"": 5,
    ""SyncTables"": [
      ""Users"",
      ""Roles"",
      ""Categories"",
      ""Vocabulary"",
      ""Kanji"",
      ""Grammar"",
      ""Lessons"",
      ""Courses"",
      ""Exercises"",
      ""LearningProgress""
    ]
  }
}";

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? "");
            File.WriteAllText(filePath, defaultSettings);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to create default appsettings.json: {ex.Message}", "Configuration Error", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {

        // HTTP Client configuration
        services.AddHttpClient<IApiClient, ApiClient>(client =>
        {
            var apiSettings = configuration.GetSection("ApiSettings").Get<ApiSettings>() ?? new ApiSettings();
            
            client.BaseAddress = new Uri(apiSettings.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(apiSettings.TimeoutSeconds);
            
            // Add default headers
            client.DefaultRequestHeaders.Add("User-Agent", "LexiFlow-AdminDashboard/1.0");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        })
            .ConfigurePrimaryHttpMessageHandler(() =>
        {
            var handler = new HttpClientHandler();

            // Only bypass SSL validation in development
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) => true;
            }

            return handler;
        });

        // Register API Client with proper configuration
        services.AddScoped<IApiClient, ApiClient>();

        // Configure all settings classes
        ConfigureSettingsClasses(services, configuration);

        // Configure core services
        ConfigureCoreServices(services, configuration);

        // Configure analytics services
        ConfigureAnalyticsServices(services, configuration);

        // Configure cache services
        ConfigureCacheServices(services, configuration);

        // Configure sync services
        ConfigureSyncServices(services, configuration);

        // Configure ViewModels
        ConfigureViewModels(services, configuration);

        // Configure Views and Dialogs
        ConfigureViews(services, configuration);
    }

    private static void ConfigureSettingsClasses(IServiceCollection services, IConfiguration configuration)
    {
        // API Settings
        services.Configure<ApiSettings>(configuration.GetSection("ApiSettings"));
        
        // Database Settings
        services.Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings"));
        
        // Cache Settings
        services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));
        
        // API Sync Settings
        services.Configure<ApiSyncOptions>(configuration.GetSection("ApiSync"));
        
        // API Endpoints
        services.Configure<ApiEndpointsSettings>(configuration.GetSection("ApiEndpoints"));
        
        // UI Settings
        services.Configure<UISettings>(configuration.GetSection("UI"));
        
        // Feature Settings
        services.Configure<FeatureSettings>(configuration.GetSection("Features"));
    }

    private static void ConfigureCoreServices(IServiceCollection services, IConfiguration configuration)
    {
        var features = configuration.GetSection("Features").Get<FeatureSettings>() ?? new FeatureSettings();

        // Core Services
        if (features.EnableVocabularyManagement)
        {
            services.AddScoped<IVocabularyManagementService, VocabularyManagementService>();
        }

        if (features.EnableUserManagement)
        {
            services.AddScoped<IUserManagementService, UserManagementService>();
        }

        if (features.EnableRealTimeUpdates)
        {
            services.AddScoped<IRealTimeAnalyticsService, RealTimeAnalyticsService>();
        }

        services.AddScoped<IDialogService, DialogService>();
        services.AddScoped<IConfigurationService, ConfigurationService>();
    }

    private static void ConfigureAnalyticsServices(IServiceCollection services, IConfiguration configuration)
    {
        var features = configuration.GetSection("Features").Get<FeatureSettings>() ?? new FeatureSettings();

        if (features.EnableAdvancedAnalytics)
        {
            services.AddScoped<AnalyticsChartDemoService>();
            services.AddScoped<AnalyticsDatabaseService>();
            services.AddScoped<PredictiveAnalyticsService>();
        }

        if (features.EnablePerformanceMonitoring)
        {
            services.AddScoped<ChartExportService>();
        }
    }

    private static void ConfigureCacheServices(IServiceCollection services, IConfiguration configuration)
    {
        var cacheSettings = configuration.GetSection("CacheSettings").Get<CacheSettings>() ?? new CacheSettings();

        if (cacheSettings.EnableCaching)
        {
            services.AddScoped<ApiCacheService>();
            services.AddScoped<ApiErrorHandler>();
        }
    }

    private static void ConfigureSyncServices(IServiceCollection services, IConfiguration configuration)
    {
        var syncSettings = configuration.GetSection("ApiSync").Get<ApiSyncOptions>() ?? new ApiSyncOptions();

        if (syncSettings.EnableBackgroundSync)
        {
            services.AddScoped<IApiSyncService, ApiSyncService>();
        }
    }

    private static void ConfigureViewModels(IServiceCollection services, IConfiguration configuration)
    {
        var features = configuration.GetSection("Features").Get<FeatureSettings>() ?? new FeatureSettings();

        // Always available ViewModels
        services.AddTransient<LoginViewModel>();
        services.AddTransient<DashboardViewModel>();

        // Feature-dependent ViewModels
        if (features.EnableVocabularyManagement)
        {
            services.AddTransient<VocabularyManagementViewModel>();
        }

        // Add other ViewModels based on features
    }

    private static void ConfigureViews(IServiceCollection services, IConfiguration configuration)
    {
        var features = configuration.GetSection("Features").Get<FeatureSettings>() ?? new FeatureSettings();

        // Core Views
        services.AddTransient<LoginView>();
        services.AddTransient<MainWindow>();
        services.AddTransient<Views.DashboardView>();

        // Feature-dependent Views
        if (features.EnableVocabularyManagement)
        {
            services.AddTransient<Views.VocabularyManagementView>();
        }

        if (features.EnableUserManagement)
        {
            services.AddTransient<Views.UserManagementView>();
        }

        if (features.EnableKanjiManagement)
        {
            services.AddTransient<Views.KanjiManagementView>();
        }

        if (features.EnableGrammarManagement)
        {
            services.AddTransient<Views.GrammarManagementView>();
        }

        if (features.EnableContentManagement)
        {
            services.AddTransient<Views.MediaManagementView>();
        }

        services.AddTransient<Views.ExamManagementView>();

        if (features.EnableAdvancedAnalytics)
        {
            services.AddTransient<Views.AnalyticsView>();
        }

        if (features.EnableSystemSettings)
        {
            services.AddTransient<Views.SettingsView>();
        }

        // Dialogs
        if (features.EnableVocabularyManagement)
        {
            services.AddTransient<Views.Dialogs.VocabularyEditDialog>();
        }

        services.AddTransient<Views.Dialogs.CategoryEditDialog>();
    }

    private void ApplyUISettings()
    {
        try
        {
            var uiSettings = Configuration.GetSection("UI").Get<UISettings>() ?? new UISettings();

            // Apply theme
            ApplyTheme(uiSettings.Theme);

            // Apply language/culture
            ApplyCulture(uiSettings.Language);

            // Set application properties based on UI settings
            if (!uiSettings.EnableAnimations)
            {
                Timeline.DesiredFrameRateProperty.OverrideMetadata(
                    typeof(Timeline),
                    new FrameworkPropertyMetadata { DefaultValue = 10 });
            }
        }
        catch (Exception ex)
        {
            var logger = ServiceProvider?.GetService<ILogger<App>>();
            logger?.LogWarning(ex, "Failed to apply UI settings from configuration");
        }
    }

    private void ApplyTheme(string theme)
    {
        try
        {
            var themeUri = theme.ToLower() switch
            {
                "dark" => new Uri("pack://application:,,,/Resources/Themes/DarkTheme.xaml"),
                "light" => new Uri("pack://application:,,,/Resources/Themes/LightTheme.xaml"),
                _ => new Uri("pack://application:,,,/Resources/Themes/LightTheme.xaml")
            };

            var themeDict = new ResourceDictionary { Source = themeUri };
            Resources.MergedDictionaries.Clear();
            Resources.MergedDictionaries.Add(themeDict);
        }
        catch (Exception ex)
        {
            var logger = ServiceProvider?.GetService<ILogger<App>>();
            logger?.LogWarning(ex, "Failed to apply theme: {Theme}", theme);
        }
    }

    private void ApplyCulture(string language)
    {
        try
        {
            var culture = new System.Globalization.CultureInfo(language);
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = culture;
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
        catch (Exception ex)
        {
            var logger = ServiceProvider?.GetService<ILogger<App>>();
            logger?.LogWarning(ex, "Failed to apply culture: {Language}", language);
        }
    }

    private async Task InitializeDatabaseAsync()
    {
        try
        {
            using var scope = ServiceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<LexiFlowContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<App>>();
            var dbSettings = Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>() ?? new DatabaseSettings();

            logger.LogInformation("Checking database connection...");

            // Test connection with timeout
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(dbSettings.CommandTimeoutSeconds));
            var canConnect = await context.Database.CanConnectAsync(cts.Token);
            
            if (!canConnect)
            {
                logger.LogWarning("Cannot connect to database. Creating database...");
                await context.Database.EnsureCreatedAsync(cts.Token);
            }

            logger.LogInformation("Database initialization completed successfully");
        }
        catch (Exception ex)
        {
            var logger = ServiceProvider.GetRequiredService<ILogger<App>>();
            logger.LogError(ex, "Database initialization failed");
            
            // Continue without database for now - can work with mock data
            MessageBox.Show(
                "Database connection failed. The application will run with limited functionality.\n\n" +
                $"Error: {ex.Message}",
                "Database Warning",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
    }
}

// Enhanced Configuration classes with all settings from appsettings.json

public class ApiSettings
{
    public string BaseUrl { get; set; } = "https://localhost:5117";
    public int TimeoutSeconds { get; set; } = 30;
    public bool EnableRetry { get; set; } = true;
    public int MaxRetryAttempts { get; set; } = 3;
    public bool UseAuthToken { get; set; } = true;
    public string AuthTokenKey { get; set; } = "LexiFlow.AuthToken";
}

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = "";
    public bool EnableSensitiveDataLogging { get; set; } = false;
    public int CommandTimeoutSeconds { get; set; } = 30;
}

public class CacheSettings
{
    public bool EnableCaching { get; set; } = true;
    public int DefaultCacheExpirationMinutes { get; set; } = 30;
    public int MaxCacheSize { get; set; } = 1000;
    public bool LogCacheOperations { get; set; } = true;
}

public class ApiEndpointsSettings
{
    public string Dashboard { get; set; } = "api/admin/dashboard";
    public string Users { get; set; } = "api/admin/users";
    public string Vocabulary { get; set; } = "api/vocabulary";
    public string Categories { get; set; } = "api/categories";
    public string Roles { get; set; } = "api/admin/roles";
    public string Activities { get; set; } = "api/admin/activities";
    public string Settings { get; set; } = "api/admin/settings";
    public string Database { get; set; } = "api/admin/database";
    public string Logs { get; set; } = "api/admin/logs";
}

public class UISettings
{
    public string Theme { get; set; } = "Light";
    public string Language { get; set; } = "vi-VN";
    public int AutoRefreshInterval { get; set; } = 30;
    public int PageSize { get; set; } = 50;
    public bool EnableAnimations { get; set; } = true;
    public bool ShowDebugInfo { get; set; } = false;
}

public class FeatureSettings
{
    public bool EnableRealTimeUpdates { get; set; } = true;
    public bool EnableOfflineMode { get; set; } = false;
    public bool EnableAdvancedAnalytics { get; set; } = true;
    public bool EnableBulkOperations { get; set; } = true;
    public bool EnableExportImport { get; set; } = true;
    public bool EnableUserManagement { get; set; } = true;
    public bool EnableRoleManagement { get; set; } = true;
    public bool EnableSystemSettings { get; set; } = true;
    public bool EnableDatabaseTools { get; set; } = false; // Disabled for AdminDashboard
    public bool EnableActivityMonitoring { get; set; } = true;
    public bool EnablePerformanceMonitoring { get; set; } = true;
    public bool EnableContentManagement { get; set; } = true;
    public bool EnableVocabularyManagement { get; set; } = true;
    public bool EnableKanjiManagement { get; set; } = true;
    public bool EnableGrammarManagement { get; set; } = true;
}

public class ApiSyncOptions
{
    public int MaxBatchSize { get; set; } = 100;
    public int AutoSyncIntervalMinutes { get; set; } = 5;
    public bool EnableBackgroundSync { get; set; } = false; // Disabled for AdminDashboard
    public bool SyncOnStartup { get; set; } = false; // Disabled for AdminDashboard
    public string Direction { get; set; } = "Both";
    public int RetryCount { get; set; } = 3;
    public int RetryDelaySeconds { get; set; } = 5;
    public List<string> SyncTables { get; set; } = new List<string>();
}

