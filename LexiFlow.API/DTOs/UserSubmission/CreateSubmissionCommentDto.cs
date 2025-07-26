using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.UserSubmission
{
    /// <summary>
    /// DTO cho tạo bình luận đơn nộp
    /// </summary>
    public class CreateSubmissionCommentDto
    {
        /// <summary>
        /// ID đơn nộp
        /// </summary>
        [Required]
        public int SubmissionID { get; set; }

        /// <summary>
        /// Nội dung bình luận
        /// </summary>
        [Required]
        public string Comment { get; set; }
    }
}
