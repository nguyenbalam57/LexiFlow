using LexiFlow.Core.Interfaces;
using LexiFlow.Core.Models;
using System;

namespace LexiFlow.App.Services
{
    /// <summary>
    /// Triển khai dịch vụ quản lý cài đặt ứng dụng
    /// </summary>
    public class AppSettingsService : IAppSettingsService
    {
        private readonly AppSettings _appSettings;

        public AppSettingsService(AppSettings appSettings)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            LoadSettings();
        }

        /// <summary>
        /// Đường dẫn đến API
        /// </summary>
        public string ApiUrl
        {
            get => _appSettings.ApiUrl;
            set => _appSettings.ApiUrl = value;
        }

        /// <summary>
        /// Chuỗi kết nối đến cơ sở dữ liệu cục bộ
        /// </summary>
        public string LocalDbConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Ghi nhớ đăng nhập
        /// </summary>
        public bool RememberLogin
        {
            get => Properties.Settings.Default.RememberMe;
            set => Properties.Settings.Default.RememberMe = value;
        }

        /// <summary>
        /// Tên đăng nhập đã lưu
        /// </summary>
        public string SavedUsername
        {
            get => Properties.Settings.Default.SavedUsername;
            set => Properties.Settings.Default.SavedUsername = value;
        }

        /// <summary>
        /// Tự động đăng nhập
        /// </summary>
        public bool AutoLogin
        {
            get => Properties.Settings.Default.AutoLogin;
            set => Properties.Settings.Default.AutoLogin = value;
        }

        /// <summary>
        /// Ngôn ngữ đã chọn
        /// </summary>
        public string SelectedLanguage
        {
            get => Properties.Settings.Default.SelectedLanguage;
            set => Properties.Settings.Default.SelectedLanguage = value;
        }

        /// <summary>
        /// Token truy cập API
        /// </summary>
        public string AccessToken
        {
            get => Properties.Settings.Default.AccessToken;
            set => Properties.Settings.Default.AccessToken = value;
        }

        /// <summary>
        /// Lưu cài đặt
        /// </summary>
        public void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Tải cài đặt
        /// </summary>
        public void LoadSettings()
        {
            // Nạp cài đặt từ Properties.Settings.Default vào _appSettings
            _appSettings.RememberLogin = Properties.Settings.Default.RememberMe;
            _appSettings.SavedUsername = Properties.Settings.Default.SavedUsername;
            _appSettings.AutoLogin = Properties.Settings.Default.AutoLogin;
            _appSettings.AccessToken = Properties.Settings.Default.AccessToken;
        }
    }
}