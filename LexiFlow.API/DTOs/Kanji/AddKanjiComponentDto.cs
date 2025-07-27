using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Kanji
{
    /// <summary>
    /// DTO for adding a component to a Kanji
    /// </summary>
    public class AddKanjiComponentDto
    {
        [Required]
        public int ComponentId { get; set; }

        [StringLength(50)]
        public string Position { get; set; } // top, bottom, left, right, etc.
    }
}
