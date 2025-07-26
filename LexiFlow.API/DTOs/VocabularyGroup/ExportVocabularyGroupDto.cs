using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.VocabularyGroup
{
    /// <summary>
    /// DTO cho xuất nhóm từ vựng
    /// </summary>
    public class ExportVocabularyGroupDto
    {
        /// <summary>
        /// ID nhóm từ vựng
        /// </summary>
        [Required]
        public int GroupId { get; set; }

        /// <summary>
        /// Định dạng xuất
        /// </summary>
        [Required]
        public string ExportFormat { get; set; } = "CSV";

        /// <summary>
        /// Danh sách các trường cần xuất
        /// </summary>
        public List<string> FieldsToExport { get; set; } = new List<string>();

        /// <summary>
        /// Bao gồm ví dụ
        /// </summary>
        public bool IncludeExamples { get; set; } = true;

        /// <summary>
        /// Bao gồm ghi chú
        /// </summary>
        public bool IncludeNotes { get; set; } = true;
    }
}
