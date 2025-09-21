using LexiFlow.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Syncs
{
    // <summary>
    /// Metadata về đồng bộ của người dùng
    /// </summary>
    public class SyncMetadata
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SyncMetadataID { get; set; }

        /// <summary>
        /// ID người dùng
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Thời gian đồng bộ lần cuối
        /// </summary>
        public DateTime? LastSyncTime { get; set; }

        /// <summary>
        /// Trạng thái đồng bộ
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        /// <summary>
        /// Tổng số mục đã đồng bộ
        /// </summary>
        public int TotalItemsSynced { get; set; }

        /// <summary>
        /// Có cần đồng bộ đầy đủ không
        /// </summary>
        public bool NeedsFullSync { get; set; }

        /// <summary>
        /// Thông báo
        /// </summary>
        [StringLength(500)]
        public string Message { get; set; }

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Thời gian cập nhật
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Người dùng liên quan
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
