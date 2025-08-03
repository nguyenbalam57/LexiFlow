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
    /// Theo dõi các mục đã xóa cho đồng bộ hóa
    /// </summary>
    [Index(nameof(EntityType), nameof(EntityId), IsUnique = true, Name = "IX_DeletedItem_Entity")]
    [Index(nameof(DeletedAt), Name = "IX_DeletedItem_DeletedAt")]
    public class DeletedItem : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string EntityType { get; set; } // Vocabulary, Kanji, Grammar, etc.

        [Required]
        public int EntityId { get; set; }

        [Required]
        public DateTime DeletedAt { get; set; }

        public int? UserID { get; set; }

        // Cải tiến: Lý do xóa
        [StringLength(255)]
        public string DeletionReason { get; set; }

        // Cải tiến: Dữ liệu sao lưu
        public string BackupData { get; set; } // JSON serialized entity before deletion

        // Cải tiến: Thông tin bối cảnh xóa
        [StringLength(50)]
        public string DeletionContext { get; set; } // UserAction, Sync, Cleanup, etc.

        [StringLength(100)]
        public string DeviceId { get; set; }

        [StringLength(50)]
        public string AppVersion { get; set; }

        // Cải tiến: Chính sách lưu giữ
        public bool IsPermanentDeletion { get; set; } = false;
        public DateTime? RetentionExpiry { get; set; }
        public bool IsArchived { get; set; } = false;

        // Cải tiến: Phân loại theo mức độ ưu tiên
        [StringLength(50)]
        public string DeletedItemType { get; set; } // UserData, SystemData, etc.

        public int Priority { get; set; } = 1; // 1-5 scale

        // Cải tiến: Trạng thái đồng bộ
        public bool IsSyncedToClients { get; set; } = false;
        public DateTime? SyncedAt { get; set; }
        public int SyncCount { get; set; } = 0;

        // Cải tiến: Phục hồi
        public bool IsRestored { get; set; } = false;
        public DateTime? RestoredAt { get; set; }
        public int? RestoredBy { get; set; }
        public int? RestoredToId { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User.User User { get; set; }

        [ForeignKey("RestoredBy")]
        public virtual User.User RestoredByUser { get; set; }
    }
}
