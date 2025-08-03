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

namespace LexiFlow.Models.Sync
{
    /// <summary>
    /// Metadata đồng bộ hóa cho client
    /// </summary>
    [Index(nameof(UserId), nameof(DeviceId), IsUnique = true, Name = "IX_SyncMetadata_User_Device")]
    public class SyncMetadata : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string DeviceId { get; set; }

        [Required]
        public DateTime LastSyncTime { get; set; }

        [StringLength(50)]
        public string ClientVersion { get; set; }

        [StringLength(50)]
        public string DeviceType { get; set; } // Mobile, Tablet, Desktop

        [StringLength(50)]
        public string DeviceModel { get; set; }

        [StringLength(50)]
        public string DeviceOS { get; set; }

        [StringLength(50)]
        public string DeviceOSVersion { get; set; }

        [StringLength(50)]
        public string AppVersion { get; set; }

        // Cải tiến: Thông tin trạng thái đồng bộ
        [StringLength(50)]
        public string SyncStatus { get; set; } // Success, Partial, Failed

        // Cải tiến: Thống kê đồng bộ
        public int ItemsSynced { get; set; } = 0;
        public int ItemsReceived { get; set; } = 0;
        public int ItemsSent { get; set; } = 0;
        public int ConflictsDetected { get; set; } = 0;
        public int ConflictsResolved { get; set; } = 0;

        // Cải tiến: Thông tin kết nối
        [StringLength(50)]
        public string ConnectionType { get; set; } // WiFi, Cellular, Unknown

        public bool IsConnected { get; set; } = true;

        // Cải tiến: Cấu hình đồng bộ
        [StringLength(50)]
        public string SyncMode { get; set; } // Full, Incremental, DeltaOnly

        [StringLength(50)]
        public string SyncScope { get; set; } // All, UserData, Settings, etc.

        // Cải tiến: Thời gian và hiệu suất
        public int SyncDurationMs { get; set; } = 0;
        public int NetworkLatencyMs { get; set; } = 0;
        public long DataTransferredBytes { get; set; } = 0;

        // Cải tiến: Sao lưu và khôi phục
        public bool HasLocalBackup { get; set; } = false;
        public DateTime? LastBackupTime { get; set; }

        // Cải tiến: Bối cảnh đồng bộ
        [StringLength(50)]
        public string SyncTrigger { get; set; } // Manual, Scheduled, AppStart, etc.

        [StringLength(255)]
        public string SyncNotes { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }
    }
}
