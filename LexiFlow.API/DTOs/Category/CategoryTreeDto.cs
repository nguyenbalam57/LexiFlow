namespace LexiFlow.API.DTOs.Category
{
    /// <summary>
    /// DTO cho cấu trúc cây danh mục
    /// </summary>
    public class CategoryTreeDto
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
        /// ID của danh mục cha (nếu có)
        /// </summary>
        public int? ParentCategoryID { get; set; }

        /// <summary>
        /// Cấp độ trong cây
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Danh sách các danh mục con
        /// </summary>
        public List<CategoryTreeDto> Children { get; set; } = new List<CategoryTreeDto>();
    }
}
