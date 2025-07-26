using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.UserSubmission
{
    /// <summary>
    /// DTO cho cập nhật đơn nộp
    /// </summary>
    public class UpdateUserSubmissionDto
    {
        /// <summary>
        /// Tiêu đề
        /// </summary>
        [StringLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// Nội dung
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// ID trạng thái
        /// </summary>
        public int? StatusID { get; set; }

        /// <summary>
        /// Chuỗi phiên bản hàng
        /// </summary>
        public string RowVersionString { get; set; }
    }
}
