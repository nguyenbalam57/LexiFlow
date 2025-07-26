using System;
using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.TechnicalTerm
{
    /// <summary>
    /// DTO cho thuật ngữ kỹ thuật
    /// </summary>
    public class TechnicalTermDto
    {
        /// <summary>
        /// ID của thuật ngữ
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
        /// Lĩnh vực con
        /// </summary>
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
        public string Abbreviation { get; set; }

        /// <summary>
        /// Các thuật ngữ liên quan
        /// </summary>
        public string RelatedTerms { get; set; }

        /// <summary>
        /// Phòng ban liên quan
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public int? CreatedByUserID { get; set; }

        /// <summary>
        /// Tên người tạo
        /// </summary>
        public string CreatedByUserName { get; set; }

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Thời gian cập nhật
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Chuỗi phiên bản hàng (dùng cho kiểm soát đồng thời)
        /// </summary>
        public string RowVersionString { get; set; }
    }
}