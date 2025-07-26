namespace LexiFlow.API.DTOs.Sync
{
    /// <summary>
    /// DTO cho tiến trình đồng bộ
    /// </summary>
    public class SyncProgressDto
    {
        /// <summary>
        /// ID phiên đồng bộ
        /// </summary>
        public string SessionId { get; set; } = string.Empty;

        /// <summary>
        /// Loại thực thể đang đồng bộ
        /// </summary>
        public string CurrentEntityType { get; set; } = string.Empty;

        /// <summary>
        /// Số lượng đã đồng bộ
        /// </summary>
        public int Processed { get; set; }

        /// <summary>
        /// Tổng số cần đồng bộ
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Phần trăm hoàn thành
        /// </summary>
        public int PercentComplete => Total > 0 ? (int)(Processed * 100.0 / Total) : 0;

        /// <summary>
        /// Trạng thái
        /// </summary>
        public string Status { get; set; } = "Running";

        /// <summary>
        /// Thời điểm bắt đầu
        /// </summary>
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Thời gian ước tính còn lại (giây)
        /// </summary>
        public int? EstimatedSecondsRemaining { get; set; }
    }
}
