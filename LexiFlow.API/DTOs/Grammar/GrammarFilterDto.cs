namespace LexiFlow.API.DTOs.Grammar
{
    /// <summary>
    /// DTO cho lọc danh sách ngữ pháp
    /// </summary>
    public class GrammarFilterDto
    {
        /// <summary>
        /// Tìm kiếm theo tên
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Lọc theo JLPT level
        /// </summary>
        public List<string> JLPTLevels { get; set; } = new List<string>();

        /// <summary>
        /// Lọc theo danh mục
        /// </summary>
        public List<int> CategoryIDs { get; set; } = new List<int>();

        /// <summary>
        /// Chỉ hiển thị điểm ngữ pháp đã học
        /// </summary>
        public bool? OnlyLearned { get; set; }

        /// <summary>
        /// Chỉ hiển thị điểm ngữ pháp cần ôn tập
        /// </summary>
        public bool? NeedsReview { get; set; }

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Số lượng mỗi trang
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// Sắp xếp theo
        /// </summary>
        public string SortBy { get; set; } = "GrammarPoint";

        /// <summary>
        /// Sắp xếp tăng dần
        /// </summary>
        public bool SortAscending { get; set; } = true;
    }
}
