using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Sync
{
    /// <summary>
    /// Item trong danh sách từ vựng cá nhân để đồng bộ
    /// </summary>
    public class PersonalWordListItemSync
    {
        /// <summary>
        /// ID mục trong danh sách (có thể là ID tạm thời từ client nếu âm)
        /// </summary>
        public int ListItemID { get; set; }

        /// <summary>
        /// ID danh sách từ vựng cá nhân
        /// </summary>
        public int ListID { get; set; }

        /// <summary>
        /// Loại mục (Vocabulary, Kanji, Grammar)
        /// </summary>
        [Required]
        [StringLength(20)]
        public string ItemType { get; set; }

        /// <summary>
        /// ID của mục
        /// </summary>
        public int ItemID { get; set; }

        /// <summary>
        /// Ghi chú cho mục này
        /// </summary>
        [StringLength(500)]
        public string Notes { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Thời gian tạo trên client
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Thời gian cập nhật trên client
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// ID tham chiếu trên client (sử dụng cho các mục mới)
        /// </summary>
        public string ClientReferenceId { get; set; }
    }
}
