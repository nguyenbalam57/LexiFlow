using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LexiFlow.Infrastructure.Data;
using LexiFlow.AdminDashboard.Services;
using LexiFlow.AdminDashboard.Services.Implementation;
using LexiFlow.AdminDashboard.ViewModels;

namespace LexiFlow.AdminDashboard;

/// <summary>
/// Interaction logic for App.xaml with Dependency Injection
/// </summary>
public partial class App : Application
{
    private IHost? _host;
    public static IServiceProvider ServiceProvider { get; private set; } = null!;

    protected override async void OnStartup(StartupEventArgs e)
    {
        try
        {
            // Build host with dependency injection
            _host = CreateHostBuilder().Build();
            ServiceProvider = _host.Services;

            // Start the host
            await _host.StartAsync();

            // Initialize database if needed
            await InitializeDatabaseAsync();

            // Show main window with proper DI
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Application startup failed: {ex.Message}", "Error", 
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
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", 
                    optional: true, reloadOnChange: true);
                config.AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                ConfigureServices(services, context.Configuration);
            })
            .ConfigureLogging((context, logging) =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddDebug();
                logging.AddEventSourceLogger();
                logging.SetMinimumLevel(LogLevel.Information);
            });
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Database Context
        services.AddDbContext<LexiFlowContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LexiFlowDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            options.UseSqlServer(connectionString);
            options.EnableSensitiveDataLogging(false);
            options.EnableServiceProviderCaching();
        });

        // Infrastructure Services
        services.AddHttpClient<IApiClient, ApiClient>(client =>
        {
            var apiBaseUrl = configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7001";
            client.BaseAddress = new Uri(apiBaseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        // Core Services
        services.AddScoped<IVocabularyManagementService, VocabularyManagementService>();
        services.AddScoped<IUserManagementService, UserManagementService>();
        services.AddScoped<IRealTimeAnalyticsService, RealTimeAnalyticsService>();
        services.AddScoped<IDialogService, DialogService>();

        // Analytics Services
        services.AddScoped<AnalyticsChartDemoService>();
        services.AddScoped<AnalyticsDatabaseService>();
        services.AddScoped<PredictiveAnalyticsService>();
        services.AddScoped<ChartExportService>();

        // Cache Services
        services.AddScoped<ApiCacheService>();
        services.AddScoped<ApiSyncService>();
        services.AddScoped<ApiErrorHandler>();

        // ViewModels
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<VocabularyManagementViewModel>();
        //services.AddTransient<AdditionalViewModels>();

        // Views
        services.AddTransient<MainWindow>();
        services.AddTransient<Views.DashboardView>();
        services.AddTransient<Views.VocabularyManagementView>();
        services.AddTransient<Views.UserManagementView>();
        services.AddTransient<Views.KanjiManagementView>();
        services.AddTransient<Views.GrammarManagementView>();
        services.AddTransient<Views.MediaManagementView>();
        services.AddTransient<Views.ExamManagementView>();
        services.AddTransient<Views.AnalyticsView>();
        services.AddTransient<Views.SettingsView>();

        // Dialogs
        services.AddTransient<Views.Dialogs.VocabularyEditDialog>();
        services.AddTransient<Views.Dialogs.CategoryEditDialog>();

        // Configuration Options
        services.Configure<ApiSettings>(configuration.GetSection("ApiSettings"));
        services.Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings"));
        services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));
    }

    private async Task InitializeDatabaseAsync()
    {
        try
        {
            using var scope = ServiceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<LexiFlowContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<App>>();

            logger.LogInformation("Checking database connection...");

            // Ensure database is created and up to date
            var canConnect = await context.Database.CanConnectAsync();
            if (!canConnect)
            {
                logger.LogWarning("Cannot connect to database. Creating database...");
                await context.Database.EnsureCreatedAsync();
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

// Configuration classes
public class ApiSettings
{
    public string BaseUrl { get; set; } = "https://localhost:7001";
    public int TimeoutSeconds { get; set; } = 30;
    public bool EnableRetry { get; set; } = true;
    public int MaxRetryAttempts { get; set; } = 3;
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
}

