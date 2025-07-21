using System;

namespace LexiFlow.Core.Interfaces
{
    /// <summary>
    /// Interface cho dịch vụ quản lý cài đặt ứng dụng
    /// </summary>
    public interface IAppSettingsService
    {
        /// <summary>
        /// Đường dẫn đến API
        /// </summary>
        string ApiUrl { get; set; }

        /// <summary>
        /// Chuỗi kết nối đến cơ sở dữ liệu cục bộ
        /// </summary>
        string LocalDbConnectionString { get; set; }

        /// <summary>
        /// Ghi nhớ đăng nhập
        /// </summary>
        bool RememberLogin { get; set; }

        /// <summary>
        /// Tên đăng nhập đã lưu
        /// </summary>
        string SavedUsername { get; set; }

        /// <summary>
        /// Tự động đăng nhập
        /// </summary>
        bool AutoLogin { get; set; }

        /// <summary>
        /// Ngôn ngữ đã chọn
        /// </summary>
        string SelectedLanguage { get; set; }

        /// <summary>
        /// Token truy cập API
        /// </summary>
        string AccessToken { get; set; }

        /// <summary>
        /// Lưu cài đặt
        /// </summary>
        void SaveSettings();

        /// <summary>
        /// Tải cài đặt
        /// </summary>
        void LoadSettings();
    }
}