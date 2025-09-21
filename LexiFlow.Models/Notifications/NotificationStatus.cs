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
    /// Trạng thái thông báo
    /// </summary>
    [Index(nameof(StatusName), IsUnique = true, Name = "IX_NotificationStatus_Name")]
    public class NotificationStatus : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusId { get; set; }

        [Required]
        [StringLength(50)]
        public string StatusName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(20)]
        public string ColorCode { get; set; }

        // Cải tiến: Cấu hình trạng thái
        public bool IsTerminal { get; set; } = false;
        public bool RequiresAction { get; set; } = false;
        public bool IsDefault { get; set; } = false;
        public int DisplayOrder { get; set; } = 0;

        // Cải tiến: Chuyển đổi trạng thái
        public string AllowedTransitions { get; set; }
        public string TransitionActions { get; set; }

        // Navigation properties
        public virtual ICollection<NotificationRecipient> NotificationRecipients { get; set; }
    }
}
