using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.WordList
{
    public class AddMultipleWordsToListDto
    {
        [Required]
        public List<int> VocabularyIDs { get; set; } = new List<int>();
    }
}
