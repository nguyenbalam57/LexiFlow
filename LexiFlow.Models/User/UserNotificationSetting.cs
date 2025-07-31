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

namespace LexiFlow.Models.User
{
    /// <summary>
    /// Cài đặt thông báo của người dùng
    /// </summary>
    [Index(nameof(UserId), IsUnique = true, Name = "IX_UserNotificationSetting_User")]
    public class UserNotificationSetting : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SettingId { get; set; }

        [Required]
        public int UserId { get; set; }

        // Cài đặt kênh thông báo
        public bool EmailNotificationsEnabled { get; set; } = true;
        public bool PushNotificationsEnabled { get; set; } = true;
        public bool InAppNotificationsEnabled { get; set; } = true;
        public bool SmsNotificationsEnabled { get; set; } = false;

        // Loại thông báo
        public bool StudyRemindersEnabled { get; set; } = true;
        public bool AchievementNotificationsEnabled { get; set; } = true;
        public bool SystemUpdatesEnabled { get; set; } = true;
        public bool NewContentNotificationsEnabled { get; set; } = true;
        public bool SocialNotificationsEnabled { get; set; } = true;

        // Tần suất thông báo
        [StringLength(50)]
        public string StudyReminderFrequency { get; set; } = "Daily"; // Daily, Weekly, None

        // Khung giờ thông báo
        public TimeSpan? QuietHoursStart { get; set; }
        public TimeSpan? QuietHoursEnd { get; set; }
        public bool QuietHoursEnabled { get; set; } = false;

        // Thời gian gửi thông báo nhắc nhở
        public TimeSpan? DailyReminderTime { get; set; } = new TimeSpan(18, 0, 0); // 6:00 PM

        // Thứ trong tuần (nếu Weekly)
        [StringLength(50)]
        public string WeeklyReminderDay { get; set; } = "Monday";

        // Cài đặt nâng cao
        public string CustomNotificationSettings { get; set; } // JSON với cài đặt tùy chỉnh

        // Email digest
        [StringLength(50)]
        public string EmailDigestFrequency { get; set; } = "Weekly"; // Daily, Weekly, None

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
