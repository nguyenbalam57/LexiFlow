namespace LexiFlow.API.DTOs.Notification
{
    /// <summary>
    /// DTO cho mức độ ưu tiên thông báo
    /// </summary>
    public class NotificationPriorityDto
    {
        /// <summary>
        /// ID mức độ ưu tiên
        /// </summary>
        public int PriorityID { get; set; }

        /// <summary>
        /// Tên mức độ ưu tiên
        /// </summary>
        public string PriorityName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Mã màu
        /// </summary>
        public string ColorCode { get; set; }
    }
}
