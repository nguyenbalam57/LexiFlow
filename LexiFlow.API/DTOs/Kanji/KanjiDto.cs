using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Kanji
{
    public class KanjiDto
    {
        public int Id { get; set; }
        public string Character { get; set; }
        public string OnYomi { get; set; }
        public string KunYomi { get; set; }
        public int Strokes { get; set; }
        public string JLPT { get; set; }
        public string Grade { get; set; }
        public string RadicalName { get; set; }
        public string StrokeOrder { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string RowVersionString { get; set; }
        public List<KanjiMeaningDto> Meanings { get; set; } = new List<KanjiMeaningDto>();
        public List<KanjiExampleDto> Examples { get; set; } = new List<KanjiExampleDto>();
    }

}
