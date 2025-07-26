namespace LexiFlow.API.DTOs.Notification
{
    /// <summary>
    /// DTO cho thông tin thông báo
    /// </summary>
    public class NotificationDto
    {
        /// <summary>
        /// ID thông báo
        /// </summary>
        public int NotificationID { get; set; }

        /// <summary>
        /// ID người gửi (nếu có)
        /// </summary>
        public int? SenderUserID { get; set; }

        /// <summary>
        /// Tên người gửi
        /// </summary>
        public string? SenderName { get; set; }

        /// <summary>
        /// Tiêu đề thông báo
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Nội dung thông báo
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Loại thông báo
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Mức độ ưu tiên
        /// </summary>
        public string PriorityName { get; set; }

        /// <summary>
        /// Cho phép phản hồi
        /// </summary>
        public bool AllowResponses { get; set; }

        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// URL đính kèm
        /// </summary>
        public string AttachmentURL { get; set; }

        /// <summary>
        /// Thông báo hệ thống
        /// </summary>
        public bool IsSystemGenerated { get; set; }

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Thời gian đã đọc
        /// </summary>
        public DateTime? ReadAt { get; set; }

        /// <summary>
        /// Đã đọc
        /// </summary>
        public bool IsRead => ReadAt.HasValue;
    }
}
