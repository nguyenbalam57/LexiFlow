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
    /// Người nhận thông báo
    /// </summary>
    [Index(nameof(NotificationId), nameof(UserId), IsUnique = true, Name = "IX_NotificationRecipient_Notification_User")]
    public class NotificationRecipient : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecipientId { get; set; }

        [Required]
        public int NotificationId { get; set; }

        public int? UserId { get; set; }

        public int? GroupId { get; set; }

        public int? StatusId { get; set; }

        // Cải tiến: Tracking chi tiết
        public bool IsDelivered { get; set; } = false;
        public string DeliveryChannels { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public bool IsFailed { get; set; } = false;
        public string FailureReason { get; set; }

        // Cải tiến: Xử lý thông báo
        public bool IsOpened { get; set; } = false;
        public bool IsClicked { get; set; } = false;
        public bool IsActioned { get; set; } = false;
        public DateTime? ActionedAt { get; set; }
        public string ActionTaken { get; set; }

        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ReadAt { get; set; }

        public bool IsArchived { get; set; } = false;

        // Cải tiến: Tùy chỉnh hiển thị
        public bool IsPinned { get; set; } = false;
        public bool IsFlagged { get; set; } = false;
        public string CustomLabel { get; set; }
        public int? DisplayOrder { get; set; }

        // Navigation properties
        [ForeignKey("NotificationId")]
        public virtual Notification Notification { get; set; }

        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("GroupId")]
        public virtual User.Group Group { get; set; }

        [ForeignKey("StatusId")]
        public virtual NotificationStatus Status { get; set; }

        public virtual ICollection<NotificationResponse> NotificationResponses { get; set; }
    }
}
