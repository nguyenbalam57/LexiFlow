namespace LexiFlow.API.DTOs.Category
{
    /// <summary>
    /// DTO cho lọc danh mục
    /// </summary>
    public class CategoryFilterDto
    {
        /// <summary>
        /// Từ khóa tìm kiếm
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// Chỉ hiển thị danh mục đang hoạt động
        /// </summary>
        public bool OnlyActive { get; set; } = true;

        /// <summary>
        /// Lọc theo cấp độ
        /// </summary>
        public string? Level { get; set; }

        /// <summary>
        /// Sắp xếp theo
        /// </summary>
        public string OrderBy { get; set; } = "DisplayOrder";

        /// <summary>
        /// Sắp xếp tăng dần
        /// </summary>
        public bool Ascending { get; set; } = true;
    }
}
