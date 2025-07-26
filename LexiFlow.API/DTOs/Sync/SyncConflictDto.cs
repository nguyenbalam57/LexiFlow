namespace LexiFlow.API.DTOs.Sync
{
    /// <summary>
    /// DTO cho xung đột đồng bộ
    /// </summary>
    public class SyncConflictDto
    {
        /// <summary>
        /// ID thực thể
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// Loại thực thể
        /// </summary>
        public string EntityType { get; set; } = string.Empty;

        /// <summary>
        /// Dữ liệu trên máy chủ
        /// </summary>
        public object? ServerData { get; set; }

        /// <summary>
        /// Dữ liệu trên thiết bị
        /// </summary>
        public object? ClientData { get; set; }

        /// <summary>
        /// Dấu thời gian phiên bản máy chủ
        /// </summary>
        public DateTime ServerTimestamp { get; set; }

        /// <summary>
        /// Dấu thời gian phiên bản thiết bị
        /// </summary>
        public DateTime ClientTimestamp { get; set; }

        /// <summary>
        /// Loại xung đột
        /// </summary>
        public string ConflictType { get; set; } = string.Empty;
    }
}
