using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.UserSubmission
{
    /// <summary>
    /// DTO cho chuyển đổi trạng thái đơn nộp
    /// </summary>
    public class ChangeSubmissionStatusDto
    {
        /// <summary>
        /// ID đơn nộp
        /// </summary>
        [Required]
        public int SubmissionID { get; set; }

        /// <summary>
        /// ID trạng thái mới
        /// </summary>
        [Required]
        public int NewStatusID { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Notes { get; set; }
    }
}
