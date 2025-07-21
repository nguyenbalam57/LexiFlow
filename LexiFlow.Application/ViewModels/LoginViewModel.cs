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
    /// <summary>
    /// ViewModel cho màn hình đăng nhập
    /// </summary>
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IAuthService _authService;
        private readonly IAppSettingsService _appSettings;
        private readonly ILogger<LoginViewModel> _logger;

        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;
        private string _successMessage = string.Empty;
        private bool _isLoading;
        private bool _rememberMe;
        private string _selectedLanguage = "VN";

        /// <summary>
        /// Tên đăng nhập
        /// </summary>
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
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        /// <summary>
        /// Mật khẩu
        /// </summary>
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
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        /// <summary>
        /// Thông báo lỗi
        /// </summary>
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

        /// <summary>
        /// Thông báo thành công
        /// </summary>
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

        /// <summary>
        /// Trạng thái đang tải
        /// </summary>
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
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        /// <summary>
        /// Trạng thái không đang tải
        /// </summary>
        public bool IsNotLoading => !IsLoading;

        /// <summary>
        /// Ghi nhớ đăng nhập
        /// </summary>
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

        /// <summary>
        /// Ngôn ngữ đã chọn
        /// </summary>
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;
                    OnPropertyChanged();
                    _appSettings.CurrentLanguage = value;
                    _appSettings.SaveSettings();

                    // Thông báo ngôn ngữ đã thay đổi
                    LanguageChanged?.Invoke(this, value);
                }
            }
        }

        /// <summary>
        /// Lệnh đăng nhập
        /// </summary>
        public ICommand LoginCommand { get; }

        /// <summary>
        /// Lệnh thay đổi ngôn ngữ
        /// </summary>
        public ICommand ChangeLanguageCommand { get; }

        /// <summary>
        /// Lệnh quên mật khẩu
        /// </summary>
        public ICommand ForgotPasswordCommand { get; }

        /// <summary>
        /// Lệnh đăng ký
        /// </summary>
        public ICommand RegisterCommand { get; }

        /// <summary>
        /// Sự kiện đăng nhập thành công
        /// </summary>
        public event EventHandler LoginSuccessful;

        /// <summary>
        /// Sự kiện ngôn ngữ thay đổi
        /// </summary>
        public event EventHandler<string> LanguageChanged;

        /// <summary>
        /// Sự kiện thông báo thuộc tính thay đổi
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Khởi tạo ViewModel đăng nhập
        /// </summary>
        public LoginViewModel(IAuthService authService, IAppSettingsService appSettings, ILogger<LoginViewModel> logger = null)
        {
            _authService = authService;
            _appSettings = appSettings;
            _logger = logger;

            // Khởi tạo các lệnh
            LoginCommand = new RelayCommand(LoginAsync, CanLogin);
            ChangeLanguageCommand = new RelayCommand<string>(ChangeLanguage);
            ForgotPasswordCommand = new RelayCommand(_ => ShowForgotPassword());
            RegisterCommand = new RelayCommand(_ => ShowRegister());

            // Tải cài đặt đã lưu
            LoadSavedSettings();
        }

        /// <summary>
        /// Kiểm tra có thể đăng nhập không
        /// </summary>
        private bool CanLogin(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !IsLoading &&
                   Username.Length >= 3 &&
                   Password.Length >= 6;
        }

        /// <summary>
        /// Xử lý đăng nhập
        /// </summary>
        private async void LoginAsync(object parameter)
        {
            try
            {
                IsLoading = true;
                ClearMessages();

                // Kiểm tra dữ liệu đầu vào
                if (!ValidateInput())
                    return;

                // Hiển thị thông báo đang xử lý
                SuccessMessage = GetLocalizedString("Login_Connecting");

                // Tạo độ trễ giả lập để UX tốt hơn
                await Task.Delay(800);

                // Xác thực người dùng
                var result = await _authService.AuthenticateAsync(Username.Trim(), Password);

                if (result.Success)
                {
                    SuccessMessage = GetLocalizedString("Login_Success");

                    // Lưu thông tin đăng nhập nếu được chọn
                    if (RememberMe)
                    {
                        SaveLoginCredentials();
                    }
                    else
                    {
                        ClearSavedCredentials();
                    }

                    // Giải phóng password khỏi bộ nhớ
                    Password = string.Empty;

                    // Tạo độ trễ ngắn để hiển thị thông báo thành công
                    await Task.Delay(500);

                    // Thông báo đăng nhập thành công
                    LoginSuccessful?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    ErrorMessage = result.Message ?? GetLocalizedString("Login_InvalidCredentials");

                    // Tăng số lần đăng nhập thất bại
                    IncrementFailedLoginAttempts();

                    // Xóa mật khẩu khi đăng nhập thất bại
                    Password = string.Empty;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Lỗi trong quá trình đăng nhập");
                ErrorMessage = string.Format(GetLocalizedString("Login_GeneralError"), ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Kiểm tra dữ liệu đầu vào
        /// </summary>
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

            // Kiểm tra ký tự không hợp lệ trong tên đăng nhập
            if (Username.Contains(" ") || Username.Contains("'") || Username.Contains("\""))
            {
                ErrorMessage = GetLocalizedString("Login_InvalidUsername");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Thay đổi ngôn ngữ
        /// </summary>
        private void ChangeLanguage(string language)
        {
            if (!string.IsNullOrEmpty(language))
            {
                SelectedLanguage = language;
            }
        }

        /// <summary>
        /// Xóa các thông báo
        /// </summary>
        private void ClearMessages()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
        }

        /// <summary>
        /// Hiển thị hộp thoại quên mật khẩu
        /// </summary>
        private void ShowForgotPassword()
        {
            // Thực hiện hiển thị thông báo hoặc hộp thoại quên mật khẩu
            // Được triển khai trong lớp View
        }

        /// <summary>
        /// Hiển thị hộp thoại đăng ký
        /// </summary>
        private void ShowRegister()
        {
            // Thực hiện hiển thị thông báo hoặc hộp thoại đăng ký
            // Được triển khai trong lớp View
        }

        /// <summary>
        /// Tải cài đặt đã lưu
        /// </summary>
        private void LoadSavedSettings()
        {
            try
            {
                // Tải ngôn ngữ
                SelectedLanguage = _appSettings.CurrentLanguage ?? "VN";

                // Tải thông tin đăng nhập đã lưu
                if (_appSettings.RememberLogin && !string.IsNullOrEmpty(_appSettings.SavedUsername))
                {
                    Username = _appSettings.SavedUsername;
                    RememberMe = true;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Lỗi khi tải cài đặt đã lưu");
            }
        }

        /// <summary>
        /// Lưu thông tin đăng nhập
        /// </summary>
        private void SaveLoginCredentials()
        {
            try
            {
                _appSettings.RememberLogin = true;
                _appSettings.SavedUsername = Username;
                _appSettings.SaveSettings();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Lỗi khi lưu thông tin đăng nhập");
            }
        }

        /// <summary>
        /// Xóa thông tin đăng nhập đã lưu
        /// </summary>
        private void ClearSavedCredentials()
        {
            try
            {
                _appSettings.RememberLogin = false;
                _appSettings.SavedUsername = string.Empty;
                _appSettings.SaveSettings();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Lỗi khi xóa thông tin đăng nhập đã lưu");
            }
        }

        /// <summary>
        /// Tăng số lần đăng nhập thất bại
        /// </summary>
        private void IncrementFailedLoginAttempts()
        {
            try
            {
                // TODO: Triển khai tính năng khóa tài khoản tạm thời sau nhiều lần đăng nhập thất bại
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Lỗi khi cập nhật số lần đăng nhập thất bại");
            }
        }

        /// <summary>
        /// Lấy chuỗi đa ngôn ngữ
        /// </summary>
        private string GetLocalizedString(string key)
        {
            // Được triển khai trong lớp View
            return key;
        }

        /// <summary>
        /// Thông báo thuộc tính thay đổi
        /// </summary>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Lớp lệnh tổng quát
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
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

    /// <summary>
    /// Lớp lệnh tổng quát có tham số
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
