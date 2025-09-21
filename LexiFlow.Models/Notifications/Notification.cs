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
    /// Thông báo
    /// </summary>
    public class Notification : BaseEntity, ISoftDeletable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }

        public int? SenderUserId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string Content { get; set; }

        public int? TypeId { get; set; }

        public int? PriorityId { get; set; }

        // Cải tiến: Chi tiết thông báo
        public string RichContent { get; set; }
        public string DataPayload { get; set; }
        public string LinkUrl { get; set; }
        public string LinkText { get; set; }

        // Cải tiến: Thông tin hiển thị
        [StringLength(255)]
        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public bool UseFullWidth { get; set; } = false;
        public string ThemeColor { get; set; }

        public bool AllowResponses { get; set; } = false;

        public DateTime? ExpirationDate { get; set; }

        [StringLength(255)]
        public string AttachmentUrl { get; set; }

        public bool IsSystemGenerated { get; set; } = false;

        // Cải tiến: Theo dõi gửi thông báo
        public bool IsScheduled { get; set; } = false;
        public DateTime? ScheduledFor { get; set; }
        public bool IsDelivered { get; set; } = false;
        public DateTime? DeliveredAt { get; set; }

        // Cải tiến: Liên kết với entity
        public string RelatedEntityType { get; set; }
        public int? RelatedEntityId { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        public int? DeletedBy { get; set; }

        // Navigation properties
        [ForeignKey("SenderUserId")]
        public virtual User.User SenderUser { get; set; }

        [ForeignKey("TypeId")]
        public virtual NotificationType Type { get; set; }

        [ForeignKey("PriorityId")]
        public virtual NotificationPriority Priority { get; set; }

        [ForeignKey("DeletedBy")]
        public virtual User.User DeletedByUser { get; set; }

        public virtual ICollection<NotificationRecipient> NotificationRecipients { get; set; }

    }
}
