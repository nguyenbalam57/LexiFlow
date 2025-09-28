using LexiFlow.Models.Cores;
using LexiFlow.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Notifications
{
    /// <summary>
    /// Người nhận thông báo
    /// Chứa thông tin về người nhận của một thông báo cụ thể, bao gồm trạng thái của thông báo đối với người nhận đó.
    /// </summary>
    [Index(nameof(NotificationId), nameof(UserId), IsUnique = true, Name = "IX_NotificationRecipient_Notification_User")]
    public class NotificationRecipient : BaseEntity
    {
        /// <summary>
        /// Khóa chính của người nhận thông báo Tự động tăng
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecipientId { get; set; }

        /// <summary>
        /// ID của thông báo
        /// </summary>
        [Required]
        public int NotificationId { get; set; }

        /// <summary>
        /// ID của người dùng nhận thông báo (có thể null nếu là nhóm)
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// ID của nhóm nhận thông báo (có thể null nếu là người dùng)
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// ID của trạng thái thông báo đối với người nhận
        /// </summary>
        public int? StatusId { get; set; }

        // Cải tiến: Tracking chi tiết

        /// <summary>
        /// Xác định xem thông báo đã được giao cho người nhận hay chưa
        /// </summary>
        public bool IsDelivered { get; set; } = false;

        /// <summary>
        /// Danh sách các kênh giao hàng đã sử dụng để gửi thông báo
        /// </summary>
        public string DeliveryChannels { get; set; }

        /// <summary>
        /// Thời gian thông báo được giao cho người nhận
        /// </summary>
        public DateTime? DeliveredAt { get; set; }

        /// <summary>
        /// Xác định xem việc gửi thông báo có thất bại hay không
        /// </summary>
        public bool IsFailed { get; set; } = false;

        /// <summary>
        /// Lý do thất bại (nếu có)
        /// </summary>
        public string FailureReason { get; set; }

        // Cải tiến: Xử lý thông báo

        /// <summary>
        /// Xác định xem thông báo đã được mở bởi người nhận hay chưa
        /// </summary>
        public bool IsOpened { get; set; } = false;

        /// <summary>
        /// Xác định xem thông báo đã được nhấp bởi người nhận hay chưa
        /// </summary>
        public bool IsClicked { get; set; } = false;

        /// <summary>
        /// Xác định xem thông báo đã được thực hiện hành động bởi người nhận hay chưa
        /// </summary>
        public bool IsActioned { get; set; } = false;

        /// <summary>
        /// Thời gian người nhận thực hiện hành động
        /// </summary>
        public DateTime? ActionedAt { get; set; }

        /// <summary>
        /// Hành động đã thực hiện trên thông báo
        /// </summary>
        public string ActionTaken { get; set; }

        /// <summary>
        /// Thời gian người nhận nhận được thông báo
        /// </summary>
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Thời gian người nhận đọc thông báo
        /// </summary>
        public DateTime? ReadAt { get; set; }

        /// <summary>
        /// Xác định xem thông báo đã được lưu trữ bởi người nhận hay chưa
        /// </summary>
        public bool IsArchived { get; set; } = false;

        // Cải tiến: Tùy chỉnh hiển thị

        /// <summary>
        /// Xác định xem thông báo đã được ghim bởi người nhận hay chưa
        /// </summary>
        public bool IsPinned { get; set; } = false;

        /// <summary>
        /// Xác định xem thông báo đã được đánh dấu bởi người nhận hay chưa
        /// </summary>
        public bool IsFlagged { get; set; } = false;

        /// <summary>
        /// Nhãn tùy chỉnh cho thông báo
        /// </summary>
        public string CustomLabel { get; set; }

        // Navigation properties

        /// <summary>
        /// Thông báo liên quan đến người nhận
        /// </summary>
        [ForeignKey("NotificationId")]
        public virtual Notification Notification { get; set; }

        /// <summary>
        /// Người dùng nhận thông báo
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }


        /// <summary>
        /// Nhóm nhận thông báo
        /// </summary>
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }


        /// <summary>
        /// Trạng thái thông báo đối với người nhận
        /// </summary>
        [ForeignKey("StatusId")]
        public virtual NotificationStatus Status { get; set; }

        /// <summary>
        /// Các phản hồi liên quan đến người nhận thông báo
        /// </summary>
        public virtual ICollection<NotificationResponse> NotificationResponses { get; set; }
    }
}
