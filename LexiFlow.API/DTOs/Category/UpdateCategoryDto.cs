namespace LexiFlow.API.DTOs.Category
{
    /// <summary>
    /// DTO cho cập nhật danh mục
    /// </summary>
    public class UpdateCategoryDto : CreateCategoryDto
    {
        /// <summary>
        /// Chuỗi phiên bản hàng (dùng cho kiểm soát đồng thời)
        /// </summary>
        public string RowVersionString { get; set; } = string.Empty;

        /// <summary>
        /// Trạng thái hoạt động của danh mục
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
