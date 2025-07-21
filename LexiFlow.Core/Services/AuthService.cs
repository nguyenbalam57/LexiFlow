using LexiFlow.Core.Entities;
using LexiFlow.Core.Interfaces;
using LexiFlow.Core.Models;
using LexiFlow.Core.Models.Responses;
using Microsoft.Extensions.Logging;

namespace LexiFlow.Core.Services
{
    /// <summary>
    /// Dịch vụ xác thực
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IApiService _apiService;
        private readonly IAppSettingsService _appSettings;
        private readonly IUserRepository _userRepository; // Thêm repository người dùng
        private readonly ILogger<AuthService> _logger;
        private UserDto _currentUser;

        public AuthService(
            IApiService apiService,
            IAppSettingsService appSettings,
            IUserRepository userRepository, // Thêm vào constructor
            ILogger<AuthService> logger)
        {
            _apiService = apiService;
            _appSettings = appSettings;
            _userRepository = userRepository; // Khởi tạo repository
            _logger = logger;
        }

        public async Task<User> ValidateUserAsync(string username, string password)
        {
            try
            {
                // Triển khai xác thực người dùng
                var user = await _userRepository.GetUserByUsernameAsync(username);
                if (user != null && VerifyPassword(password, user.PasswordHash))
                {
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi xác thực người dùng '{username}'");
                return null;
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                // Triển khai lấy thông tin người dùng theo ID
                return await _userRepository.GetUserByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy thông tin người dùng ID {id}");
                return null;
            }
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            try
            {
                // Triển khai xác minh mật khẩu
                return BCrypt.Net.BCrypt.Verify(password, passwordHash);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xác minh mật khẩu");
                return false;
            }
        }

        /// <summary>
        /// Xác thực người dùng
        /// </summary>
        public async Task<ServiceResult<LoginResponse>> AuthenticateAsync(string username, string password)
        {
            try
            {
                var response = await _apiService.LoginAsync(username, password);

                if (response.SuccessResult && response.Data != null)
                {
                    // Lưu thông tin người dùng hiện tại
                    _currentUser = response.Data.User;

                    // Lưu thông tin đăng nhập nếu cần
                    if (_appSettings.RememberLogin)
                    {
                        _appSettings.SavedUsername = username;
                    }

                    return ServiceResult<LoginResponse>.Success(response.Data);
                }
                else
                {
                    return ServiceResult<LoginResponse>.Fail(response.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi trong quá trình xác thực");
                return ServiceResult<LoginResponse>.Fail($"Lỗi xác thực: {ex.Message}");
            }
        }

        /// <summary>
        /// Đăng xuất
        /// </summary>
        public void Logout()
        {
            _apiService.ClearAccessToken();
            _currentUser = null;

            // Xóa thông tin tự động đăng nhập
            _appSettings.AutoLogin = false;
            _appSettings.SaveSettings();
        }

        /// <summary>
        /// Kiểm tra xem người dùng đã đăng nhập chưa
        /// </summary>
        public bool IsAuthenticated()
        {
            return _currentUser != null && !string.IsNullOrEmpty(_apiService.GetCurrentToken());
        }

        /// <summary>
        /// Làm mới token
        /// </summary>
        public async Task<ServiceResult<string>> RefreshTokenAsync()
        {
            try
            {
                var response = await _apiService.RefreshTokenAsync();

                if (response.SuccessResult && !string.IsNullOrEmpty(response.Data))
                {
                    return ServiceResult<string>.Success(response.Data);
                }
                else
                {
                    // Xóa thông tin đăng nhập nếu token không thể làm mới
                    if (response.SessionExpired)
                    {
                        Logout();
                    }

                    return ServiceResult<string>.Fail(response.Message, response.SessionExpired);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi trong quá trình làm mới token");
                return ServiceResult<string>.Fail($"Lỗi làm mới token: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy token hiện tại
        /// </summary>
        public string GetCurrentToken()
        {
            return _apiService.GetCurrentToken();
        }

        /// <summary>
        /// Lấy thông tin người dùng hiện tại
        /// </summary>
        public UserDto GetCurrentUser()
        {
            return _currentUser;
        }
    }
}