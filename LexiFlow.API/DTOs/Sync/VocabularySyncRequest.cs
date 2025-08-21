using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Sync
{
    /// <summary>
    /// Request model cho đồng bộ từ vựng
    /// </summary>
    public class VocabularySyncRequest
    {
        /// <summary>
        /// Thời gian đồng bộ lần cuối từ client
        /// </summary>
        public DateTime? LastSyncTime { get; set; }

        /// <summary>
        /// Định danh thiết bị thực hiện đồng bộ
        /// </summary>
        [StringLength(100)]
        public string DeviceId { get; set; }

        /// <summary>
        /// Phiên bản ứng dụng client
        /// </summary>
        [StringLength(20)]
        public string AppVersion { get; set; }

        /// <summary>
        /// Danh sách từ vựng cần đồng bộ từ client lên server
        /// </summary>
        public List<Models.Learning.Vocabulary.Vocabulary> Items { get; set; } = new List<Models.Learning.Vocabulary.Vocabulary>();

        /// <summary>
        /// Danh sách ID từ vựng đã xóa trên client
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