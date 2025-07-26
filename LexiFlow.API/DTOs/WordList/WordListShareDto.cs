using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.WordList
{
    /// <summary>
    /// DTO cho chia sẻ danh sách từ
    /// </summary>
    public class WordListShareDto
    {
        /// <summary>
        /// ID danh sách từ
        /// </summary>
        [Required]
        public int ListId { get; set; }

        /// <summary>
        /// Danh sách người dùng để chia sẻ
        /// </summary>
        public List<int> UserIds { get; set; } = new List<int>();

        /// <summary>
        /// Cho phép chỉnh sửa
        /// </summary>
        public bool AllowEdit { get; set; } = false;

        /// <summary>
        /// Thời gian hết hạn
        /// </summary>
        public DateTime? ExpiryDate { get; set; }
    }
}
