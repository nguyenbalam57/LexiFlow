using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Sync
{
    /// <summary>
    /// Item từ vựng để đồng bộ
    /// </summary>
    public class VocabularySyncItem
    {
        /// <summary>
        /// ID từ vựng (có thể là ID tạm thời từ client nếu âm)
        /// </summary>
        public int VocabularyID { get; set; }

        /// <summary>
        /// Từ vựng tiếng Nhật
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Japanese { get; set; }

        /// <summary>
        /// Cách đọc
        /// </summary>
        [StringLength(100)]
        public string Reading { get; set; }

        /// <summary>
        /// Nghĩa của từ
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Meaning { get; set; }

        /// <summary>
        /// Ví dụ sử dụng
        /// </summary>
        [StringLength(1000)]
        public string Example { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        [StringLength(500)]
        public string Notes { get; set; }

        /// <summary>
        /// Cấp độ JLPT (N5-N1)
        /// </summary>
        [StringLength(2)]
        public string Level { get; set; }

        /// <summary>
        /// Danh sách ID danh mục
        /// </summary>
        public List<int> CategoryIds { get; set; } = new List<int>();

        /// <summary>
        /// Thời gian tạo trên client
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Thời gian cập nhật trên client
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Chuỗi kiểm tra xung đột (thường là hash hoặc timestamp)
        /// </summary>
        public string SyncChecksum { get; set; }

        /// <summary>
        /// ID tham chiếu trên client (sử dụng cho các mục mới)
        /// </summary>
        public string ClientReferenceId { get; set; }
    }
}
