using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.TechnicalTerm
{
    /// <summary>
    /// DTO cho cập nhật thuật ngữ kỹ thuật
    /// </summary>
    public class UpdateTechnicalTermDto
    {
        /// <summary>
        /// Thuật ngữ tiếng Nhật
        /// </summary>
        [StringLength(100)]
        public string Japanese { get; set; }

        /// <summary>
        /// Kana của thuật ngữ
        /// </summary>
        [StringLength(100)]
        public string Kana { get; set; }

        /// <summary>
        /// Romaji của thuật ngữ
        /// </summary>
        [StringLength(100)]
        public string Romaji { get; set; }

        /// <summary>
        /// Nghĩa tiếng Việt
        /// </summary>
        [StringLength(255)]
        public string Vietnamese { get; set; }

        /// <summary>
        /// Nghĩa tiếng Anh
        /// </summary>
        [StringLength(255)]
        public string English { get; set; }

        /// <summary>
        /// Lĩnh vực thuật ngữ
        /// </summary>
        [StringLength(100)]
        public string Field { get; set; }

        /// <summary>
        /// Lĩnh vực con
        /// </summary>
        [StringLength(100)]
        public string SubField { get; set; }

        /// <summary>
        /// Định nghĩa chi tiết
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        /// Ngữ cảnh sử dụng
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// Ví dụ sử dụng
        /// </summary>
        public string Examples { get; set; }

        /// <summary>
        /// Viết tắt
        /// </summary>
        [StringLength(50)]
        public string Abbreviation { get; set; }

        /// <summary>
        /// Các thuật ngữ liên quan
        /// </summary>
        [StringLength(255)]
        public string RelatedTerms { get; set; }

        /// <summary>
        /// Phòng ban liên quan
        /// </summary>
        [StringLength(100)]
        public string Department { get; set; }

        /// <summary>
        /// Chuỗi phiên bản hàng (dùng cho kiểm soát đồng thời)
        /// </summary>
        [Required]
        public string RowVersionString { get; set; }
    }
}