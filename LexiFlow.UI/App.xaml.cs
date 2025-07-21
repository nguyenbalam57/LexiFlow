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

        private void Application_Startup(object sender, StartupEventArgs e)
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

            // Start application with login window
            var loginWindow = new LoginView();
            loginWindow.Show();
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

            // Configure AppSettings service
            var appSettings = new AppSettings
            {
                ApiUrl = Configuration["Api:BaseUrl"],
                RememberLogin = Properties.Settings.Default.RememberMe,
                SavedUsername = Properties.Settings.Default.SavedUsername,
                AutoLogin = false
            };

            services.AddSingleton(appSettings);
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Log unhandled exceptions
            var logger = ServiceProvider?.GetService<ILogger<App>>();
            logger?.LogError(e.Exception, "Unhandled application exception");

            // Show user-friendly error message
            MessageBox.Show($"An unexpected error occurred: {e.Exception.Message}\n\nPlease restart the application.",
                "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            // Mark as handled to prevent application crash
            e.Handled = true;
        }
    }
}

