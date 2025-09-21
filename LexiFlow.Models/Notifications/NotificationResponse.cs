using LexiFlow.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Notification
{
    /// <summary>
    /// Phản hồi thông báo
    /// </summary>
    public class NotificationResponse : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResponseId { get; set; }

        [Required]
        public int RecipientId { get; set; }

        public string ResponseContent { get; set; }

        // Cải tiến: Loại phản hồi
        [StringLength(50)]
        public string ResponseType { get; set; } // Text, Button, Action

        [StringLength(50)]
        public string ResponseAction { get; set; }

        public string ResponseData { get; set; }

        // Cải tiến: Thêm thông tin
        public int? RespondedByUserId { get; set; }
        public bool IsSystemResponse { get; set; } = false;
        public bool IsPrivate { get; set; } = false;

        public DateTime ResponseTime { get; set; } = DateTime.UtcNow;

        [StringLength(255)]
        public string AttachmentUrl { get; set; }

        // Cải tiến: Liên kết với hành động
        public string ActionType { get; set; }
        public string ActionTarget { get; set; }
        public bool ActionCompleted { get; set; } = false;
        public DateTime? ActionCompletedAt { get; set; }

        // Navigation properties
        [ForeignKey("RecipientId")]
        public virtual NotificationRecipient Recipient { get; set; }

        [ForeignKey("RespondedByUserId")]
        public virtual User.User RespondedByUser { get; set; }
    }
}
