namespace LexiFlow.API.DTOs.Sync
{
    /// <summary>
    /// DTO cho cấu hình đồng bộ
    /// </summary>
    public class SyncConfigDto
    {
        /// <summary>
        /// Kích thước lô tối đa
        /// </summary>
        public int MaxBatchSize { get; set; } = 100;

        /// <summary>
        /// Thời gian chờ tối đa (giây)
        /// </summary>
        public int MaxTimeout { get; set; } = 60;

        /// <summary>
        /// Danh sách các loại thực thể có thể đồng bộ
        /// </summary>
        public List<string> SyncableEntities { get; set; } = new List<string>();

        /// <summary>
        /// Tần suất đồng bộ tự động (phút)
        /// </summary>
        public int AutoSyncInterval { get; set; } = 30;

        /// <summary>
        /// Cho phép đồng bộ khi sử dụng dữ liệu di động
        /// </summary>
        public bool SyncOnMobileData { get; set; } = false;
    }
}
