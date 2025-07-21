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
        private readonly ILogger<AuthService> _logger;
        private UserDto _currentUser;

        public AuthService(
            IApiService apiService,
            IAppSettingsService appSettings,
            ILogger<AuthService> logger)
        {
            _apiService = apiService;
            _appSettings = appSettings;
            _logger = logger;
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
            return _appSettings.AccessToken;
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
