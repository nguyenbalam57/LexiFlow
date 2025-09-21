using LexiFlow.AdminDashboard.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OxyPlot.Utilities;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LexiFlow.AdminDashboard.ViewModels.LoginViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IApiClient _apiClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginViewModel> _logger;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private bool _rememberMe = false;
        private bool _isLoading = false;
        private string _errorMessage = string.Empty;

        public LoginViewModel(
            IApiClient apiClient,
            IConfiguration configuration,
            ILogger<LoginViewModel> logger)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            LoginCommand = new RelayCommand(async () => await LoginAsync(), () => CanLogin());
            ExitCommand = new RelayCommand(() => Application.Current.Shutdown());
            TestConnectionCommand = new RelayCommand(async () => await TestConnectionAsync());

            // Load saved credentials if remember me was checked
            LoadSavedCredentials();
        }

        #region Properties

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
                ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
            }
        }

        public bool RememberMe
        {
            get => _rememberMe;
            set
            {
                _rememberMe = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
                ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand LoginCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand TestConnectionCommand { get; }

        #endregion

        #region Methods

        private async Task InitialConnectionTestAsync()
        {
            try
            {
                var apiSettings = _configuration.GetSection("ApiSettings").Get<ApiSettings>();
                if (apiSettings != null)
                {
                    var testResult = await Helpers.ApiConnectionHelper.TestApiConnectionAsync(
                        apiSettings.BaseUrl, _logger);

                    if (!testResult.HttpsResult?.Success == true && !testResult.HttpResult?.Success == true)
                    {
                        ErrorMessage = "Cannot connect to API server. Click 'Test Connection' for diagnostics.";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Initial connection test failed");
            }
        }

        private bool CanLogin()
        {
            return !IsLoading && 
                   !string.IsNullOrWhiteSpace(Username) && 
                   !string.IsNullOrWhiteSpace(Password);
        }

        private void LoadSavedCredentials()
        {
            try
            {
                if (Properties.Settings.Default.RememberMe)
                {
                    Username = Properties.Settings.Default.Username ?? "";
                    RememberMe = true;
                    _logger.LogInformation("Loaded saved credentials for user: {Username}", Username);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error loading saved credentials");
            }
        }

        private async Task LoginAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                _logger.LogInformation("Attempting login for user: {Username}", Username);

                var loginRequest = new
                {
                    Username = Username.Trim(),
                    Password = Password,
                    RememberMe = RememberMe
                };

                // Check API connectivity first
                if (!await _apiClient.CheckHealthAsync())
                {
                    ErrorMessage = "Không thể kết nối đến server. Vui lòng kiểm tra kết nối mạng.";
                    return;
                }

                var authResponse = await _apiClient.PostAsync<AuthResponse>("api/auth/login", loginRequest);

                if (authResponse?.Token != null)
                {
                    // Set authentication token for future API calls
                    _apiClient.SetAuthToken(authResponse.Token);
                    
                    // Save credentials if remember me is checked
                    SaveCredentials(authResponse);

                    _logger.LogInformation("User logged in successfully: {Username}", authResponse.User.Username);

                    // Navigate to main window
                    await NavigateToMainWindow();
                }
                else
                {
                    ErrorMessage = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                    _logger.LogWarning("Login failed for user: {Username}", Username);
                }
            }
            catch (ApiException apiEx)
            {
                _logger.LogError(apiEx, "API error during login: {StatusCode}", apiEx.StatusCode);
                
                ErrorMessage = apiEx.StatusCode switch
                {
                    System.Net.HttpStatusCode.Unauthorized => "Tên đăng nhập hoặc mật khẩu không chính xác.",
                    System.Net.HttpStatusCode.Forbidden => "Tài khoản của bạn không có quyền truy cập Admin Dashboard.",
                    System.Net.HttpStatusCode.TooManyRequests => "Quá nhiều lần thử đăng nhập. Vui lòng thử lại sau.",
                    _ => $"Lỗi server: {apiEx.Message}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login");
                ErrorMessage = "Đã xảy ra lỗi không xác định. Vui lòng thử lại.";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void SaveCredentials(AuthResponse authResponse)
        {
            try
            {
                Properties.Settings.Default.AccessToken = authResponse.Token;
                Properties.Settings.Default.RefreshToken = authResponse.RefreshToken;
                Properties.Settings.Default.Username = RememberMe ? authResponse.User.Username : "";
                Properties.Settings.Default.RememberMe = RememberMe;
                Properties.Settings.Default.Save();
                
                _logger.LogDebug("Credentials saved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error saving credentials");
            }
        }

        private async Task NavigateToMainWindow()
        {
            try
            {
                // Get MainWindow from DI container
                var mainWindow = App.ServiceProvider.GetRequiredService<MainWindow>();
                
                // Show main window
                mainWindow.Show();
                
                // Close login window
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window is Views.LoginViews.LoginView loginView)
                        {
                            loginView.Close();
                            break;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error navigating to main window");
                ErrorMessage = "Lỗi khởi tạo ứng dụng. Vui lòng thử lại.";
            }
        }

        private async Task TestConnectionAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                var apiSettings = _configuration.GetSection("ApiSettings").Get<ApiSettings>();
                if (apiSettings == null)
                {
                    ErrorMessage = "API settings not configured";
                    return;
                }

                var testResult = await Helpers.ApiConnectionHelper.TestApiConnectionAsync(
                    apiSettings.BaseUrl, _logger);

                var troubleshootingGuide = Helpers.ApiConnectionHelper.GetConnectionTroubleshootingGuide(testResult);

                System.Windows.MessageBox.Show(
                    troubleshootingGuide,
                    "API Connection Diagnostics",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information);

                if (testResult.HttpsResult?.Success == true || testResult.HttpResult?.Success == true)
                {
                    ErrorMessage = "✅ API connection successful!";
                }
                else
                {
                    ErrorMessage = "❌ Cannot connect to API. Check diagnostics above.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Connection test failed: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    #region DTOs

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public UserProfile User { get; set; } = new();
    }

    public class UserProfile
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    #endregion

    #region RelayCommand

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object? parameter) => _execute();

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion
}
