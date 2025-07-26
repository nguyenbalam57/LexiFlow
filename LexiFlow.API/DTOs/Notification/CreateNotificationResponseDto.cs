using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Notification
{
    /// <summary>
    /// DTO cho tạo phản hồi thông báo
    /// </summary>
    public class CreateNotificationResponseDto
    {
        /// <summary>
        /// ID người nhận
        /// </summary>
        [Required]
        public int RecipientID { get; set; }

        /// <summary>
        /// Nội dung phản hồi
        /// </summary>
        [Required]
        public string ResponseContent { get; set; }

        /// <summary>
        /// ID loại phản hồi
        /// </summary>
        public int? ResponseTypeID { get; set; }

        /// <summary>
        /// URL đính kèm
        /// </summary>
        public string AttachmentURL { get; set; }
    }
}
