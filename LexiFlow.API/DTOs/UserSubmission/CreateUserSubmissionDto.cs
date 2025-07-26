using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.UserSubmission
{
    /// <summary>
    /// DTO cho tạo đơn nộp
    /// </summary>
    public class CreateUserSubmissionDto
    {
        /// <summary>
        /// Loại đơn nộp
        /// </summary>
        [Required]
        [StringLength(50)]
        public string SubmissionType { get; set; }

        /// <summary>
        /// Tiêu đề
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// Nội dung
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// ID mục liên quan
        /// </summary>
        public int? RelatedItemID { get; set; }

        /// <summary>
        /// Loại mục liên quan
        /// </summary>
        public string RelatedItemType { get; set; }

        /// <summary>
        /// Danh sách tệp đính kèm
        /// </summary>
        public List<CreateSubmissionAttachmentDto> Attachments { get; set; } = new List<CreateSubmissionAttachmentDto>();
    }
}
