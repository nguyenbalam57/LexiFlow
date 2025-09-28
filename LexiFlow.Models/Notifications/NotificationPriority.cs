using LexiFlow.Models.Cores;
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
    /// Mức độ ưu tiên thông báo
    /// Mức độ ưu tiên của thông báo dựa vào số nguyên, với giá trị thấp hơn biểu thị mức độ ưu tiên cao hơn.
    /// Ví dụ: 1 - Cao, 2 - Trung bình, 3 - Thấp
    /// Mức độ ưu tiên này có thể ảnh hưởng đến cách thức và thời điểm thông báo được gửi đến người nhận.
    /// </summary>
    [Index(nameof(PriorityName), IsUnique = true, Name = "IX_NotificationPriority_Name")]
    public class NotificationPriority : BaseEntity
    {
        /// <summary>
        /// Khóa chính của mức độ ưu tiên thông báo Tự động tăng
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PriorityId { get; set; }

        /// <summary>
        /// Tên mức độ ưu tiên thông báo
        /// </summary>
        [Required]
        [StringLength(50)]
        public string PriorityName { get; set; }

        /// <summary>
        /// Mô tả mức độ ưu tiên thông báo
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Mã màu hiển thị cho mức độ ưu tiên thông báo
        /// </summary>
        [StringLength(20)]
        public string ColorCode { get; set; }

        // Cải tiến: Cấu hình hiển thị

        /// <summary>
        /// Cho phép ngắt quãng thông báo
        /// </summary>
        public bool EnableInterruption { get; set; } = false;

        /// <summary>
        /// Giữ thông báo cho đến khi có hành động
        /// </summary>
        public bool PersistUntilAction { get; set; } = false;

        /// <summary>
        /// Thời gian lặp lại nhắc nhở (phút)
        /// </summary>
        public int? RepeatReminderMinutes { get; set; }

        // Cải tiến: Các kênh thông báo

        /// <summary>
        /// Bắt buộc hiển thị thông báo dưới dạng thông báo đẩy
        /// </summary>
        public bool ForcePush { get; set; } = false;

        // Navigation properties

        /// <summary>
        /// Các thông báo liên quan đến mức độ ưu tiên này
        /// </summary>
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
