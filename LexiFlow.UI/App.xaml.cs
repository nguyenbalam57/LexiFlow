using LexiFlow.Application;
using LexiFlow.Core.Interfaces;
using LexiFlow.Infrastructure;
using LexiFlow.Infrastructure.Data;
using LexiFlow.UI.Helpers;
using LexiFlow.UI.Services;
using LexiFlow.UI.Views.Login;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace LexiFlow.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    private readonly IHost _host;
    private ILogger<App>? _logger;

    public App()
    {
        // Create appsettings.json if it doesn't exist
        CreateAppSettingsIfNotExists();

        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                // Configure logging
                services.AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddDebug();
                    builder.SetMinimumLevel(LogLevel.Information);
                });

                // Register application and infrastructure services
                services.AddApplication();
                services.AddInfrastructure(context.Configuration);

                // Register UI-specific services
                services.AddSingleton<ISettingsService, SettingsService>();

                // Register views
                services.AddTransient<LoginView>();
                services.AddTransient<MainWindow>();
            })
            .Build();

        // Set up unhandled exception handler
        this.DispatcherUnhandledException += Application_DispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
    }

    // Create default appsettings.json file if it doesn't exist
    private void CreateAppSettingsIfNotExists()
    {
        try
        {
            string appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            if (!File.Exists(appSettingsPath))
            {
                // Define default appsettings content
                string defaultSettings = @"{
  ""ConnectionStrings"": {
    ""DefaultConnection"": ""Server=.\\SQLEXPRESS;Database=LexiFlow;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true""
  },
  ""Logging"": {
    ""LogLevel"": {
      ""Default"": ""Information"",
      ""Microsoft"": ""Warning"",
      ""Microsoft.Hosting.Lifetime"": ""Information""
    }
  },
  ""AllowedHosts"": ""*"",
  ""AppSettings"": {
    ""AppName"": ""LexiFlow"",
    ""Version"": ""1.0.0"",
    ""DefaultLanguage"": ""VN"",
    ""EnableAutoBackup"": true,
    ""BackupIntervalDays"": 7,
    ""MaxLoginAttempts"": 5,
    ""SessionTimeoutMinutes"": 30
  }
}";
                // Create directory if it doesn't exist
                Directory.CreateDirectory(Path.GetDirectoryName(appSettingsPath));

                // Write the default settings
                File.WriteAllText(appSettingsPath, defaultSettings);

                System.Diagnostics.Debug.WriteLine($"Created default appsettings.json at {appSettingsPath}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to create appsettings.json: {ex.Message}");
            // Don't throw here, we'll handle configuration issues later
        }
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        // Initialize language system
        LanguageHelper.InitializeLanguage();

        await _host.StartAsync();

        // Get logger after host is started
        _logger = _host.Services.GetService<ILogger<App>>();

        // Show splash screen while initializing
        //var splashScreen = new SplashScreen("Resources/Images/splash.png");
        //splashScreen.Show(false, true);

        try
        {
            // Initialize the database
            _logger?.LogInformation("Starting database initialization...");

            var dbInitializer = _host.Services.GetRequiredService<SqlDatabaseInitializer>();
            bool dbInitialized = await dbInitializer.InitializeDatabaseAsync();

            if (!dbInitialized)
            {
                _logger?.LogError("Database initialization failed");

                // Show error dialog
                var result = MessageBox.Show(
                    "Không thể kết nối hoặc khởi tạo cơ sở dữ liệu.\n\n" +
                    "Vui lòng kiểm tra:\n" +
                    "1. SQL Server Express đã được cài đặt\n" +
                    "2. SQL Server service đang chạy\n" +
                    "3. Cấu hình kết nối trong appsettings.json\n\n" +
                    "Bạn có muốn tiếp tục mở ứng dụng không?",
                    "Lỗi Cơ Sở Dữ Liệu",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Error);

                if (result == MessageBoxResult.No)
                {
                    Shutdown();
                    return;
                }
            }
            else
            {
                _logger?.LogInformation("Database initialized successfully");
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to initialize database");

            var result = MessageBox.Show(
                $"Lỗi khởi tạo cơ sở dữ liệu:\n{ex.Message}\n\n" +
                "Bạn có muốn tiếp tục mở ứng dụng không?",
                "Lỗi",
                MessageBoxButton.YesNo,
                MessageBoxImage.Error);

            if (result == MessageBoxResult.No)
            {
                Shutdown();
                return;
            }
        }
        finally
        {
            // Close splash screen
            //splashScreen.Close(TimeSpan.FromSeconds(0.5));
        }

        // Show the login window
        try
        {
            var loginWindow = _host.Services.GetRequiredService<LoginView>();
            loginWindow.Show();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to show login window");
            MessageBox.Show(
                $"Không thể mở cửa sổ đăng nhập:\n{ex.Message}",
                "Lỗi",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            Shutdown();
        }

        base.OnStartup(e);

    }

    protected override async void OnExit(ExitEventArgs e)
    {
        _logger?.LogInformation("Application shutting down...");

        using (_host)
        {
            await _host.StopAsync(TimeSpan.FromSeconds(5));
        }

        base.OnExit(e);
    }

    // Provides access to the service provider
    public static IServiceProvider Services => ((App)Current)._host.Services;

    // Global exception handler
    private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        _logger?.LogError(e.Exception, "Unhandled UI exception");

        MessageBox.Show(
            $"Có lỗi xảy ra:\n{e.Exception.Message}\n\n" +
            "Ứng dụng sẽ tiếp tục chạy nhưng có thể không ổn định.",
            "Lỗi",
            MessageBoxButton.OK,
            MessageBoxImage.Warning);

        // Mark as handled to prevent app from crashing
        e.Handled = true;
    }

    // Global exception handler for non-UI threads
    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            _logger?.LogCritical(ex, "Unhandled non-UI exception");

            // Run on UI thread
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show(
                    $"Lỗi nghiêm trọng:\n{ex.Message}\n\n" +
                    "Ứng dụng sẽ đóng.",
                    "Lỗi Nghiêm Trọng",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            });
        }
    }

    // Helper method to check database connectivity
    public static async Task<bool> CheckDatabaseConnectivity()
    {
        try
        {
            var dbInitializer = Services.GetRequiredService<SqlDatabaseInitializer>();
            return await dbInitializer.InitializeDatabaseAsync();
        }
        catch
        {
            return false;
        }
    }
}

