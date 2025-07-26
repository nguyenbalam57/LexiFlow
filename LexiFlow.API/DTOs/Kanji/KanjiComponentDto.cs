using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Kanji
{
    public class KanjiComponentDto
    {
        public int Id { get; set; }
        public int ComponentId { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        public string Position { get; set; }
    }
}
