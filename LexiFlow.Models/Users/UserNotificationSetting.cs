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

namespace LexiFlow.Models.Users
{
    /// <summary>
    /// Model đại diện cho cài đặt thông báo của người dùng trong hệ thống LexiFlow.
    /// Cho phép người dùng tùy chỉnh các loại thông báo, kênh nhận thông báo và thời gian nhận thông báo.
    /// </summary>
    /// <remarks>
    /// Entity này quản lý toàn bộ tùy chọn thông báo bao gồm:
    /// - Các kênh thông báo (Email, Push, In-app, SMS)
    /// - Loại thông báo (Học tập, Thành tích, Cập nhật hệ thống, v.v.)
    /// - Thời gian và tần suất thông báo
    /// - Chế độ yên lặng và cài đặt nâng cao
    /// </remarks>
    [Index(nameof(UserId), IsUnique = true, Name = "IX_UserNotificationSetting_User")]
    public class UserNotificationSetting : BaseEntity
    {
        /// <summary>
        /// Khóa chính của bảng cài đặt thông báo
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SettingId { get; set; }

        /// <summary>
        /// ID của người dùng sở hữu cài đặt thông báo này
        /// </summary>
        /// <remarks>
        /// Khóa ngoại tham chiếu đến bảng User. Mỗi user chỉ có một bản ghi cài đặt thông báo.
        /// </remarks>
        [Required]
        public int UserId { get; set; }

        #region Cài đặt kênh thông báo

        /// <summary>
        /// Cho phép gửi thông báo qua email
        /// </summary>
        /// <value>True nếu bật thông báo email, False nếu tắt. Mặc định: true</value>
        public bool EmailNotificationsEnabled { get; set; } = true;

        /// <summary>
        /// Cho phép gửi thông báo đẩy (push notification) đến thiết bị di động
        /// </summary>
        /// <value>True nếu bật push notification, False nếu tắt. Mặc định: true</value>
        public bool PushNotificationsEnabled { get; set; } = true;

        /// <summary>
        /// Cho phép hiển thị thông báo trong ứng dụng
        /// </summary>
        /// <value>True nếu bật thông báo in-app, False nếu tắt. Mặc định: true</value>
        public bool InAppNotificationsEnabled { get; set; } = true;

        /// <summary>
        /// Cho phép gửi thông báo qua tin nhắn SMS
        /// </summary>
        /// <value>True nếu bật SMS, False nếu tắt. Mặc định: false</value>
        public bool SmsNotificationsEnabled { get; set; } = false;

        #endregion

        #region Loại thông báo

        /// <summary>
        /// Cho phép nhận thông báo nhắc nhở học tập
        /// </summary>
        /// <value>True nếu bật nhắc nhở học tập, False nếu tắt. Mặc định: true</value>
        public bool StudyRemindersEnabled { get; set; } = true;

        /// <summary>
        /// Cho phép nhận thông báo về thành tích và huy hiệu đạt được
        /// </summary>
        /// <value>True nếu bật thông báo thành tích, False nếu tắt. Mặc định: true</value>
        public bool AchievementNotificationsEnabled { get; set; } = true;

        /// <summary>
        /// Cho phép nhận thông báo về cập nhật hệ thống và tính năng mới
        /// </summary>
        /// <value>True nếu bật thông báo cập nhật, False nếu tắt. Mặc định: true</value>
        public bool SystemUpdatesEnabled { get; set; } = true;

        /// <summary>
        /// Cho phép nhận thông báo về nội dung học tập mới
        /// </summary>
        /// <value>True nếu bật thông báo nội dung mới, False nếu tắt. Mặc định: true</value>
        public bool NewContentNotificationsEnabled { get; set; } = true;

        /// <summary>
        /// Cho phép nhận thông báo về hoạt động xã hội (bình luận, thích, theo dõi)
        /// </summary>
        /// <value>True nếu bật thông báo xã hội, False nếu tắt. Mặc định: true</value>
        public bool SocialNotificationsEnabled { get; set; } = true;

        #endregion

        #region Tần suất thông báo

        /// <summary>
        /// Tần suất gửi thông báo nhắc nhở học tập
        /// </summary>
        /// <value>
        /// Các giá trị hợp lệ:
        /// - "Daily": Hàng ngày
        /// - "Weekly": Hàng tuần  
        /// - "None": Không nhắc nhở
        /// Mặc định: "Daily"
        /// </value>
        [StringLength(50)]
        public string StudyReminderFrequency { get; set; } = "Daily";

        #endregion

        #region Khung giờ thông báo

        /// <summary>
        /// Thời điểm bắt đầu khung giờ yên lặng (không gửi thông báo)
        /// </summary>
        /// <value>Thời gian bắt đầu giờ yên lặng. Null nếu không sử dụng</value>
        public TimeSpan? QuietHoursStart { get; set; }

        /// <summary>
        /// Thời điểm kết thúc khung giờ yên lặng
        /// </summary>
        /// <value>Thời gian kết thúc giờ yên lặng. Null nếu không sử dụng</value>
        public TimeSpan? QuietHoursEnd { get; set; }

        /// <summary>
        /// Bật/tắt chế độ giờ yên lặng
        /// </summary>
        /// <value>True nếu bật giờ yên lặng, False nếu tắt. Mặc định: false</value>
        public bool QuietHoursEnabled { get; set; } = false;

        #endregion

        #region Thời gian nhắc nhở

        /// <summary>
        /// Thời gian trong ngày để gửi thông báo nhắc nhở học tập hàng ngày
        /// </summary>
        /// <value>Thời gian nhắc nhở. Mặc định: 18:00 (6:00 PM)</value>
        public TimeSpan? DailyReminderTime { get; set; } = new TimeSpan(18, 0, 0);

        /// <summary>
        /// Thứ trong tuần để gửi thông báo nhắc nhở (khi chọn tần suất Weekly)
        /// </summary>
        /// <value>
        /// Tên thứ bằng tiếng Anh: Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
        /// Mặc định: "Monday"
        /// </value>
        [StringLength(50)]
        public string WeeklyReminderDay { get; set; } = "Monday";

        #endregion

        #region Cài đặt nâng cao

        /// <summary>
        /// Chuỗi JSON chứa các cài đặt thông báo tùy chỉnh nâng cao
        /// </summary>
        /// <value>
        /// Dữ liệu JSON có thể chứa:
        /// - Cài đặt thông báo cho từng loại nội dung cụ thể
        /// - Ngưỡng điểm số để thông báo
        /// - Cài đặt âm thanh và rung cho từng loại thông báo
        /// </value>
        public string CustomNotificationSettings { get; set; }

        #endregion

        #region Email digest

        /// <summary>
        /// Tần suất gửi email tổng hợp (digest) các hoạt động và thông báo
        /// </summary>
        /// <value>
        /// Các giá trị hợp lệ:
        /// - "Daily": Hàng ngày
        /// - "Weekly": Hàng tuần
        /// - "None": Không gửi email tổng hợp
        /// Mặc định: "Weekly"
        /// </value>
        [StringLength(50)]
        public string EmailDigestFrequency { get; set; } = "Weekly";

        #endregion

        #region Navigation properties

        /// <summary>
        /// Thông tin người dùng sở hữu cài đặt thông báo này
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        #endregion
    }
}