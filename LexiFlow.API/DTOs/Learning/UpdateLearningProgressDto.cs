using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Learning
{
    public class UpdateLearningProgressDto
    {
        [Required]
        public int VocabularyID { get; set; }
        public bool IsCorrect { get; set; }
        public int MemoryStrength { get; set; }
    }
}
