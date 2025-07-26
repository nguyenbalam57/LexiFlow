namespace LexiFlow.API.DTOs.Kanji
{
    public class KanjiComponentDto
    {
        public int ComponentID { get; set; }
        public string ComponentName { get; set; }
        public string Character { get; set; }
        public string Meaning { get; set; }
        public string Type { get; set; }
        public int? StrokeCount { get; set; }
        public string Position { get; set; }
    }
}
