using LexiFlow.Core.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System;
using System.Threading.Tasks;


namespace LexiFlow.Application.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IAuthService _authService;
        private readonly ILogger<LoginViewModel> _logger;

        // Properties
        private string _username = string.Empty;
        private string _password = string.Empty;
        private bool _rememberMe;
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

            _logger = App.ServiceProvider.GetService(typeof(ILogger<LoginViewModel>)) as ILogger<LoginViewModel>
                ?? throw new InvalidOperationException("ILogger<LoginViewModel> not found in service provider");

            // Initialize commands
            LoginCommand = new RelayCommand(async _ => await LoginAsync());
            ForgotPasswordCommand = new RelayCommand(_ => NavigateToForgotPassword());
            SignUpCommand = new RelayCommand(_ => NavigateToSignUp());
        }

        private async Task LoginAsync()
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Please enter both username and password";
                    return;
                }

                // Set loading state
                IsLoading = true;
                ErrorMessage = string.Empty;

                // Attempt to authenticate
                var result = await _authService.AuthenticateAsync(Username, Password);

                if (result.Success)
                {
                    _logger.LogInformation("User {Username} logged in successfully", Username);

                    // Save credentials if remember me is checked
                    SaveCredentials();

                    // Navigate to main application
                    OpenMainWindow();
                }
                else
                {
                    // Show error message
                    ErrorMessage = result.Message ?? "Login failed. Please check your credentials.";
                    _logger.LogWarning("Login failed for user {Username}: {Message}", Username, result.Message);
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                ErrorMessage = "An error occurred during login. Please try again.";
                _logger.LogError(ex, "Login error for user {Username}", Username);
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
            Properties.Settings.Default.RememberMe = RememberMe;
            Properties.Settings.Default.SavedUsername = RememberMe ? Username : string.Empty;
            Properties.Settings.Default.Save();
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
                    if (window is LoginView)
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
            MessageBox.Show("This feature is not implemented yet.", "Forgot Password",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void NavigateToSignUp()
        {
            // Implementation for sign up navigation
            MessageBox.Show("This feature is not implemented yet.", "Sign Up",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Simple implementation of ICommand for the ViewModel
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T?> _execute;
        private readonly Func<T?, bool>? _canExecute;

        public RelayCommand(Action<T?> execute, Func<T?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute((T?)parameter);
        }

        public void Execute(object? parameter)
        {
            _execute((T?)parameter);
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
