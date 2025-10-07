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

namespace LexiFlow.Models.Schedulings
{
    /// <summary>
    /// Nhắc nhở lịch trình - Quản lý các nhắc nhở cho các mục lịch trình
    /// </summary>
    public class ScheduleReminder : BaseEntity
    {
        /// <summary>
        /// Mã định danh duy nhất cho nhắc nhở
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReminderId { get; set; }

        /// <summary>
        /// Mã định danh của mục lịch trình cần nhắc nhở
        /// </summary>
        [Required]
        public int ItemId { get; set; }

        /// <summary>
        /// Mã định danh người dùng nhận nhắc nhở (có thể null nếu nhắc nhở chung)
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Thời gian nhắc nhở trước khi sự kiện diễn ra (số lượng)
        /// </summary>
        public int? ReminderTime { get; set; }

        /// <summary>
        /// Đơn vị thời gian cho nhắc nhở (phút, giờ, ngày, tuần)
        /// </summary>
        [StringLength(20)]
        public string ReminderUnit { get; set; }

        // Cải tiến: Kênh gửi nhắc nhở
        /// <summary>
        /// Cho phép gửi nhắc nhở qua email
        /// </summary>
        public bool IsEmailReminder { get; set; } = false;

        /// <summary>
        /// Cho phép hiển thị nhắc nhở dạng popup (mặc định bật)
        /// </summary>
        public bool IsPopupReminder { get; set; } = true;

        /// <summary>
        /// Cho phép gửi nhắc nhở qua push notification
        /// </summary>
        public bool IsPushReminder { get; set; } = false;

        // Cải tiến: Chi tiết gửi
        /// <summary>
        /// Nội dung tùy chỉnh cho thông báo nhắc nhở
        /// </summary>
        [StringLength(255)]
        public string ReminderMessage { get; set; }

        /// <summary>
        /// Âm thanh tùy chỉnh khi nhắc nhở (đường dẫn hoặc tên file)
        /// </summary>
        public int? MediaFileId { get; set; }

        /// <summary>
        /// Thông tin tệp phương tiện cho nhắc nhở
        /// </summary>
        [ForeignKey("MediaFileId")]
        public virtual MediaFile MediaFile { get; set; }

        /// <summary>
        /// Xác định nhắc nhở có lặp lại hay không
        /// </summary>
        public bool IsRepeating { get; set; } = false;

        /// <summary>
        /// Khoảng thời gian lặp lại nhắc nhở (tính bằng phút)
        /// </summary>
        public int? RepeatIntervalMinutes { get; set; }

        /// <summary>
        /// Trạng thái đã gửi nhắc nhở hay chưa
        /// </summary>
        public bool IsSent { get; set; } = false;

        /// <summary>
        /// Thời điểm gửi nhắc nhở thực tế
        /// </summary>
        public DateTime? SentAt { get; set; }

        // Cải tiến: Theo dõi tương tác
        /// <summary>
        /// Trạng thái người dùng đã xác nhận nhận nhắc nhở hay chưa
        /// </summary>
        public bool IsAcknowledged { get; set; } = false;

        /// <summary>
        /// Thời điểm người dùng xác nhận nhắc nhở
        /// </summary>
        public DateTime? AcknowledgedAt { get; set; }

        /// <summary>
        /// Hành động người dùng thực hiện khi xác nhận (đồng ý, hoãn, hủy, v.v.)
        /// </summary>
        public string AcknowledgementAction { get; set; }

        // Navigation properties
        /// <summary>
        /// Liên kết tới mục lịch trình được nhắc nhở
        /// </summary>
        [ForeignKey("ItemId")]
        public virtual ScheduleItem Item { get; set; }

        /// <summary>
        /// Liên kết tới người dùng nhận nhắc nhở
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
