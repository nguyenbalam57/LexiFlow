namespace LexiFlow.API.DTOs.WordList
{
    /// <summary>
    /// DTO cho lọc danh sách từ
    /// </summary>
    public class WordListFilterDto
    {
        /// <summary>
        /// Từ khóa tìm kiếm
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// Sắp xếp theo
        /// </summary>
        public string OrderBy { get; set; } = "ListName";

        /// <summary>
        /// Sắp xếp tăng dần
        /// </summary>
        public bool Ascending { get; set; } = true;

        /// <summary>
        /// Lọc theo số lượng từ
        /// </summary>
        public int? MinItemCount { get; set; }

        /// <summary>
        /// Thời gian tạo từ
        /// </summary>
        public DateTime? CreatedFrom { get; set; }

        /// <summary>
        /// Thời gian tạo đến
        /// </summary>
        public DateTime? CreatedTo { get; set; }

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
