using LexiFlow.Core.Services;
using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Sync
{
    /// <summary>
    /// Request model cho giải quyết xung đột đồng bộ
    /// </summary>
    public class SyncConflictResolutionRequest
    {
        /// <summary>
        /// Định danh thiết bị thực hiện đồng bộ
        /// </summary>
        [Required]
        [StringLength(100)]
        public string DeviceId { get; set; }

        /// <summary>
        /// Danh sách các quyết định giải quyết xung đột
        /// </summary>
        [Required]
        public List<ConflictResolutionItem> Resolutions { get; set; } = new List<ConflictResolutionItem>();
    }
}
