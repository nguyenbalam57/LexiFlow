namespace LexiFlow.API.DTOs.UserSubmission
{
    /// <summary>
    /// DTO cho bình luận đơn nộp
    /// </summary>
    public class SubmissionCommentDto
    {
        /// <summary>
        /// ID bình luận
        /// </summary>
        public int CommentID { get; set; }

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
        /// Nội dung bình luận
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
