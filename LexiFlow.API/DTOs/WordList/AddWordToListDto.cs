using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.WordList
{
    public class AddWordToListDto
    {
        [Required]
        public int VocabularyID { get; set; }
    }
}
