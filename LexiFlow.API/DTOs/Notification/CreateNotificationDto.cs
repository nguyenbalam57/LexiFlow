using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Notification
{
    /// <summary>
    /// DTO cho tạo thông báo mới
    /// </summary>
    public class CreateNotificationDto
    {
        /// <summary>
        /// Tiêu đề thông báo
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        /// <summary>
        /// Nội dung thông báo
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// ID loại thông báo
        /// </summary>
        public int? TypeID { get; set; }

        /// <summary>
        /// ID mức độ ưu tiên
        /// </summary>
        public int? PriorityID { get; set; }

        /// <summary>
        /// Cho phép phản hồi
        /// </summary>
        public bool AllowResponses { get; set; } = false;

        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// URL đính kèm
        /// </summary>
        public string AttachmentURL { get; set; }

        /// <summary>
        /// Danh sách ID người nhận
        /// </summary>
        public List<int> RecipientUserIDs { get; set; } = new List<int>();

        /// <summary>
        /// Danh sách ID nhóm nhận
        /// </summary>
        public List<int> RecipientGroupIDs { get; set; } = new List<int>();
    }
}
