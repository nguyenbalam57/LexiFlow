using LexiFlow.Application.Services;
using LexiFlow.Core.Interfaces;
using LexiFlow.Core.Models;
using LexiFlow.Core.Services;
using LexiFlow.Infrastructure.Data;
using LexiFlow.Infrastructure.Repositories;
using LexiFlow.UI.Views.Login;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;

namespace LexiFlow.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public static IConfiguration Configuration { get; private set; }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            // Set default culture
            var selectedLanguage = Properties.Settings.Default.SelectedLanguage;
            if (!string.IsNullOrEmpty(selectedLanguage))
            {
                var culture = new CultureInfo(selectedLanguage);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }

            // Configure services
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Build service provider
            ServiceProvider = serviceCollection.BuildServiceProvider();

            try
            {
                // Khởi tạo cơ sở dữ liệu cục bộ
                var localStorageService = ServiceProvider.GetService<ILocalStorageService>();
                await localStorageService.InitializeDatabaseAsync();

                // Khởi tạo phiên làm việc
                var sessionManager = ServiceProvider.GetService<SessionManager>();
                var sessionInitialized = await sessionManager.InitializeSessionAsync();

                if (sessionInitialized)
                {
                    // Đã khôi phục phiên làm việc, mở màn hình chính
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                }
                else
                {
                    // Cần đăng nhập, mở màn hình đăng nhập
                    var loginWindow = new LoginView();
                    loginWindow.Show();
                }

                // Đăng ký xử lý sự kiện phiên làm việc
                sessionManager.SessionStateChanged += SessionManager_SessionStateChanged;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khởi động
                var logger = ServiceProvider.GetService<ILogger<App>>();
                logger.LogError(ex, "Lỗi khi khởi động ứng dụng");

                MessageBox.Show($"Đã xảy ra lỗi khi khởi động ứng dụng: {ex.Message}",
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);

                // Mở màn hình đăng nhập
                var loginWindow = new LoginView();
                loginWindow.Show();
            }
        }

        private void SessionManager_SessionStateChanged(object sender, SessionStateChangedEventArgs e)
        {
            // Xử lý sự kiện phiên làm việc thay đổi
            switch (e.State)
            {
                case SessionState.Expired:
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Phiên làm việc đã hết hạn. Vui lòng đăng nhập lại.",
                            "Phiên hết hạn", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Mở màn hình đăng nhập
                        var loginWindow = new LoginView();
                        loginWindow.Show();

                        // Đóng các cửa sổ khác
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window is not LoginView)
                            {
                                window.Close();
                            }
                        }
                    });
                    break;

                case SessionState.Error:
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Đã xảy ra lỗi với phiên làm việc. Vui lòng đăng nhập lại.",
                            "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                    break;
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Add configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            // Add logging
            services.AddLogging(configure =>
            {
                configure.AddConsole();
                configure.AddDebug();
            });

            // Add DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            // Add repositories
            services.AddScoped<IUserRepository, UserRepository>();

            // Add infrastructure services
            services.AddSingleton<SqlEntityAdapter>();

            // Add application services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IApiService, ApiService>();
            services.AddSingleton<IAppSettingsService, AppSettingsService>();
            services.AddScoped<ILocalStorageService, LocalStorageService>();
            services.AddScoped<IVocabularyService, VocabularyService>();

            // Add token and session management
            services.AddSingleton<TokenManager>();
            services.AddSingleton<SessionManager>();

            // Configure AppSettings service
            var appSettings = new AppSettings
            {
                ApiUrl = Configuration["Api:BaseUrl"],
                RememberLogin = Properties.Settings.Default.RememberMe,
                SavedUsername = Properties.Settings.Default.SavedUsername,
                AutoLogin = Properties.Settings.Default.AutoLogin,
                AccessToken = Properties.Settings.Default.AccessToken
            };

            services.AddSingleton(appSettings);
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Log unhandled exceptions
            var logger = ServiceProvider?.GetService<ILogger<App>>();
            logger?.LogError(e.Exception, "Lỗi không được xử lý trong ứng dụng");

            // Show user-friendly error message
            MessageBox.Show($"Đã xảy ra lỗi không mong muốn: {e.Exception.Message}\n\nVui lòng khởi động lại ứng dụng.",
                "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);

            // Mark as handled to prevent application crash
            e.Handled = true;
        }
    }
}