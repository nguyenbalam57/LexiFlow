using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Kanji
{
    public class KanjiExampleDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Word { get; set; }

        [StringLength(100)]
        public string Reading { get; set; }

        [StringLength(255)]
        public string Meaning { get; set; }

        [StringLength(10)]
        public string LanguageCode { get; set; } = "vi";
    }
}
