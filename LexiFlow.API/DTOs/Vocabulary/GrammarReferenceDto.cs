namespace LexiFlow.API.DTOs.Vocabulary
{
    /// <summary>
    /// DTO cho tham chiếu ngữ pháp
    /// </summary>
    public class GrammarReferenceDto
    {
        /// <summary>
        /// ID ngữ pháp
        /// </summary>
        public int GrammarID { get; set; }

        /// <summary>
        /// Tên điểm ngữ pháp
        /// </summary>
        public string GrammarPoint { get; set; }

        /// <summary>
        /// Mẫu ngữ pháp
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// Nghĩa
        /// </summary>
        public string Meaning { get; set; }
    }
}
