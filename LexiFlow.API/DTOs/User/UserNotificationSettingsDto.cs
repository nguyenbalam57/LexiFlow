namespace LexiFlow.API.DTOs.User
{
    /// <summary>
    /// DTO cho cài đặt thông báo của người dùng
    /// </summary>
    public class UserNotificationSettingsDto
    {
        /// <summary>
        /// ID người dùng
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Cho phép email thông báo
        /// </summary>
        public bool EnableEmailNotifications { get; set; } = true;

        /// <summary>
        /// Cho phép thông báo push
        /// </summary>
        public bool EnablePushNotifications { get; set; } = true;

        /// <summary>
        /// Cho phép thông báo trong ứng dụng
        /// </summary>
        public bool EnableInAppNotifications { get; set; } = true;

        /// <summary>
        /// Thông báo cập nhật học tập
        /// </summary>
        public bool NotifyLearningUpdates { get; set; } = true;

        /// <summary>
        /// Thông báo kết quả kiểm tra
        /// </summary>
        public bool NotifyTestResults { get; set; } = true;

        /// <summary>
        /// Thông báo tin nhắn mới
        /// </summary>
        public bool NotifyNewMessages { get; set; } = true;

        /// <summary>
        /// Thông báo hệ thống
        /// </summary>
        public bool NotifySystemUpdates { get; set; } = true;
    }
}
