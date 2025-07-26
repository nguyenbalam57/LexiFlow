using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Grammar
{
    /// <summary>
    /// DTO cho tạo đánh giá điểm ngữ pháp
    /// </summary>
    public class CreateGrammarRatingDto
    {
        /// <summary>
        /// ID điểm ngữ pháp
        /// </summary>
        [Required]
        public int GrammarID { get; set; }

        /// <summary>
        /// Điểm đánh giá (1-5)
        /// </summary>
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        /// <summary>
        /// Nhận xét
        /// </summary>
        public string Comment { get; set; }
    }
}
