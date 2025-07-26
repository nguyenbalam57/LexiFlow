using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.VocabularyGroup
{
    /// <summary>
    /// DTO cho nhập nhóm từ vựng
    /// </summary>
    public class ImportVocabularyGroupDto
    {
        /// <summary>
        /// Tên nhóm từ vựng
        /// </summary>
        [Required]
        public string GroupName { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// ID danh mục
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Định dạng tệp
        /// </summary>
        [Required]
        public string FileFormat { get; set; } = "CSV";

        /// <summary>
        /// Tên các cột trong tệp
        /// </summary>
        public List<string> ColumnMappings { get; set; } = new List<string>();

        /// <summary>
        /// Đè lên nếu từ vựng đã tồn tại
        /// </summary>
        public bool OverrideExisting { get; set; } = false;
    }
}
