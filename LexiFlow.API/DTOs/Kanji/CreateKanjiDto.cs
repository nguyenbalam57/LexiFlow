using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Kanji
{
    public class CreateKanjiDto
    {
        [Required]
        [StringLength(10)]
        public string Character { get; set; }

        [StringLength(100)]
        public string OnYomi { get; set; }

        [StringLength(100)]
        public string KunYomi { get; set; }

        public int Strokes { get; set; }

        [StringLength(20)]
        public string JLPT { get; set; }

        [StringLength(20)]
        public string Grade { get; set; }

        [StringLength(50)]
        public string RadicalName { get; set; }

        public string StrokeOrder { get; set; }

        public List<KanjiMeaningDto> Meanings { get; set; } = new List<KanjiMeaningDto>();

        public List<KanjiExampleDto> Examples { get; set; } = new List<KanjiExampleDto>();

        public List<KanjiComponentDto> Components { get; set; } = new List<KanjiComponentDto>();
    }
}
