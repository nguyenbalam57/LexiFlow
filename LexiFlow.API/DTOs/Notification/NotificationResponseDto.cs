namespace LexiFlow.API.DTOs.Notification
{
    /// <summary>
    /// DTO cho phản hồi thông báo
    /// </summary>
    public class NotificationResponseDto
    {
        /// <summary>
        /// ID phản hồi
        /// </summary>
        public int ResponseID { get; set; }

        /// <summary>
        /// ID người nhận
        /// </summary>
        public int RecipientID { get; set; }

        /// <summary>
        /// Nội dung phản hồi
        /// </summary>
        public string ResponseContent { get; set; }

        /// <summary>
        /// ID loại phản hồi
        /// </summary>
        public int? ResponseTypeID { get; set; }

        /// <summary>
        /// Thời gian phản hồi
        /// </summary>
        public DateTime ResponseTime { get; set; }

        /// <summary>
        /// URL đính kèm
        /// </summary>
        public string AttachmentURL { get; set; }
    }
}
