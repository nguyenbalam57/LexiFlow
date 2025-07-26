using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Category
{
    #region Category DTOs
    /// <summary>
    /// DTO cho thông tin danh mục
    /// </summary>
    public class CategoryDto
    {
        /// <summary>
        /// ID của danh mục
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// Tên danh mục
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả về danh mục
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Cấp độ (nếu có)
        /// </summary>
        public string? Level { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Trạng thái hoạt động của danh mục
        /// </summary>
        public bool IsActive { get; set; }

        public int? ParentCategoryID { get; set; }

        /// <summary>
        /// Số nhóm từ vựng trong danh mục
        /// </summary>
        public int GroupCount { get; set; }

        /// <summary>
        /// Thời gian tạo danh mục
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Thời gian cập nhật danh mục
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Chuỗi phiên bản hàng (dùng cho kiểm soát đồng thời)
        /// </summary>
        public string RowVersionString { get; set; } = string.Empty;
    }

    #endregion
}
