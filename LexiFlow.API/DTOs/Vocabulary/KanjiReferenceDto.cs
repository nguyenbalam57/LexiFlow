namespace LexiFlow.API.DTOs.Vocabulary
{
    /// <summary>
    /// DTO cho tham chiếu kanji trong từ vựng
    /// </summary>
    public class KanjiReferenceDto
    {
        /// <summary>
        /// ID kanji
        /// </summary>
        public int KanjiID { get; set; }

        /// <summary>
        /// Ký tự kanji
        /// </summary>
        public string Character { get; set; }

        /// <summary>
        /// Âm on
        /// </summary>
        public string Onyomi { get; set; }

        /// <summary>
        /// Âm kun
        /// </summary>
        public string Kunyomi { get; set; }

        /// <summary>
        /// Nghĩa
        /// </summary>
        public string Meaning { get; set; }

        /// <summary>
        /// Vị trí trong từ
        /// </summary>
        public int Position { get; set; }
    }
}
