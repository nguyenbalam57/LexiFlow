using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LexiFlow.Models.Core;
using LexiFlow.Models.Gamification;
using LexiFlow.Models.Media;
using LexiFlow.Models.Progress;
using LexiFlow.Models.User.UserRelations;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.Models.User
{
    /// <summary>
    /// Mô hình User đã tối ưu
    /// </summary>
    [Index(nameof(Username), IsUnique = true, Name = "IX_User_Username")]
    [Index(nameof(Email), Name = "IX_User_Email")]
    public class User : SoftDeletableEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public int? DepartmentId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        // Cải tiến: Thêm refreshToken cho authentication
        [StringLength(255)]
        public string RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        public bool IsActive { get; set; } = true;

        // Cải tiến: Hỗ trợ xác thực 2 yếu tố
        public bool IsMfaEnabled { get; set; } = false;

        [StringLength(100)]
        public string MfaKey { get; set; }

        // Cải tiến: Theo dõi thông tin cuối cùng đăng nhập
        public DateTime? LastLoginAt { get; set; }

        [StringLength(45)]
        public string LastLoginIP { get; set; }

        // Cải tiến: Cài đặt người dùng
        [StringLength(10)]
        public string PreferredLanguage { get; set; } = "vi";

        [StringLength(50)]
        public string TimeZone { get; set; } = "Asia/Ho_Chi_Minh";

        public int? DailyGoalMinutes { get; set; } = 30;

        // Navigation properties
        public virtual UserProfile Profile { get; set; }

        public virtual UserLearningPreference LearningPreference { get; set; }
        
        public virtual UserNotificationSetting NotificationSetting { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<MediaFile> MediaFiles { get; set; }
        public virtual ICollection<LearningProgress> LearningProgresses { get; set; }
        public virtual ICollection<LearningSession> LearningSessions { get; set; }
        public virtual ICollection<UserBadge> Badges { get; set; }
    }
}
