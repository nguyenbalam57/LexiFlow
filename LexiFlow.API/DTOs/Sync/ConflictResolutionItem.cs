
using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Sync
{
    /// <summary>
    /// Item giải quyết xung đột
    /// </summary>
    public class ConflictResolutionItem
    {
        /// <summary>
        /// ID xung đột
        /// </summary>
        public int ConflictId { get; set; }

        /// <summary>
        /// Loại đối tượng xung đột (Vocabulary, Kanji, Grammar, LearningProgress, PersonalWordList)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string EntityType { get; set; }

        /// <summary>
        /// ID đối tượng xung đột
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// Phương thức giải quyết
        /// </summary>
        [Required]
        public ConflictResolution Resolution { get; set; }

        /// <summary>
        /// Dữ liệu tùy chỉnh (nếu Resolution là UseCustomVersion)
        /// </summary>
        public string CustomData { get; set; }

        /// <summary>
        /// Ghi chú giải quyết xung đột
        /// </summary>
        [StringLength(500)]
        public string ResolutionNotes { get; set; }
    }
}
