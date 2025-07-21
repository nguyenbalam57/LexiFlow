using LexiFlow.Core.Interfaces;
using LexiFlow.Core.Models;
using LexiFlow.Core.Models.Responses;
using LexiFlow.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace LexiFlow.App.ViewModels
{
    /// <summary>
    /// ViewModel cho màn hình đăng nhập
    /// </summary>
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IAuthService _authService;
        private readonly IApiService _apiService;
        private readonly IAppSettingsService _appSettings;
        private readonly TokenManager _tokenManager;
        private readonly SessionManager _sessionManager;
        private readonly ILogger<LoginViewModel> _logger;

        // Properties
        private string _username = string.Empty;
        private string _password = string.Empty;
        private bool _rememberMe;
        private bool _autoLogin;
        private bool _isLoading;
        private string _errorMessage = string.Empty;
        private bool _hasError;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public bool RememberMe
        {
            get => _rememberMe;
            set
            {
                _rememberMe = value;
                OnPropertyChanged();

                // Nếu bỏ chọn RememberMe, cũng bỏ chọn AutoLogin
                if (!value && AutoLogin)
                {
                    AutoLogin = false;
                }
            }
        }

        public bool AutoLogin
        {
            get => _autoLogin;
            set
            {
                _autoLogin = value;
                OnPropertyChanged();

                // Nếu chọn AutoLogin, cũng chọn RememberMe
                if (value && !RememberMe)
                {
                    RememberMe = true;
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                HasError = !string.IsNullOrEmpty(value);
                OnPropertyChanged();
            }
        }

        public bool HasError
        {
            get => _hasError;
            set
            {
                _hasError = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand LoginCommand { get; }
        public ICommand ForgotPasswordCommand { get; }
        public ICommand SignUpCommand { get; }

        // Constructor
        public LoginViewModel()
        {
            // Get services from dependency injection
            _authService = App.ServiceProvider.GetService(typeof(IAuthService)) as IAuthService
                ?? throw new InvalidOperationException("IAuthService not found in service provider");

            _apiService = App.ServiceProvider.GetService(typeof(IApiService)) as IApiService
                ?? throw new InvalidOperationException("IApiService not found in service provider");

            _appSettings = App.ServiceProvider.GetService(typeof(IAppSettingsService)) as IAppSettingsService
                ?? throw new InvalidOperationException("IAppSettingsService not found in service provider");

            _tokenManager = App.ServiceProvider.GetService(typeof(TokenManager)) as TokenManager
                ?? throw new InvalidOperationException("TokenManager not found in service provider");

            _sessionManager = App.ServiceProvider.GetService(typeof(SessionManager)) as SessionManager
                ?? throw new InvalidOperationException("SessionManager not found in service provider");

            _logger = App.ServiceProvider.GetService(typeof(ILogger<LoginViewModel>)) as ILogger<LoginViewModel>
                ?? throw new InvalidOperationException("ILogger<LoginViewModel> not found in service provider");

            // Initialize commands
            LoginCommand = new RelayCommand(async _ => await LoginAsync());
            ForgotPasswordCommand = new RelayCommand(_ => NavigateToForgotPassword());
            SignUpCommand = new RelayCommand(_ => NavigateToSignUp());

            // Load saved username if remember me is checked
            LoadSavedCredentials();
        }

        private void LoadSavedCredentials()
        {
            if (_appSettings.RememberLogin)
            {
                Username = _appSettings.SavedUsername;
                RememberMe = true;
                AutoLogin = _appSettings.AutoLogin;
            }
        }

        private async Task LoginAsync()
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Vui lòng nhập cả tên đăng nhập và mật khẩu";
                    return;
                }

                // Set loading state
                IsLoading = true;
                ErrorMessage = string.Empty;

                // Attempt to authenticate
                var result = await _apiService.LoginAsync(Username, Password);

                if (result.SuccessResult && result.Data != null)
                {
                    _logger.LogInformation("Đăng nhập thành công cho người dùng {Username}", Username);

                    // Save credentials if remember me is checked
                    SaveCredentials();

                    // Navigate to main application
                    OpenMainWindow();
                }
                else
                {
                    // Show error message
                    ErrorMessage = result.Message ?? "Đăng nhập thất bại. Vui lòng kiểm tra thông tin đăng nhập.";
                    _logger.LogWarning("Đăng nhập thất bại cho người dùng {Username}: {Message}", Username, result.Message);
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                ErrorMessage = "Đã xảy ra lỗi khi đăng nhập. Vui lòng thử lại.";
                _logger.LogError(ex, "Lỗi đăng nhập cho người dùng {Username}", Username);
            }
            finally
            {
                // Reset loading state
                IsLoading = false;
            }
        }

        private void SaveCredentials()
        {
            // Save username if remember me is checked
            _appSettings.RememberLogin = RememberMe;
            _appSettings.SavedUsername = RememberMe ? Username : string.Empty;
            _appSettings.AutoLogin = AutoLogin;
            _appSettings.SaveSettings();
        }

        private void OpenMainWindow()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Create and show main window
                var mainWindow = new MainWindow();
                mainWindow.Show();

                // Close login window
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is Views.Login.LoginView)
                    {
                        window.Close();
                        break;
                    }
                }
            });
        }

        private void NavigateToForgotPassword()
        {
            // Implementation for forgot password navigation
            MessageBox.Show("Tính năng này chưa được triển khai.", "Quên mật khẩu",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void NavigateToSignUp()
        {
            // Implementation for sign up navigation
            MessageBox.Show("Tính năng này chưa được triển khai.", "Đăng ký",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Simple relay command implementation
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}