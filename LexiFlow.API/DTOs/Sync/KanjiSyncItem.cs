using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Sync
{
    /// <summary>
    /// Item kanji để đồng bộ
    /// </summary>
    public class KanjiSyncItem
    {
        /// <summary>
        /// ID kanji (có thể là ID tạm thời từ client nếu âm)
        /// </summary>
        public int KanjiID { get; set; }

        /// <summary>
        /// Ký tự kanji
        /// </summary>
        [Required]
        [StringLength(1)]
        public string Character { get; set; }

        /// <summary>
        /// Cách đọc âm On (âm Hán)
        /// </summary>
        [StringLength(100)]
        public string OnReading { get; set; }

        /// <summary>
        /// Cách đọc âm Kun (âm Nhật)
        /// </summary>
        [StringLength(100)]
        public string KunReading { get; set; }

        /// <summary>
        /// Nghĩa
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Meaning { get; set; }

        /// <summary>
        /// Số nét
        /// </summary>
        public int StrokeCount { get; set; }

        /// <summary>
        /// Thứ tự viết
        /// </summary>
        [StringLength(1000)]
        public string StrokeOrder { get; set; }

        /// <summary>
        /// Cấp độ JLPT (N5-N1)
        /// </summary>
        [StringLength(2)]
        public string Level { get; set; }

        /// <summary>
        /// Ví dụ sử dụng
        /// </summary>
        [StringLength(1000)]
        public string Examples { get; set; }

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
