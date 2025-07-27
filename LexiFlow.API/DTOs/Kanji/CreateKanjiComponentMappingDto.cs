using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Kanji
{
    /// <summary>
    /// DTO for creating a Kanji component mapping
    /// </summary>
    public class CreateKanjiComponentMappingDto
    {
        /// <summary>
        /// ID of the component
        /// </summary>
        [Required]
        public int ComponentID { get; set; }

        /// <summary>
        /// Position of the component in the Kanji
        /// </summary>
        [StringLength(50)]
        public string Position { get; set; }
    }
}
