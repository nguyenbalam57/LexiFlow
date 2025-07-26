using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.VocabularyGroup
{
    /// <summary>
    /// DTO cho việc thêm từ vựng vào nhóm
    /// </summary>
    public class AddVocabularyToGroupDto
    {
        /// <summary>
        /// ID của từ vựng cần thêm vào nhóm
        /// </summary>
        [Required]
        public int VocabularyId { get; set; }
    }
}
