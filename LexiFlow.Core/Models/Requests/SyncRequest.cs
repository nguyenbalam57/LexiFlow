using LexiFlow.Models;

namespace LexiFlow.Core.Models.Requests
{
    /// <summary>
    /// Yêu cầu đồng bộ dữ liệu
    /// </summary>
    public class SyncRequest
    {
        /// <summary>
        /// Thời gian đồng bộ lần cuối
        /// </summary>
        public DateTime? LastSyncTime { get; set; }

        /// <summary>
        /// Danh sách từ vựng cần tải lên
        /// </summary>
        public List<Vocabulary> ModifiedItems { get; set; } = new List<Vocabulary>();

        /// <summary>
        /// Danh sách ID từ vựng đã xóa
        /// </summary>
        public List<int> DeletedItemIds { get; set; } = new List<int>();

        /// <summary>
        /// ID thiết bị đồng bộ
        /// </summary>
        public string DeviceId { get; set; }
    }
}
