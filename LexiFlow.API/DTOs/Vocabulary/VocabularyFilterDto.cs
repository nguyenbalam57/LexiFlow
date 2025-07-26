namespace LexiFlow.API.DTOs.Vocabulary
{
    /// <summary>
    /// DTO cho lọc danh sách từ vựng
    /// </summary>
    public class VocabularyFilterDto
    {
        /// <summary>
        /// Tìm kiếm theo từ hoặc nghĩa
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Lọc theo level
        /// </summary>
        public List<string> Levels { get; set; } = new List<string>();

        /// <summary>
        /// Lọc theo loại từ
        /// </summary>
        public List<string> PartsOfSpeech { get; set; } = new List<string>();

        /// <summary>
        /// Lọc theo nhóm
        /// </summary>
        public List<int> GroupIDs { get; set; } = new List<int>();

        /// <summary>
        /// Chỉ hiển thị từ vựng đã học
        /// </summary>
        public bool? OnlyLearned { get; set; }

        /// <summary>
        /// Chỉ hiển thị từ vựng cần ôn tập
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
        public string SortBy { get; set; } = "Japanese";

        /// <summary>
        /// Sắp xếp tăng dần
        /// </summary>
        public bool SortAscending { get; set; } = true;
    }
}
