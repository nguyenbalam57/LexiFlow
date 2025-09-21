using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LexiFlow.Models.Cores;
using LexiFlow.Models.Medias;
using LexiFlow.Models.Progress;
using LexiFlow.Models.Users.UserRelations;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.Models.Users
{
    /// <summary>
    /// Mô hình User đã tối ưu - Lưu trữ thông tin người dùng trong hệ thống LexiFlow
    /// Bao gồm thông tin xác thực, cài đặt cá nhân và các liên kết với các module khác
    /// </summary>
    [Index(nameof(Username), IsUnique = true, Name = "IX_User_Username")]
    [Index(nameof(Email), Name = "IX_User_Email")]
    public class User : AuditableEntity
    {
        /// <summary>
        /// ID duy nhất của người dùng (Primary Key)
        /// Được tự động tạo bởi database
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        /// <summary>
        /// ID phòng ban mà người dùng thuộc về (có thể null)
        /// Dùng để phân quyền theo phòng ban và theo dõi tiến độ nhóm
        /// </summary>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// Tên đăng nhập duy nhất của người dùng
        /// Được sử dụng để đăng nhập vào hệ thống
        /// Tối đa 50 ký tự, bắt buộc và không được trùng lặp
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        /// <summary>
        /// Mật khẩu đã được mã hóa (hash) của người dùng
        /// Không bao giờ lưu trữ mật khẩu dạng plain text
        /// Tối đa 255 ký tự, bắt buộc
        /// </summary>
        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Địa chỉ email của người dùng
        /// Sử dụng cho thông báo, khôi phục mật khẩu và liên lạc
        /// Tối đa 255 ký tự, có thể null
        /// </summary>
        [StringLength(255)]
        public string Email { get; set; }

        /// <summary>
        /// Token làm mới phiên đăng nhập
        /// Sử dụng cho việc gia hạn token JWT mà không cần đăng nhập lại
        /// Tối đa 255 ký tự
        /// </summary>
        [StringLength(255)]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Thời gian hết hạn của refresh token
        /// Sau thời gian này, người dùng phải đăng nhập lại
        /// </summary>
        public DateTime? RefreshTokenExpiryTime { get; set; }

        /// <summary>
        /// Thời gian đăng nhập cuối cùng
        /// Theo dõi hoạt động của người dùng và bảo mật tài khoản
        /// </summary>
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// Địa chỉ IP của lần đăng nhập cuối cùng
        /// Sử dụng để phát hiện hoạt động bất thường và bảo mật
        /// Tối đa 45 ký tự (hỗ trợ IPv6)
        /// </summary>
        [StringLength(45)]
        public string LastLoginIP { get; set; }

        /// <summary>
        /// Ngôn ngữ ưa thích của người dùng
        /// Xác định ngôn ngữ hiển thị giao diện và nội dung
        /// Mặc định là "vi" (tiếng Việt), tối đa 10 ký tự
        /// </summary>
        [StringLength(10)]
        public string PreferredLanguage { get; set; } = "vi";

        /// <summary>
        /// Múi giờ của người dùng
        /// Sử dụng để hiển thị thời gian chính xác theo vị trí địa lý
        /// Mặc định là "Asia/Ho_Chi_Minh", tối đa 50 ký tự
        /// </summary>
        [StringLength(50)]
        public string TimeZone { get; set; } = "Asia/Ho_Chi_Minh";

        /// <summary>
        /// Mục tiêu học tập hàng ngày (tính bằng phút)
        /// Người dùng đặt mục tiêu thời gian học mỗi ngày
        /// Mặc định là 30 phút, có thể null nếu không đặt mục tiêu
        /// </summary>
        public int? DailyGoalMinutes { get; set; } = 30;

        // Navigation properties - Các mối quan hệ với bảng khác

        /// <summary>
        /// Thông tin hồ sơ chi tiết của người dùng
        /// Bao gồm họ tên, avatar, thông tin cá nhân khác
        /// Quan hệ 1-1 với bảng UserProfile
        /// </summary>
        public virtual UserProfile Profile { get; set; }

        /// <summary>
        /// Thiết lập học tập cá nhân của người dùng
        /// Bao gồm phương pháp học ưa thích, cấp độ, mục tiêu
        /// Quan hệ 1-1 với bảng UserLearningPreference
        /// </summary>
        public virtual UserLearningPreference LearningPreference { get; set; }

        /// <summary>
        /// Cài đặt thông báo của người dùng
        /// Xác định loại thông báo nào sẽ được gửi và qua kênh nào
        /// Quan hệ 1-1 với bảng UserNotificationSetting
        /// </summary>
        public virtual UserNotificationSetting NotificationSetting { get; set; }

        /// <summary>
        /// Phòng ban mà người dùng thuộc về
        /// Sử dụng cho phân quyền và quản lý theo tổ chức
        /// Quan hệ nhiều-1 với bảng Department
        /// </summary>
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        /// <summary>
        /// Danh sách các vai trò (roles) được gán cho người dùng
        /// Một người dùng có thể có nhiều vai trò khác nhau
        /// Quan hệ nhiều-nhiều với bảng Role thông qua UserRole
        /// </summary>
        public virtual ICollection<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Danh sách các file media mà người dùng đã tải lên
        /// Bao gồm avatar, file âm thanh, hình ảnh liên quan đến học tập
        /// Quan hệ 1-nhiều với bảng MediaFile
        /// </summary>
        public virtual ICollection<MediaFile> MediaFiles { get; set; }

        /// <summary>
        /// Danh sách tiến độ học tập của người dùng
        /// Theo dõi quá trình học các từ vựng, kanji, ngữ pháp
        /// Quan hệ 1-nhiều với bảng LearningProgress
        /// </summary>
        public virtual ICollection<LearningProgress> LearningProgresses { get; set; }
    }
}