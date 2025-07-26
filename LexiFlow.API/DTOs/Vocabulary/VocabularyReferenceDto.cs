namespace LexiFlow.API.DTOs.Vocabulary
{
    /// <summary>
    /// DTO cho tham chiếu từ vựng
    /// </summary>
    public class VocabularyReferenceDto
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
        /// Kana
        /// </summary>
        public string Kana { get; set; }

        /// <summary>
        /// Nghĩa tiếng Việt
        /// </summary>
        public string Vietnamese { get; set; }

        /// <summary>
        /// Loại quan hệ
        /// </summary>
        public string RelationType { get; set; }
    }
}
