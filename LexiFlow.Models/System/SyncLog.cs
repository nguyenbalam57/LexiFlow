using LexiFlow.Models.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.System
{
    /// <summary>
    /// Nhật ký đồng bộ
    /// </summary>
    [Index(nameof(UserId), nameof(TableName), nameof(LastSyncAt), Name = "IX_SyncLog_User_Table_Time")]
    public class SyncLog : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SyncId { get; set; }

        [Required]
        public int UserId { get; set; }

        [StringLength(100)]
        public string TableName { get; set; }

        public DateTime? LastSyncAt { get; set; }

        public int? RecordsSynced { get; set; }

        [StringLength(20)]
        public string SyncDirection { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        // Cải tiến: Thông tin chi tiết
        public string RecordIds { get; set; }
        public int? RecordsCreated { get; set; }
        public int? RecordsUpdated { get; set; }
        public int? RecordsDeleted { get; set; }
        public int? RecordsSkipped { get; set; }

        // Cải tiến: Thông tin thiết bị
        [StringLength(100)]
        public string DeviceId { get; set; }
        [StringLength(50)]
        public string DeviceType { get; set; }
        [StringLength(50)]
        public string AppVersion { get; set; }

        // Cải tiến: Lỗi và cảnh báo
        public int? ErrorCount { get; set; } = 0;
        public int? WarningCount { get; set; } = 0;

        public string ErrorMessage { get; set; }

        // Cải tiến: Xử lý xung đột
        public int? ConflictCount { get; set; } = 0;
        public string ConflictResolution { get; set; }
        public string ConflictDetails { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }
    }
}
