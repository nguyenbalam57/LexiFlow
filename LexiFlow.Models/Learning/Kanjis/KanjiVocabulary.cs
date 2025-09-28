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
    /// Quan hệ nhiều-nhiều với thuộc tính bổ sung
    /// KanjiId - VocabularyId là duy nhất
    /// </summary>
    [Index(nameof(KanjiId), nameof(VocabularyId), IsUnique = true, Name = "IX_KanjiVocabulary_Kanji_Vocab")]
    public class KanjiVocabulary : BaseLearning
    {
        /// <summary>
        /// Khóa chính của liên kết (tự tăng).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KanjiVocabularyId { get; set; }

        /// <summary>
        /// ID của Kanji.
        /// </summary>
        [Required]
        public int KanjiId { get; set; }

        /// <summary>
        /// ID của từ vựng.
        /// </summary>
        [Required]
        public int VocabularyId { get; set; }

        /// <summary>
        /// Vị trí của Kanji trong từ vựng (nếu có).
        /// </summary>
        public int? Position { get; set; }  // Vị trí Kanji trong từ vựng

        /// <summary>
        /// Cách đọc của Kanji trong từ vựng (nếu có).
        /// </summary>
        public string ReadingUsed { get; set; } // On, Kun, Special

        /// <summary>
        /// Ghi chú về cách đọc (nếu có).
        /// </summary>
        public string ReadingNotes { get; set; } // Ghi chú về cách đọc

        /// <summary>
        /// Có cách đọc đặc biệt.
        /// </summary>
        public bool IsIrregularReading { get; set; } = false;

        /// <summary>
        /// Vai trò nghĩa của Kanji trong từ vựng (nếu có).
        /// </summary>
        [StringLength(50)]
        public string MeaningRole { get; set; } // Primary, Secondary, Modifier

        /// <summary>
        /// Ghi chú học tập về Kanji trong từ vựng (nếu có).
        /// </summary>
        public string StudyNote { get; set; } // Ghi chú học tập

        /// <summary>
        /// Mức độ khó khi học Kanji trong từ vựng (1-10).
        /// </summary>
        public int? Difficulty { get; set; } // Mức độ khó khi học

        // Navigation properties
        /// <summary>
        /// Kanji liên quan.
        /// </summary>
        [ForeignKey("KanjiId")]
        public virtual Kanji Kanji { get; set; }

        /// <summary>
        /// Từ vựng liên quan.
        /// </summary>
        [ForeignKey("VocabularyId")]
        public virtual Vocabulary Vocabulary { get; set; }
    }
}
