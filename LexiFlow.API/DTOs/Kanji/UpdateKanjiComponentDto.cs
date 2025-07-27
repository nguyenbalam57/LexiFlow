using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Kanji
{
    /// <summary>
    /// DTO for updating an existing Kanji component
    /// </summary>
    public class UpdateKanjiComponentDto
    {
        [Required]
        [StringLength(10)]
        public string Character { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Meaning { get; set; }

        [Required]
        [StringLength(20)]
        public string Type { get; set; } // Radical, Element, etc.

        public int StrokeCount { get; set; }

        [Required]
        public string RowVersionString { get; set; }
    }
}
