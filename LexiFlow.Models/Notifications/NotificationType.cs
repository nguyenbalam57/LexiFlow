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
    /// Loại thông báo
    /// </summary>
    [Index(nameof(TypeName), IsUnique = true, Name = "IX_NotificationType_Name")]
    public class NotificationType : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(255)]
        public string IconPath { get; set; }

        [StringLength(20)]
        public string ColorCode { get; set; }

        // Cải tiến: Mẫu thông báo
        public string TemplateSubject { get; set; }
        public string TemplateContent { get; set; }
        public string TemplateParams { get; set; }

        // Cải tiến: Cấu hình gửi thông báo
        public bool EnableInApp { get; set; } = true;
        public bool EnableEmail { get; set; } = false;
        public bool EnablePush { get; set; } = false;
        public bool EnableSMS { get; set; } = false;

        // Cải tiến: Cấu hình hiển thị
        public int DisplayDurationSeconds { get; set; } = 0;
        public bool RequiresAcknowledgment { get; set; } = false;
        public string ActionType { get; set; }
        public string ActionUrl { get; set; }

        // Navigation properties
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
