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
    /// Xung đột đồng bộ hóa dữ liệu
    /// </summary>
    [Index(nameof(UserId), nameof(EntityType), nameof(EntityId), Name = "IX_SyncConflict_User_Entity")]
    [Index(nameof(ConflictStatus), Name = "IX_SyncConflict_Status")]
    public class SyncConflict : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConflictId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string DeviceId { get; set; }

        [Required]
        [StringLength(50)]
        public string EntityType { get; set; } // Vocabulary, Kanji, Grammar, etc.

        [Required]
        public int EntityId { get; set; }

        [Required]
        [StringLength(50)]
        public string ConflictType { get; set; } // Update, Delete, Create

        [Required]
        [StringLength(50)]
        public string ConflictStatus { get; set; } // Pending, Resolved, Ignored

        [Required]
        public string ClientData { get; set; } // JSON serialized client entity

        [Required]
        public string ServerData { get; set; } // JSON serialized server entity

        public string ResolutionData { get; set; } // JSON serialized resolved entity

        [StringLength(50)]
        public string ResolutionStrategy { get; set; } // ClientWins, ServerWins, Manual, Merge

        public int? ResolvedBy { get; set; }

        public DateTime? ResolvedAt { get; set; }

        // Cải tiến: Thông tin xung đột chi tiết
        public string ConflictDetails { get; set; } // JSON data about conflict fields

        public int ConflictSeverity { get; set; } = 1; // 1-5 scale

        // Cải tiến: Lịch sử xử lý xung đột
        public string ResolutionHistory { get; set; } // JSON data with history

        public int ResolutionAttempts { get; set; } = 0;

        // Cải tiến: Bối cảnh xung đột
        [StringLength(50)]
        public string ConflictContext { get; set; } // Study, Practice, Edit, etc.

        [StringLength(50)]
        public string ConflictDetectedBy { get; set; } // Client, Server

        public DateTime ClientModifiedAt { get; set; }
        public DateTime ServerModifiedAt { get; set; }

        // Cải tiến: Tham chiếu đến thực thể liên quan
        public string RelatedEntities { get; set; } // JSON array of related entity references

        // Cải tiến: Đề xuất giải pháp
        public string ResolutionSuggestions { get; set; } // JSON array of suggestions

        // Cải tiến: Thông báo người dùng
        public bool IsUserNotified { get; set; } = false;
        public DateTime? NotifiedAt { get; set; }
        public string UserResponse { get; set; } // JSON data with user's choice

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("ResolvedBy")]
        public virtual User.User ResolvedByUser { get; set; }
    }
}
