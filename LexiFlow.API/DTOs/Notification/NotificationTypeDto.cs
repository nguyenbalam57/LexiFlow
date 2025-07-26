namespace LexiFlow.API.DTOs.Notification
{
    /// <summary>
    /// DTO cho loại thông báo
    /// </summary>
    public class NotificationTypeDto
    {
        /// <summary>
        /// ID loại thông báo
        /// </summary>
        public int TypeID { get; set; }

        /// <summary>
        /// Tên loại thông báo
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Đường dẫn biểu tượng
        /// </summary>
        public string IconPath { get; set; }

        /// <summary>
        /// Mã màu
        /// </summary>
        public string ColorCode { get; set; }
    }
}
