namespace LexiFlow.API.DTOs.Sync
{
    /// <summary>
    /// Phương thức giải quyết xung đột
    /// </summary>
    public enum ConflictResolution
    {
        /// <summary>
        /// Sử dụng phiên bản từ client
        /// </summary>
        UseClientVersion = 1,

        /// <summary>
        /// Sử dụng phiên bản từ server
        /// </summary>
        UseServerVersion = 2,

        /// <summary>
        /// Sử dụng phiên bản tùy chỉnh
        /// </summary>
        UseCustomVersion = 3,

        /// <summary>
        /// Xóa mục
        /// </summary>
        DeleteItem = 4,

        /// <summary>
        /// Gộp hai phiên bản
        /// </summary>
        MergeVersions = 5
    }
}
