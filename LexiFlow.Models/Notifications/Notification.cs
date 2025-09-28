using LexiFlow.Models.Cores;
using LexiFlow.Models.Medias;
using LexiFlow.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Notifications
{
    /// <summary>
    /// Thông báo
    /// </summary>
    public class Notification : BaseEntity
    {
        /// <summary>
        /// Khóa chính của thông báo Tự động tăng
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }

        /// <summary>
        /// Người gửi thông báo (có thể null nếu là hệ thống)
        /// </summary>
        public int? SenderUserId { get; set; }

        /// <summary>
        /// Tiêu đề thông báo
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        /// <summary>
        /// Nội dung thông báo
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Loại thông báo
        /// </summary>
        public int? TypeId { get; set; }

        /// <summary>
        /// Mức độ ưu tiên thông báo
        /// </summary>
        public int? PriorityId { get; set; }

        // Cải tiến: Chi tiết thông báo

        /// <summary>
        /// Nội dung phong phú của thông báo
        /// </summary>
        public string RichContent { get; set; }

        /// <summary>
        /// Dữ liệu kèm theo thông báo
        /// </summary>
        public string DataPayload { get; set; }

        /// <summary>
        /// URL liên kết của thông báo
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        /// Văn bản hiển thị cho liên kết
        /// </summary>
        public string LinkText { get; set; }

        // Cải tiến: Thông tin hiển thị

        /// <summary>
        /// Sử dụng chiều rộng đầy đủ cho thông báo
        /// </summary>
        public bool UseFullWidth { get; set; } = false;

        /// <summary>
        /// Mã màu chủ đề cho thông báo
        /// </summary>
        public string ThemeColor { get; set; }

        /// <summary>
        /// Cho phép người nhận phản hồi không
        /// </summary>
        public bool AllowResponses { get; set; } = false;

        /// <summary>
        /// Ngày hết hạn của thông báo
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// URL đính kèm của thông báo
        /// </summary>
        public string AttachmentUrl { get; set; }

        /// <summary>
        /// Thông báo được tạo bởi hệ thống
        /// </summary>
        public bool IsSystemGenerated { get; set; } = false;

        // Cải tiến: Theo dõi gửi thông báo

        /// <summary>
        /// Thông báo đã lên lịch hay chưa
        /// </summary>
        public bool IsScheduled { get; set; } = false;

        /// <summary>
        /// Thời gian lên lịch gửi thông báo
        /// </summary>
        public DateTime? ScheduledFor { get; set; }

        /// <summary>
        /// Thông báo đã được gửi hay chưa
        /// </summary>
        public bool IsDelivered { get; set; } = false;

        /// <summary>
        /// Thời gian thông báo được gửi
        /// </summary>
        public DateTime? DeliveredAt { get; set; }

        // Cải tiến: Liên kết với entity

        /// <summary>
        /// Loại thực thể liên quan đến thông báo
        /// </summary>
        public string RelatedEntityType { get; set; }

        /// <summary>
        /// ID thực thể liên quan đến thông báo
        /// </summary>
        public int? RelatedEntityId { get; set; }


        // Navigation properties

        /// <summary>
        /// Người gửi thông báo
        /// </summary>
        [ForeignKey("SenderUserId")]
        public virtual User SenderUser { get; set; }


        /// <summary>
        /// Loại thông báo
        /// </summary>
        [ForeignKey("TypeId")]
        public virtual NotificationType Type { get; set; }


        /// <summary>
        /// Mức độ ưu tiên thông báo
        /// </summary>
        [ForeignKey("PriorityId")]
        public virtual NotificationPriority Priority { get; set; }

        /// <summary>
        /// Các người nhận của thông báo
        /// </summary>
        public virtual ICollection<NotificationRecipient> NotificationRecipients { get; set; }

        /// <summary>
        /// Các tệp phương tiện liên quan đến thông báo
        /// </summary>
        public virtual ICollection<MediaFile> MediaFiles { get; set; }

    }
}
