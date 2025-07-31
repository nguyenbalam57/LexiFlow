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

namespace LexiFlow.Models.Notification
{
    /// <summary>
    /// Mức độ ưu tiên thông báo
    /// </summary>
    [Index(nameof(PriorityName), IsUnique = true, Name = "IX_NotificationPriority_Name")]
    public class NotificationPriority : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PriorityId { get; set; }

        [Required]
        [StringLength(50)]
        public string PriorityName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int? DisplayOrder { get; set; }

        [StringLength(20)]
        public string ColorCode { get; set; }

        // Cải tiến: Cấu hình hiển thị
        public bool EnableInterruption { get; set; } = false;
        public bool PersistUntilAction { get; set; } = false;
        public int? RepeatReminderMinutes { get; set; }

        // Cải tiến: Các kênh thông báo
        public bool ForceEmail { get; set; } = false;
        public bool ForcePush { get; set; } = false;
        public bool ForceSMS { get; set; } = false;

        // Navigation properties
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
