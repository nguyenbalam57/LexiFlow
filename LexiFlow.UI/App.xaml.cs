using LexiFlow.Application;
using LexiFlow.Core.Interfaces;
using LexiFlow.Infrastructure;
using LexiFlow.UI.Helpers;
using LexiFlow.UI.Services;
using LexiFlow.UI.Views.Login;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                                                ""DefaultLanguage"": ""VN""
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

        // Initialize the database using the SQL script
        try
        {
            await Infrastructure.DependencyInjection.InitializeDatabaseAsync(_host.Services);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Không thể khởi tạo cơ sở dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
            return;
        }

        // Show the login window first
        var loginWindow = _host.Services.GetRequiredService<LoginView>();
        loginWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync();
        }

        base.OnExit(e);
    }

    // Provides access to the service provider
    public static IServiceProvider Services => ((App)Current)._host.Services;

    // Global exception handler
    private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        MessageBox.Show($"Có lỗi xảy ra: {e.Exception.Message}", "Lỗi",
            MessageBoxButton.OK, MessageBoxImage.Error);

        // Log the exception
        System.Diagnostics.Debug.WriteLine($"Unhandled exception: {e.Exception}");

        // Mark as handled to prevent app from crashing
        e.Handled = true;
    }
}

