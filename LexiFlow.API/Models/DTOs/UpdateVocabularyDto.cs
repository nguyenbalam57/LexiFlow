using System.ComponentModel.DataAnnotations;
namespace LexiFlow.API.Models.DTOs
{
    public class UpdateVocabularyDto : CreateVocabularyDto
    {
        public string RowVersionString { get; set; } = string.Empty;
    }
}
