using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.UserSubmission
{
    /// <summary>
    /// DTO cho tạo tệp đính kèm đơn nộp
    /// </summary>
    public class CreateSubmissionAttachmentDto
    {
        /// <summary>
        /// Tên tệp
        /// </summary>
        [Required]
        public string FileName { get; set; }

        /// <summary>
        /// Đường dẫn tệp
        /// </summary>
        [Required]
        public string FilePath { get; set; }

        /// <summary>
        /// Kích thước tệp
        /// </summary>
        [Required]
        public long FileSize { get; set; }

        /// <summary>
        /// Loại tệp
        /// </summary>
        [Required]
        public string FileType { get; set; }
    }
}
