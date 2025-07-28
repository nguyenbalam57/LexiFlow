using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Sync
{
    /// <summary>
    /// Item tiến trình học tập để đồng bộ
    /// </summary>
    public class LearningProgressSyncItem
    {
        /// <summary>
        /// ID tiến trình học tập (có thể là ID tạm thời từ client nếu âm)
        /// </summary>
        public int ProgressID { get; set; }

        /// <summary>
        /// Loại đối tượng học tập (Vocabulary, Kanji, Grammar)
        /// </summary>
        [Required]
        [StringLength(20)]
        public string ItemType { get; set; }

        /// <summary>
        /// ID của đối tượng học tập
        /// </summary>
        public int ItemID { get; set; }

        /// <summary>
        /// Mức độ thành thạo (0-100)
        /// </summary>
        public int ProficiencyLevel { get; set; }

        /// <summary>
        /// Số lần ôn tập
        /// </summary>
        public int ReviewCount { get; set; }

        /// <summary>
        /// Thời gian ôn tập gần nhất
        /// </summary>
        public DateTime? LastReviewedAt { get; set; }

        /// <summary>
        /// Thời gian ôn tập tiếp theo (theo thuật toán SRS)
        /// </summary>
        public DateTime? NextReviewAt { get; set; }

        /// <summary>
        /// Yếu tố EF (Ease Factor) trong thuật toán SRS
        /// </summary>
        public float EaseFactor { get; set; } = 2.5f;

        /// <summary>
        /// Khoảng thời gian ôn tập hiện tại (ngày)
        /// </summary>
        public int CurrentInterval { get; set; }

        /// <summary>
        /// Ghi chú học tập
        /// </summary>
        [StringLength(500)]
        public string Notes { get; set; }

        /// <summary>
        /// Đã đánh dấu là đã học xong chưa
        /// </summary>
        public bool IsLearned { get; set; }

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
