namespace LexiFlow.API.DTOs.Kanji
{
    /// <summary>
    /// DTO cho lọc danh sách kanji
    /// </summary>
    public class KanjiFilterDto
    {
        /// <summary>
        /// Tìm kiếm theo ký tự hoặc nghĩa
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Lọc theo JLPT level
        /// </summary>
        public List<string> JLPTLevels { get; set; } = new List<string>();

        /// <summary>
        /// Lọc theo grade
        /// </summary>
        public List<int> Grades { get; set; } = new List<int>();

        /// <summary>
        /// Lọc theo số nét
        /// </summary>
        public int? MinStrokeCount { get; set; }
        public int? MaxStrokeCount { get; set; }

        /// <summary>
        /// Lọc theo bộ thủ
        /// </summary>
        public List<string> Radicals { get; set; } = new List<string>();

        /// <summary>
        /// Chỉ hiển thị kanji đã học
        /// </summary>
        public bool? OnlyLearned { get; set; }

        /// <summary>
        /// Chỉ hiển thị kanji cần ôn tập
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
        public string SortBy { get; set; } = "StrokeCount";

        /// <summary>
        /// Sắp xếp tăng dần
        /// </summary>
        public bool SortAscending { get; set; } = true;
    }
}
