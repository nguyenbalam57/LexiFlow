namespace LexiFlow.API.DTOs.Vocabulary
{
    /// <summary>
    /// DTO cho thống kê từ vựng
    /// </summary>
    public class VocabularyStatisticsDto
    {
        /// <summary>
        /// Tổng số từ vựng
        /// </summary>
        public int TotalVocabulary { get; set; }

        /// <summary>
        /// Thống kê từ vựng theo level
        /// </summary>
        public Dictionary<string, int> VocabularyByLevel { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// Thống kê từ vựng theo loại từ
        /// </summary>
        public Dictionary<string, int> VocabularyByPartOfSpeech { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// Thống kê từ vựng theo nhóm
        /// </summary>
        public Dictionary<string, int> VocabularyByGroup { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// Từ vựng được học nhiều nhất
        /// </summary>
        public List<MostStudiedVocabularyDto> MostStudiedVocabulary { get; set; } = new List<MostStudiedVocabularyDto>();
    }
}
