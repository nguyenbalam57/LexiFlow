using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.WordList
{
    /// <summary>
    /// DTO cho sao chép danh sách từ
    /// </summary>
    public class WordListCopyDto
    {
        /// <summary>
        /// ID danh sách từ nguồn
        /// </summary>
        [Required]
        public int SourceListId { get; set; }

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
        /// Chỉ sao chép các mục đã chọn
        /// </summary>
        public List<int>? SelectedItemIds { get; set; }
    }
}
