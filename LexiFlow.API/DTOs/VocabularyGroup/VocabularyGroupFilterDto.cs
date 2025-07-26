namespace LexiFlow.API.DTOs.VocabularyGroup
{
    /// <summary>
    /// DTO cho lọc nhóm từ vựng
    /// </summary>
    public class VocabularyGroupFilterDto
    {
        /// <summary>
        /// Từ khóa tìm kiếm
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// Lọc theo danh mục
        /// </summary>
        public List<int> CategoryIds { get; set; } = new List<int>();

        /// <summary>
        /// Chỉ hiển thị nhóm đang hoạt động
        /// </summary>
        public bool OnlyActive { get; set; } = true;

        /// <summary>
        /// Chỉ hiển thị nhóm do người dùng tạo
        /// </summary>
        public bool? OnlyUserCreated { get; set; }

        /// <summary>
        /// Sắp xếp theo
        /// </summary>
        public string OrderBy { get; set; } = "GroupName";

        /// <summary>
        /// Sắp xếp tăng dần
        /// </summary>
        public bool Ascending { get; set; } = true;

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Số lượng mỗi trang
        /// </summary>
        public int PageSize { get; set; } = 20;
    }
}
