namespace LexiFlow.API.DTOs.Grammar
{
    /// <summary>
    /// DTO cho điểm ngữ pháp được học nhiều nhất
    /// </summary>
    public class MostStudiedGrammarDto
    {
        /// <summary>
        /// ID điểm ngữ pháp
        /// </summary>
        public int GrammarID { get; set; }

        /// <summary>
        /// Tên điểm ngữ pháp
        /// </summary>
        public string GrammarPoint { get; set; }

        /// <summary>
        /// JLPT level
        /// </summary>
        public string JLPTLevel { get; set; }

        /// <summary>
        /// Số lượt học
        /// </summary>
        public int StudyCount { get; set; }
    }
}
