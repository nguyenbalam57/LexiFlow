using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Grammar
{
    /// <summary>
    /// DTO cho đánh giá điểm ngữ pháp
    /// </summary>
    public class GrammarRatingDto
    {
        /// <summary>
        /// ID đánh giá
        /// </summary>
        public int RatingID { get; set; }

        /// <summary>
        /// ID điểm ngữ pháp
        /// </summary>
        public int GrammarID { get; set; }

        /// <summary>
        /// ID người dùng
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Điểm đánh giá (1-5)
        /// </summary>
        [Range(1, 5)]
        public int Rating { get; set; }

        /// <summary>
        /// Nhận xét
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Thời gian đánh giá
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
