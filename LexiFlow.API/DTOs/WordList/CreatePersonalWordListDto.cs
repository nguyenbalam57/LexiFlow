using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.WordList
{
    public class CreatePersonalWordListDto
    {
        [Required]
        [StringLength(100)]
        public string ListName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
    }
}
