using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.API.DTOs.Sync
{
    /// <summary>
    /// Thông tin về xung đột đồng bộ
    /// </summary>
    public class SyncConflict
    {
        /// <summary>
        /// ID xung đột
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConflictID { get; set; }

        /// <summary>
        /// ID người dùng
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Loại entity
        /// </summary>
        [Required]
        [StringLength(50)]
        public string EntityType { get; set; }

        /// <summary>
        /// ID entity
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// Dữ liệu phiên bản client
        /// </summary>
        public string ClientData { get; set; }

        /// <summary>
        /// Dữ liệu phiên bản server
        /// </summary>
        public string ServerData { get; set; }

        /// <summary>
        /// Thời gian cập nhật của client
        /// </summary>
        public DateTime ClientUpdateTime { get; set; }

        /// <summary>
        /// Thời gian cập nhật của server
        /// </summary>
        public DateTime ServerUpdateTime { get; set; }

        /// <summary>
        /// Loại xung đột
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ConflictType { get; set; }

        /// <summary>
        /// Đã giải quyết chưa
        /// </summary>
        public bool IsResolved { get; set; }

        /// <summary>
        /// Thời gian giải quyết
        /// </summary>
        public DateTime? ResolvedAt { get; set; }

        /// <summary>
        /// Phương thức giải quyết
        /// </summary>
        [StringLength(50)]
        public string ResolutionMethod { get; set; }

        /// <summary>
        /// Thời gian phát hiện
        /// </summary>
        public DateTime DetectedAt { get; set; }

        /// <summary>
        /// Người dùng liên quan
        /// </summary>
        //[ForeignKey("UserID")]
        //public virtual Models.User User { get; set; }
    }
}
