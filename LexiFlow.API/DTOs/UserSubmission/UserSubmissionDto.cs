namespace LexiFlow.API.DTOs.UserSubmission
{
    /// <summary>
    /// DTO cho đơn nộp của người dùng
    /// </summary>
    public class UserSubmissionDto
    {
        /// <summary>
        /// ID đơn nộp
        /// </summary>
        public int SubmissionID { get; set; }

        /// <summary>
        /// ID người dùng
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Tên người dùng
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Loại đơn nộp
        /// </summary>
        public string SubmissionType { get; set; }

        /// <summary>
        /// Tiêu đề
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Nội dung
        /// </summary>
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
        /// ID trạng thái
        /// </summary>
        public int StatusID { get; set; }

        /// <summary>
        /// Tên trạng thái
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// Danh sách tệp đính kèm
        /// </summary>
        public List<SubmissionAttachmentDto> Attachments { get; set; } = new List<SubmissionAttachmentDto>();

        /// <summary>
        /// Danh sách bình luận
        /// </summary>
        public List<SubmissionCommentDto> Comments { get; set; } = new List<SubmissionCommentDto>();

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Thời gian cập nhật
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
