namespace LexiFlow.API.DTOs.Kanji
{
    /// <summary>
    /// DTO for component mapping
    /// </summary>
    public class KanjiComponentMappingDto
    {
        public int MappingID { get; set; }
        public int KanjiID { get; set; }
        public int ComponentID { get; set; }
        public string Position { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
