namespace LexiFlow.API.DTOs.Category
{
    /// <summary>
    /// DTO cho thống kê theo danh mục
    /// </summary>
    public class CategoryStatsDto
    {
        /// <summary>
        /// ID của danh mục
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Tên của danh mục
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        /// Số lượng nhóm từ vựng trong danh mục
        /// </summary>
        public int Count { get; set; }
    }
}
