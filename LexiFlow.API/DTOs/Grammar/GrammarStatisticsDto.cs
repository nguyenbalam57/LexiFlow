namespace LexiFlow.API.DTOs.Grammar
{
    /// <summary>
    /// DTO cho thống kê về ngữ pháp
    /// </summary>
    public class GrammarStatisticsDto
    {
        /// <summary>
        /// Tổng số điểm ngữ pháp
        /// </summary>
        public int TotalGrammarPoints { get; set; }

        /// <summary>
        /// Thống kê ngữ pháp theo JLPT level
        /// </summary>
        public Dictionary<string, int> GrammarByJLPTLevel { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// Thống kê ngữ pháp theo danh mục
        /// </summary>
        public Dictionary<string, int> GrammarByCategory { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// Điểm ngữ pháp được học nhiều nhất
        /// </summary>
        public List<MostStudiedGrammarDto> MostStudiedGrammar { get; set; } = new List<MostStudiedGrammarDto>();
    }
}
