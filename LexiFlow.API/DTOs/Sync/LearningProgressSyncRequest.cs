using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Sync
{
    /// <summary>
    /// Request model cho đồng bộ tiến trình học tập
    /// </summary>
    public class LearningProgressSyncRequest
    {
        /// <summary>
        /// Thời gian đồng bộ lần cuối từ client
        /// </summary>
        public DateTime? LastSyncTime { get; set; }

        /// <summary>
        /// Định danh thiết bị thực hiện đồng bộ
        /// </summary>
        [Required]
        [StringLength(100)]
        public string DeviceId { get; set; }

        /// <summary>
        /// Phiên bản ứng dụng client
        /// </summary>
        [StringLength(20)]
        public string AppVersion { get; set; }

        /// <summary>
        /// Danh sách tiến trình học tập cần đồng bộ từ client lên server
        /// </summary>
        [Required]
        public List<Models.LearningProgress> Items { get; set; } = new List<Models.LearningProgress>();

        /// <summary>
        /// Danh sách ID tiến trình học tập đã xóa trên client
        /// </summary>
        public List<int> DeletedItemIds { get; set; } = new List<int>();

        /// <summary>
        /// Giới hạn số lượng item trả về trong 1 lần đồng bộ
        /// </summary>
        public int? MaxItemsToReturn { get; set; }

        /// <summary>
        /// Có yêu cầu đồng bộ đầy đủ không
        /// </summary>
        public bool FullSync { get; set; }
    }
}
