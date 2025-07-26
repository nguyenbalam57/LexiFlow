using System;

namespace LexiFlow.API.DTOs.TechnicalTerm
{
    /// <summary>
    /// DTO cho thuật ngữ kỹ thuật của người dùng
    /// </summary>
    public class UserTechnicalTermDto
    {
        /// <summary>
        /// ID thuật ngữ người dùng
        /// </summary>
        public int UserTermID { get; set; }

        /// <summary>
        /// ID người dùng
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// ID thuật ngữ
        /// </summary>
        public int TermID { get; set; }

        /// <summary>
        /// Thuật ngữ tiếng Nhật
        /// </summary>
        public string Japanese { get; set; }

        /// <summary>
        /// Kana của thuật ngữ
        /// </summary>
        public string Kana { get; set; }

        /// <summary>
        /// Romaji của thuật ngữ
        /// </summary>
        public string Romaji { get; set; }

        /// <summary>
        /// Nghĩa tiếng Việt
        /// </summary>
        public string Vietnamese { get; set; }

        /// <summary>
        /// Nghĩa tiếng Anh
        /// </summary>
        public string English { get; set; }

        /// <summary>
        /// Lĩnh vực thuật ngữ
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Đã đánh dấu
        /// </summary>
        public bool IsBookmarked { get; set; }

        /// <summary>
        /// Tần suất sử dụng
        /// </summary>
        public int UsageFrequency { get; set; }

        /// <summary>
        /// Lần sử dụng cuối cùng
        /// </summary>
        public DateTime? LastUsed { get; set; }

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