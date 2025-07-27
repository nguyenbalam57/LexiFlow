using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Kanji
{
    /// <summary>
    /// DTO for adding a Kanji to a vocabulary item
    /// </summary>
    public class AddKanjiToVocabularyDto
    {
        [Required]
        public int KanjiId { get; set; }

        [Required]
        public int Position { get; set; } // Position in the vocabulary word
    }
}
