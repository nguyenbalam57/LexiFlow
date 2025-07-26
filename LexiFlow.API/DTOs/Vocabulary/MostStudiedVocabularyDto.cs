namespace LexiFlow.API.DTOs.Vocabulary
{
    /// <summary>
    /// DTO cho từ vựng được học nhiều nhất
    /// </summary>
    public class MostStudiedVocabularyDto
    {
        /// <summary>
        /// ID từ vựng
        /// </summary>
        public int VocabularyID { get; set; }

        /// <summary>
        /// Từ tiếng Nhật
        /// </summary>
        public string Japanese { get; set; }

        /// <summary>
        /// Nghĩa tiếng Việt
        /// </summary>
        public string Vietnamese { get; set; }

        /// <summary>
        /// Số lượt học
        /// </summary>
        public int StudyCount { get; set; }
    }
}
