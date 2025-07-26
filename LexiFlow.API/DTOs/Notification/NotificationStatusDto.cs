namespace LexiFlow.API.DTOs.Notification
{
    /// <summary>
    /// DTO cho trạng thái thông báo
    /// </summary>
    public class NotificationStatusDto
    {
        /// <summary>
        /// ID trạng thái
        /// </summary>
        public int StatusID { get; set; }

        /// <summary>
        /// Tên trạng thái
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Mã màu
        /// </summary>
        public string ColorCode { get; set; }
    }
}
