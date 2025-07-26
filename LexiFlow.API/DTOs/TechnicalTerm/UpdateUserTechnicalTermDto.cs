namespace LexiFlow.API.DTOs.TechnicalTerm
{
    /// <summary>
    /// DTO cho cập nhật thuật ngữ kỹ thuật của người dùng
    /// </summary>
    public class UpdateUserTechnicalTermDto
    {
        /// <summary>
        /// Đã đánh dấu
        /// </summary>
        public bool IsBookmarked { get; set; }

        /// <summary>
        /// Ví dụ cá nhân
        /// </summary>
        public string PersonalExample { get; set; }

        /// <summary>
        /// Ngữ cảnh làm việc
        /// </summary>
        public string WorkContext { get; set; }
    }
}