using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LexiFlow.Models.Core;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.Models.Learning.Kanji
{
    /// <summary>
    /// Liên kết giữa Kanji và từ vựng
    /// </summary>
    [Index(nameof(KanjiId), nameof(VocabularyId), IsUnique = true, Name = "IX_KanjiVocabulary_Kanji_Vocab")]
    public class KanjiVocabulary : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KanjiVocabularyId { get; set; }

        [Required]
        public int KanjiId { get; set; }

        [Required]
        public int VocabularyId { get; set; }

        public int? Position { get; set; }  // Vị trí Kanji trong từ vựng

        // Cải tiến: Chi tiết mối quan hệ
        [StringLength(50)]
        public string ReadingUsed { get; set; } // On, Kun, Special

        [StringLength(255)]
        public string ReadingNotes { get; set; } // Ghi chú về cách đọc

        // Cải tiến: Phân loại và sử dụng
        [StringLength(50)]
        public string UsageType { get; set; } // Common, Rare, Technical

        public bool IsIrregularReading { get; set; } = false; // Có cách đọc đặc biệt

        [StringLength(50)]
        public string MeaningRole { get; set; } // Primary, Secondary, Modifier

        // Cải tiến: Ghi chú học tập
        public string StudyNote { get; set; } // Ghi chú học tập

        public int? Difficulty { get; set; } // Mức độ khó khi học

        // Navigation properties
        [ForeignKey("KanjiId")]
        public virtual Kanji Kanji { get; set; }

        [ForeignKey("VocabularyId")]
        public virtual Vocabulary.Vocabulary Vocabulary { get; set; }
    }
}
