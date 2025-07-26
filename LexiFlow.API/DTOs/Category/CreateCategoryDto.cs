using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Category
{
    /// <summary>
    /// DTO cho tạo danh mục mới
    /// </summary>
    public class CreateCategoryDto
    {
        /// <summary>
        /// Tên danh mục
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả về danh mục
        /// </summary>
        [StringLength(255)]
        public string? Description { get; set; }

        /// <summary>
        /// Cấp độ (nếu có)
        /// </summary>
        [StringLength(20)]
        public string? Level { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int? DisplayOrder { get; set; }
    }
}
