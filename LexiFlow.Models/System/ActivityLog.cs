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
    /// Nhật ký hoạt động
    /// </summary>
    [Index(nameof(UserId), nameof(Timestamp), Name = "IX_ActivityLog_User_Time")]
    [Index(nameof(Module), nameof(Action), Name = "IX_ActivityLog_Module_Action")]
    public class ActivityLog : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }

        public int? UserId { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string Action { get; set; }

        [StringLength(50)]
        public string Module { get; set; }

        public string Details { get; set; }

        [StringLength(45)]
        public string IPAddress { get; set; }

        // Cải tiến: Thông tin bổ sung
        [StringLength(255)]
        public string UserAgent { get; set; }

        [StringLength(50)]
        public string Platform { get; set; }

        [StringLength(100)]
        public string Location { get; set; }

        // Cải tiến: Entity liên quan
        [StringLength(50)]
        public string EntityType { get; set; }

        public int? EntityId { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        // Cải tiến: Mức độ hoạt động
        [StringLength(20)]
        public string Severity { get; set; } // Info, Warning, Error, Critical

        public bool IsSystem { get; set; } = false;

        public bool RequiresAttention { get; set; } = false;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }
    }
}
