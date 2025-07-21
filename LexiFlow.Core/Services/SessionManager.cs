using LexiFlow.Core.Interfaces;
using LexiFlow.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LexiFlow.Core.Services
{
    /// <summary>
    /// Lớp quản lý phiên làm việc của người dùng
    /// </summary>
    public class SessionManager
    {
        private readonly ILogger<SessionManager> _logger;
        private readonly IApiService _apiService;
        private readonly TokenManager _tokenManager;
        private readonly IAppSettingsService _appSettings;

        // Sự kiện phiên làm việc thay đổi
        public event EventHandler<SessionStateChangedEventArgs> SessionStateChanged;

        public SessionManager(
            ILogger<SessionManager> logger,
            IApiService apiService,
            TokenManager tokenManager,
            IAppSettingsService appSettings)
        {
            _logger = logger;
            _apiService = apiService;
            _tokenManager = tokenManager;
            _appSettings = appSettings;
        }

        /// <summary>
        /// Khởi tạo phiên làm việc khi ứng dụng khởi động
        /// </summary>
        public async Task<bool> InitializeSessionAsync()
        {
            try
            {
                _logger.LogInformation("Đang khởi tạo phiên làm việc...");

                // Kiểm tra token còn hiệu lực không
                if (_tokenManager.IsTokenValid())
                {
                    _logger.LogInformation("Tìm thấy token còn hiệu lực, thử làm mới...");

                    // Token còn hiệu lực, nhưng nên refresh để đảm bảo
                    var refreshResult = await _apiService.RefreshTokenAsync();
                    if (refreshResult.SuccessResult)
                    {
                        _logger.LogInformation("Đã làm mới token thành công");

                        // Kích hoạt sự kiện phiên làm việc đã khôi phục
                        OnSessionStateChanged(SessionState.Restored);
                        return true;
                    }
                    else
                    {
                        _logger.LogWarning($"Không thể làm mới token: {refreshResult.Message}");
                    }
                }

                // Kiểm tra xem có thể tự động đăng nhập không
                if (_appSettings.AutoLogin && !string.IsNullOrEmpty(_appSettings.SavedUsername))
                {
                    _logger.LogInformation("Thử đăng nhập tự động...");
                    // Chức năng tự động đăng nhập cần được triển khai đầy đủ
                    // Lưu ý: không nên lưu password trong cài đặt

                    // Nếu triển khai, sẽ trả về true nếu thành công
                }

                // Không có token hợp lệ hoặc tự động đăng nhập thất bại
                _logger.LogInformation("Cần đăng nhập thủ công");
                OnSessionStateChanged(SessionState.RequiresLogin);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi khởi tạo phiên làm việc");
                OnSessionStateChanged(SessionState.Error);
                return false;
            }
        }

        /// <summary>
        /// Xử lý phiên làm việc hết hạn
        /// </summary>
        public void HandleExpiredSession()
        {
            _logger.LogWarning("Phiên làm việc đã hết hạn");
            _apiService.ClearAccessToken();
            OnSessionStateChanged(SessionState.Expired);
        }

        /// <summary>
        /// Đăng xuất
        /// </summary>
        public void Logout()
        {
            _logger.LogInformation("Đăng xuất khỏi phiên làm việc");
            _apiService.ClearAccessToken();

            // Xóa thông tin tự động đăng nhập
            _appSettings.AutoLogin = false;
            _appSettings.SaveSettings();

            OnSessionStateChanged(SessionState.LoggedOut);
        }

        /// <summary>
        /// Kích hoạt sự kiện trạng thái phiên làm việc thay đổi
        /// </summary>
        private void OnSessionStateChanged(SessionState state)
        {
            SessionStateChanged?.Invoke(this, new SessionStateChangedEventArgs(state));
        }
    }

    /// <summary>
    /// Enum trạng thái phiên làm việc
    /// </summary>
    public enum SessionState
    {
        LoggedIn,    // Đã đăng nhập
        LoggedOut,   // Đã đăng xuất
        Expired,     // Phiên làm việc hết hạn
        Restored,    // Phiên làm việc đã khôi phục
        RequiresLogin, // Yêu cầu đăng nhập
        Error        // Có lỗi xảy ra
    }

    /// <summary>
    /// EventArgs cho sự kiện phiên làm việc thay đổi
    /// </summary>
    public class SessionStateChangedEventArgs : EventArgs
    {
        public SessionState State { get; }

        public SessionStateChangedEventArgs(SessionState state)
        {
            State = state;
        }
    }
}