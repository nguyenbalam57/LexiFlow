using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.VocabularyGroup
{
    /// <summary>
    /// Data transfer object for creating a vocabulary group
    /// </summary>
    public class CreateVocabularyGroupDto
    {
        /// <summary>
        /// Name of the vocabulary group
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string GroupName { get; set; } = string.Empty;

        /// <summary>
        /// Description of the vocabulary group
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Category ID this group belongs to
        /// </summary>
        public int? CategoryId { get; set; }
    }
}
