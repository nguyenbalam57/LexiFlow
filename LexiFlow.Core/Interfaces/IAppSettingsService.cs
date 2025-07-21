using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Core.Interfaces
{
    /// <summary>
    /// Interface cho dịch vụ cài đặt ứng dụng
    /// </summary>
    public interface IAppSettingsService
    {
        /// <summary>
        /// URL cơ sở của API
        /// </summary>
        string ApiBaseUrl { get; set; }

        /// <summary>
        /// Token truy cập hiện tại
        /// </summary>
        string AccessToken { get; set; }

        /// <summary>
        /// Chuỗi kết nối cơ sở dữ liệu địa phương
        /// </summary>
        string LocalDbConnectionString { get; }

        /// <summary>
        /// Ngôn ngữ hiện tại
        /// </summary>
        string CurrentLanguage { get; set; }

        /// <summary>
        /// Bật/tắt ghi nhớ đăng nhập
        /// </summary>
        bool RememberLogin { get; set; }

        /// <summary>
        /// Tên người dùng đã lưu
        /// </summary>
        string SavedUsername { get; set; }

        /// <summary>
        /// Bật/tắt tự động đăng nhập
        /// </summary>
        bool AutoLogin { get; set; }

        /// <summary>
        /// Lưu cài đặt
        /// </summary>
        void SaveSettings();

        /// <summary>
        /// Tải cài đặt
        /// </summary>
        void LoadSettings();

        /// <summary>
        /// Đặt lại cài đặt về mặc định
        /// </summary>
        void ResetSettings();
    }
}
