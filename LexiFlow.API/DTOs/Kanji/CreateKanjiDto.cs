using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Kanji
{
    public class CreateKanjiDto
    {
        [Required]
        [StringLength(10)]
        public string Character { get; set; }

        [StringLength(100)]
        public string Onyomi { get; set; }

        [StringLength(100)]
        public string Kunyomi { get; set; }

        [StringLength(255)]
        public string Meaning { get; set; }

        public int? StrokeCount { get; set; }

        [StringLength(10)]
        public string JLPTLevel { get; set; }

        public int? Grade { get; set; }

        [StringLength(100)]
        public string Radicals { get; set; }

        [StringLength(100)]
        public string Components { get; set; }

        public string Examples { get; set; }

        public string MnemonicHint { get; set; }

        [StringLength(255)]
        public string WritingOrderImage { get; set; }

        public List<int> ComponentIds { get; set; } = new List<int>();
    }
}
