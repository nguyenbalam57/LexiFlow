using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.VocabularyGroup
{
    /// <summary>
    /// DTO cho việc xóa từ vựng khỏi nhóm
    /// </summary>
    public class RemoveVocabularyFromGroupDto
    {
        /// <summary>
        /// ID của từ vựng cần xóa khỏi nhóm
        /// </summary>
        [Required]
        public int VocabularyId { get; set; }
    }
}
