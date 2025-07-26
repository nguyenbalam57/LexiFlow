namespace LexiFlow.API.DTOs.VocabularyGroup
{
    /// <summary>
    /// DTO cho tìm kiếm nhóm từ vựng
    /// </summary>
    public class VocabularyGroupSearchDto
    {
        /// <summary>
        /// Từ khóa tìm kiếm
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// Chỉ bao gồm nhóm hoạt động
        /// </summary>
        public bool ActiveOnly { get; set; } = true;

        /// <summary>
        /// ID danh mục để lọc
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Sắp xếp theo
        /// </summary>
        public string? SortBy { get; set; } = "GroupName";

        /// <summary>
        /// Hướng sắp xếp
        /// </summary>
        public string? SortDirection { get; set; } = "Asc";

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Kích thước trang
        /// </summary>
        public int PageSize { get; set; } = 20;
    }
}
