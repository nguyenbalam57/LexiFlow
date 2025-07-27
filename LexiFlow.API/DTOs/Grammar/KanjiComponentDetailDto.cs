namespace LexiFlow.API.DTOs.Grammar
{
    /// <summary>
    /// DTO for component details
    /// </summary>
    public class KanjiComponentDetailDto
    {
        public int Id { get; set; }
        public string Character { get; set; }
        public string Name { get; set; }
        public string Meaning { get; set; }
        public string Type { get; set; }
        public int StrokeCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string RowVersionString { get; set; }
    }
}
