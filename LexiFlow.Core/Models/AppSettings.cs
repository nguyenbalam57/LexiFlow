namespace LexiFlow.Core.Models
{
    /// <summary>
    /// Cài đặt ứng dụng
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Đường dẫn đến API
        /// </summary>
        public string ApiUrl { get; set; } = string.Empty;

        /// <summary>
        /// Ghi nhớ đăng nhập
        /// </summary>
        public bool RememberLogin { get; set; }

        /// <summary>
        /// Tên đăng nhập đã lưu
        /// </summary>
        public string SavedUsername { get; set; } = string.Empty;

        /// <summary>
        /// Tự động đăng nhập
        /// </summary>
        public bool AutoLogin { get; set; }

        /// <summary>
        /// Token truy cập API
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;
    }
}