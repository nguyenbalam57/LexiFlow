using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.User
{
    public class UserPersonalDefinitionDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Text { get; set; }

        [StringLength(50)]
        public string PartOfSpeech { get; set; }

        public int SortOrder { get; set; }
    }
}
