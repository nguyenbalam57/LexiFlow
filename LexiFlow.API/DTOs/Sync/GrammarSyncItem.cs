using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Sync
{
    /// <summary>
    /// Item ngữ pháp để đồng bộ
    /// </summary>
    public class GrammarSyncItem
    {
        /// <summary>
        /// ID ngữ pháp (có thể là ID tạm thời từ client nếu âm)
        /// </summary>
        public int GrammarID { get; set; }

        /// <summary>
        /// Mẫu ngữ pháp
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Pattern { get; set; }

        /// <summary>
        /// Nghĩa của mẫu ngữ pháp
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Meaning { get; set; }

        /// <summary>
        /// Cách sử dụng
        /// </summary>
        [StringLength(1000)]
        public string Usage { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        [StringLength(1000)]
        public string Notes { get; set; }

        /// <summary>
        /// Cấp độ JLPT (N5-N1)
        /// </summary>
        [StringLength(2)]
        public string Level { get; set; }

        /// <summary>
        /// Danh sách ví dụ
        /// </summary>
        public List<GrammarExampleSyncItem> Examples { get; set; } = new List<GrammarExampleSyncItem>();

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
