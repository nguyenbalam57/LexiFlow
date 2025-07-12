using LexiFlow.Core.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace LexiFlow.Application.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IAuthService _authService;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _isLoading;
        private bool _rememberMe;
        private string _selectedLanguage = "VN";

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                    // Clear error message when user types
                    if (!string.IsNullOrEmpty(_errorMessage))
                    {
                        ErrorMessage = string.Empty;
                    }
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                    // Clear error message when user types
                    if (!string.IsNullOrEmpty(_errorMessage))
                    {
                        ErrorMessage = string.Empty;
                    }
                }
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool RememberMe
        {
            get => _rememberMe;
            set
            {
                if (_rememberMe != value)
                {
                    _rememberMe = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;
                    OnPropertyChanged();
                    ChangeLanguage();
                }
            }
        }

        public List<string> AvailableLanguages { get; } = new List<string> { "VN", "EN", "JP" };

        public ICommand LoginCommand { get; }
        public ICommand ChangeLanguageCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? LoginSuccessful;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            LoginCommand = new RelayCommand(LoginAsync, CanLogin);
            ChangeLanguageCommand = new RelayCommand<string>(lang => SelectedLanguage = lang ?? "VN");
        }

        private bool CanLogin(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password) && !IsLoading;
        }

        private async void LoginAsync(object? parameter)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                // Simple validation
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Username and password are required";
                    return;
                }

                // Authenticate user
                var user = await _authService.AuthenticateAsync(Username, Password);

                if (user != null)
                {
                    // Save remember me if checked
                    if (RememberMe)
                    {
                        SaveLoginCredentials();
                    }

                    // Notify login successful
                    LoginSuccessful?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    ErrorMessage = "Invalid username or password";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Login failed: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ChangeLanguage()
        {
            // Change application language
            // In a real app, this would use a language service
            switch (SelectedLanguage)
            {
                case "VN":
                    // Switch to Vietnamese
                    UpdateResourceDictionary("VN");
                    break;
                case "EN":
                    // Switch to English
                    UpdateResourceDictionary("EN");
                    break;
                case "JP":
                    // Switch to Japanese
                    UpdateResourceDictionary("JP");
                    break;
            }
        }

        private void UpdateResourceDictionary(string language)
        {
            // This would be implemented in the UI layer to update the app's resources
        }

        private void SaveLoginCredentials()
        {
            // In a real app, this would securely store the username (not password)
            // using something like ProtectedData or a secure storage mechanism
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
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
