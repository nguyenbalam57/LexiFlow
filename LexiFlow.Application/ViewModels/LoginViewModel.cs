using LexiFlow.Core.Interfaces;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
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
                    ClearMessages();
                    ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
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
                    ClearMessages();
                    ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
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

        public string SuccessMessage
        {
            get => _successMessage;
            set
            {
                if (_successMessage != value)
                {
                    _successMessage = value;
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
                    OnPropertyChanged(nameof(IsNotLoading));
                    ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsNotLoading => !IsLoading;

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

        public string SuccessMessage
        {
            get => _successMessage;
            set
            {
                if (_successMessage != value)
                {
                    _successMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _successMessage = string.Empty;

        private void ClearMessages()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
        }

        public List<string> AvailableLanguages { get; } = new List<string> { "VN", "EN", "JP" };

        public ICommand LoginCommand { get; }
        public ICommand ChangeLanguageCommand { get; }
        public ICommand ForgotPasswordCommand { get; }
        public ICommand RegisterCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? LoginSuccessful;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            LoginCommand = new RelayCommand(LoginAsync, CanLogin);
            ChangeLanguageCommand = new RelayCommand<string>(ChangeLanguageExecute);
            ForgotPasswordCommand = new RelayCommand(_ => ShowForgotPassword());
            RegisterCommand = new RelayCommand(_ => ShowRegister());

            // Load saved login credentials if remember me was checked
            LoadSavedCredentials();
        }

        private bool CanLogin(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !IsLoading &&
                   Username.Length >= 3 &&
                   Password.Length >= 6;
        }

        private async void LoginAsync(object? parameter)
        {
            try
            {
                IsLoading = true;
                ClearMessages();

                // Validate input
                if (!ValidateInput())
                    return;

                // Show progress message
                SuccessMessage = GetLocalizedString("Login_Connecting");

                // Simulate network delay for better UX
                await Task.Delay(800);

                // Authenticate user
                var user = await _authService.AuthenticateAsync(Username.Trim(), Password);

                if (user != null)
                {
                    SuccessMessage = GetLocalizedString("Login_Success");

                    // Save credentials if remember me is checked
                    if (RememberMe)
                    {
                        SaveLoginCredentials();
                    }
                    else
                    {
                        ClearSavedCredentials();
                    }

                    // Short delay to show success message
                    await Task.Delay(500);

                    // Notify login successful
                    LoginSuccessful?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    ErrorMessage = GetLocalizedString("Login_InvalidCredentials");

                    // Clear password on failed login
                    Password = string.Empty;

                    // Focus back to username field
                    await Task.Delay(100);
                }
            }
            catch (SqlException sqlEx)
            {
                ErrorMessage = GetLocalizedString("Login_DatabaseError");
                System.Diagnostics.Debug.WriteLine($"Database error: {sqlEx.Message}");
            }
            catch (UnauthorizedAccessException)
            {
                ErrorMessage = GetLocalizedString("Login_AccessDenied");
            }
            catch (TimeoutException)
            {
                ErrorMessage = GetLocalizedString("Login_Timeout");
            }
            catch (Exception ex)
            {
                ErrorMessage = string.Format(GetLocalizedString("Login_GeneralError"), ex.Message);
                System.Diagnostics.Debug.WriteLine($"Login error: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = GetLocalizedString("Login_UsernameRequired");
                return false;
            }

            if (Username.Length < 3)
            {
                ErrorMessage = GetLocalizedString("Login_UsernameMinLength");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = GetLocalizedString("Login_PasswordRequired");
                return false;
            }

            if (Password.Length < 6)
            {
                ErrorMessage = GetLocalizedString("Login_PasswordMinLength");
                return false;
            }

            // Check for common invalid characters
            if (Username.Contains(" ") || Username.Contains("'") || Username.Contains("\""))
            {
                ErrorMessage = GetLocalizedString("Login_InvalidUsername");
                return false;
            }

            return true;
        }

        private void ChangeLanguageExecute(string? language)
        {
            if (!string.IsNullOrEmpty(language) && AvailableLanguages.Contains(language))
            {
                SelectedLanguage = language;
            }
        }

        private void ChangeLanguage()
        {
            try
            {
                // Clear any existing messages since they might be in the old language
                ClearMessages();

                // Change application language
                var app = Application.Current;
                if (app?.Resources != null)
                {
                    var resourceDictionaries = app.Resources.MergedDictionaries;

                    // Find and remove existing language dictionary
                    var existingLangDict = resourceDictionaries
                        .FirstOrDefault(rd => rd.Source?.OriginalString?.Contains("Languages_") == true);

                    if (existingLangDict != null)
                    {
                        resourceDictionaries.Remove(existingLangDict);
                    }

                    // Add new language dictionary
                    var newLangDict = new ResourceDictionary
                    {
                        Source = new Uri($"/LexiFlow.UI;component/Resources/Languages/Languages_{SelectedLanguage}.xaml",
                                       UriKind.RelativeOrAbsolute)
                    };

                    resourceDictionaries.Add(newLangDict);

                    // Save language preference
                    SaveLanguagePreference();

                    // Show language changed message
                    SuccessMessage = GetLocalizedString("Login_LanguageChanged");

                    // Clear the message after a short delay
                    Task.Delay(2000).ContinueWith(_ =>
                    {
                        Application.Current.Dispatcher.Invoke(() => SuccessMessage = string.Empty);
                    });
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = GetLocalizedString("Login_LanguageChangeError");
                System.Diagnostics.Debug.WriteLine($"Language change error: {ex.Message}");
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

        private void ShowForgotPassword()
        {
            var message = GetLocalizedString("Login_ForgotPasswordMessage");
            var title = GetLocalizedString("Login_ForgotPasswordTitle");
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowRegister()
        {
            var message = GetLocalizedString("Login_RegisterMessage");
            var title = GetLocalizedString("Login_RegisterTitle");
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LoadSavedCredentials()
        {
            try
            {
                var settings = Properties.Settings.Default;
                if (settings.RememberMe && !string.IsNullOrEmpty(settings.SavedUsername))
                {
                    Username = settings.SavedUsername;
                    RememberMe = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading saved credentials: {ex.Message}");
            }
        }

        private void SaveLoginCredentials()
        {
            try
            {
                var settings = Properties.Settings.Default;
                settings.SavedUsername = RememberMe ? Username : string.Empty;
                settings.RememberMe = RememberMe;
                settings.LastLoginDate = DateTime.Now;
                settings.Save();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving credentials: {ex.Message}");
            }
        }

        private void ClearSavedCredentials()
        {
            try
            {
                var settings = Properties.Settings.Default;
                settings.SavedUsername = string.Empty;
                settings.RememberMe = false;
                settings.Save();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error clearing saved credentials: {ex.Message}");
            }
        }

        private void SaveLanguagePreference()
        {
            try
            {
                Properties.Settings.Default.PreferredLanguage = SelectedLanguage;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving language preference: {ex.Message}");
            }
        }

        private string GetLocalizedString(string key)
        {
            try
            {
                var resource = Application.Current.TryFindResource(key);
                return resource?.ToString() ?? key;
            }
            catch
            {
                return key;
            }
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
