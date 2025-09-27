using LexiFlow.Models.Cores;
using LexiFlow.Models.Learning.Commons;
using LexiFlow.Models.Learning.Vocabularys;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.Models.Learning.Kanjis
{
    /// <summary>
    /// Liên kết giữa Kanji và từ vựng
    /// </summary>
    [Index(nameof(KanjiId), nameof(VocabularyId), IsUnique = true, Name = "IX_KanjiVocabulary_Kanji_Vocab")]
    public class KanjiVocabulary : BaseLearning
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KanjiVocabularyId { get; set; }

        [Required]
        public int KanjiId { get; set; }

        [Required]
        public int VocabularyId { get; set; }

        public int? Position { get; set; }  // Vị trí Kanji trong từ vựng

        [StringLength(50)]
        public string ReadingUsed { get; set; } // On, Kun, Special

        [StringLength(255)]
        public string ReadingNotes { get; set; } // Ghi chú về cách đọc

        [StringLength(50)]
        public string UsageType { get; set; } // Common, Rare, Technical

        public bool IsIrregularReading { get; set; } = false; // Có cách đọc đặc biệt

        [StringLength(50)]
        public string MeaningRole { get; set; } // Primary, Secondary, Modifier

        public string StudyNote { get; set; } // Ghi chú học tập

        public int? Difficulty { get; set; } // Mức độ khó khi học

        // Navigation properties
        [ForeignKey("KanjiId")]
        public virtual Kanji Kanji { get; set; }

        [ForeignKey("VocabularyId")]
        public virtual Vocabulary Vocabulary { get; set; }
    }
}
