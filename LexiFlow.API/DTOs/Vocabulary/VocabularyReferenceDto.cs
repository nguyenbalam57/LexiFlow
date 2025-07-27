namespace LexiFlow.API.DTOs.Vocabulary
{
    /// <summary>
    /// DTO cho tham chiếu từ vựng trong các quan hệ
    /// </summary>
    public class VocabularyReferenceDto
    {
        /// <summary>
        /// ID của từ vựng
        /// </summary>
        public int VocabularyID { get; set; }

        /// <summary>
        /// Từ vựng
        /// </summary>
        public string Term { get; set; } = string.Empty;

        /// <summary>
        /// Cách đọc
        /// </summary>
        public string Reading { get; set; } = string.Empty;
    }
}
