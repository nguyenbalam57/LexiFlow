using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.WordList
{
    /// <summary>
    /// DTO cho gộp danh sách từ
    /// </summary>
    public class WordListMergeDto
    {
        /// <summary>
        /// Danh sách ID của các danh sách từ cần gộp
        /// </summary>
        [Required]
        public List<int> SourceListIds { get; set; } = new List<int>();

        /// <summary>
        /// Tên danh sách mới
        /// </summary>
        [Required]
        [StringLength(100)]
        public string NewListName { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả
        /// </summary>
        [StringLength(255)]
        public string? Description { get; set; }

        /// <summary>
        /// Loại bỏ các mục trùng lặp
        /// </summary>
        public bool RemoveDuplicates { get; set; } = true;
    }
}
