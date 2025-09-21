using LexiFlow.Models.Cores;
using LexiFlow.Models.Medias;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.Models.Users
{
    /// <summary>
    /// Thông tin profile người dùng - lưu trữ thông tin cá nhân và cài đặt hiển thị
    /// Mở rộng thông tin cơ bản của User với các chi tiết về profile và preferences
    /// </summary>
    public class UserProfile : AuditableEntity
    {
        /// <summary>
        /// ID người dùng (khóa chính và khóa ngoại đến bảng User)
        /// </summary>
        [Key]
        public int UserId { get; set; }

        /// <summary>
        /// Tên của người dùng
        /// </summary>
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        /// <summary>
        /// Tên hiển thị công khai (có thể khác với tên thật)
        /// </summary>
        [StringLength(150)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Biệt danh/nickname
        /// </summary>
        [StringLength(50)]
        public string Nickname { get; set; }

        /// <summary>
        /// Giới thiệu bản thân / mô tả profile
        /// </summary>
        [StringLength(1000)]
        public string Biography { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Giới tính (Male, Female, Other, PreferNotToSay)
        /// </summary>
        [StringLength(20)]
        public string Gender { get; set; }

        /// <summary>
        /// Vị trí công việc/chức danh
        /// </summary>
        [StringLength(100)]
        public string JobTitle { get; set; }

        /// <summary>
        /// ID của file media avatar
        /// Trong database của MediaFile thì các mục id khác sẽ không được có thông tin
        /// </summary>
        public int? AvatarMediaId { get; set; }

        /// <summary>
        /// URL avatar dự phòng (nếu không có file media)
        /// </summary>
        [StringLength(500)]
        public string AvatarUrl { get; set; }

        /// <summary>
        /// Profile công khai (có thể xem bởi người khác)
        /// </summary>
        public bool IsPublicProfile { get; set; } = false;

        /// <summary>
        /// Hiển thị thống kê học tập trên profile
        /// </summary>
        public bool ShowLearningStats { get; set; } = true;

        /// <summary>
        /// Hiển thị badges/huy hiệu trên profile
        /// </summary>
        public bool ShowBadges { get; set; } = true;

        /// <summary>
        /// Hiển thị thành tích trên profile
        /// </summary>
        public bool ShowAchievements { get; set; } = true;

        /// <summary>
        /// Hiển thị hoạt động gần đây
        /// </summary>
        public bool ShowRecentActivity { get; set; } = true;

        /// <summary>
        /// Hiển thị thông tin liên hệ
        /// </summary>
        public bool ShowContactInfo { get; set; } = false;

        /// <summary>
        /// Cho phép người khác gửi tin nhắn
        /// </summary>
        public bool AllowMessages { get; set; } = true;

        /// <summary>
        /// Màu chủ đề ưa thích
        /// </summary>
        [StringLength(20)]
        public string PreferredThemeColor { get; set; } = "#2196F3";

        /// <summary>
        /// Chế độ tối/sáng (Light, Dark, Auto)
        /// </summary>
        [StringLength(20)]
        public string ThemeMode { get; set; } = "Auto";

        /// <summary>
        /// Mức độ học tập hiện tại (Beginner, Intermediate, Advanced)
        /// </summary>
        [StringLength(20)]
        public string CurrentLevel { get; set; }

        /// <summary>
        /// Mục tiêu học tập chính
        /// </summary>
        [StringLength(500)]
        public string LearningGoals { get; set; }

        /// <summary>
        /// Lý do học tiếng Nhật
        /// </summary>
        [StringLength(1000)]
        public string LearningMotivation { get; set; }

        /// <summary>
        /// Kinh nghiệm học tập trước đó
        /// </summary>
        [StringLength(1000)]
        public string PreviousExperience { get; set; }

        /// <summary>
        /// Điểm mạnh trong học tập
        /// </summary>
        [StringLength(500)]
        public string LearningStrengths { get; set; }

        /// <summary>
        /// Khó khăn trong học tập
        /// </summary>
        [StringLength(500)]
        public string LearningChallenges { get; set; }

        /// <summary>
        /// Trạng thái xác thực profile
        /// </summary>
        public bool IsVerified { get; set; } = false;

        /// <summary>
        /// Ngày xác thực profile
        /// </summary>
        public DateTime? VerifiedAt { get; set; }

        /// <summary>
        /// ID người xác thực
        /// </summary>
        public int? VerifiedBy { get; set; }

        /// <summary>
        /// Điểm reputation/uy tín
        /// </summary>
        public int ReputationScore { get; set; } = 0;

        /// <summary>
        /// Trạng thái online cuối cùng
        /// </summary>
        public DateTime? LastOnlineAt { get; set; }

        /// <summary>
        /// Có đang online không
        /// </summary>
        public bool IsOnline { get; set; } = false;

        /// <summary>
        /// Trạng thái hoạt động (Available, Busy, Away, DoNotDisturb)
        /// </summary>
        [StringLength(20)]
        public string ActivityStatus { get; set; } = "Available";

        /// <summary>
        /// Tin nhắn trạng thái tùy chỉnh
        /// </summary>
        [StringLength(200)]
        public string StatusMessage { get; set; }

        /// <summary>
        /// Thống kê bổ sung (JSON format)
        /// </summary>
        public string StatisticsJson { get; set; }

        /// <summary>
        /// Ghi chú nội bộ (chỉ admin)
        /// </summary>
        public string InternalNotes { get; set; }

        // Navigation properties
        /// <summary>
        /// Người dùng sở hữu profile này
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// File media avatar
        /// </summary>
        [ForeignKey("AvatarMediaId")]
        public virtual MediaFile AvatarMedia { get; set; }

        /// <summary>
        /// Người xác thực profile
        /// </summary>
        [ForeignKey("VerifiedBy")]
        public virtual User VerifiedByUser { get; set; }

        // Computed Properties
        /// <summary>
        /// Tên hiển thị ưa thích (DisplayName hoặc FullName)
        /// </summary>
        [NotMapped]
        public string PreferredName => !string.IsNullOrEmpty(DisplayName) ? DisplayName : FullName;

        /// <summary>
        /// Tuổi của người dùng
        /// </summary>
        [NotMapped]
        public int? Age
        {
            get
            {
                if (!DateOfBirth.HasValue) return null;
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Value.Year;
                if (DateOfBirth.Value.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        /// <summary>
        /// Kiểm tra profile có đầy đủ thông tin cơ bản không
        /// </summary>
        [NotMapped]
        public bool IsProfileComplete
        {
            get
            {
                return !string.IsNullOrEmpty(Biography)
                    && DateOfBirth.HasValue;
            }
        }

        /// <summary>
        /// Phần trăm hoàn thành profile
        /// </summary>
        [NotMapped]
        public int ProfileCompletionPercentage
        {
            get
            {
                int completed = 0;
                int total = 10;

                if (!string.IsNullOrEmpty(Biography)) completed++;
                if (DateOfBirth.HasValue) completed++;
                if (AvatarMediaId.HasValue || !string.IsNullOrEmpty(AvatarUrl)) completed++;
                if (!string.IsNullOrEmpty(LearningGoals)) completed++;
                if (!string.IsNullOrEmpty(CurrentLevel)) completed++;
                if (!string.IsNullOrEmpty(LearningMotivation)) completed++;

                return (int)((double)completed / total * 100);
            }
        }

        // Methods
        /// <summary>
        /// Cập nhật thời gian online cuối cùng
        /// </summary>
        public virtual void UpdateLastOnline()
        {
            LastOnlineAt = DateTime.UtcNow;
            IsOnline = true;
            UpdateTimestamp();
        }

        /// <summary>
        /// Đặt trạng thái offline
        /// </summary>
        public virtual void SetOffline()
        {
            IsOnline = false;
            UpdateTimestamp();
        }

        /// <summary>
        /// Cập nhật trạng thái hoạt động
        /// </summary>
        /// <param name="status">Trạng thái mới</param>
        /// <param name="message">Tin nhắn trạng thái</param>
        public virtual void UpdateActivityStatus(string status, string message = null)
        {
            ActivityStatus = status;
            StatusMessage = message;
            UpdateTimestamp();
        }

        /// <summary>
        /// Xác thực profile
        /// </summary>
        /// <param name="verifiedBy">ID người xác thực</param>
        public virtual void VerifyProfile(int verifiedBy)
        {
            IsVerified = true;
            VerifiedAt = DateTime.UtcNow;
            VerifiedBy = verifiedBy;
            UpdateTimestamp();
        }

        /// <summary>
        /// Hủy xác thực profile
        /// </summary>
        public virtual void UnverifyProfile()
        {
            IsVerified = false;
            VerifiedAt = null;
            VerifiedBy = null;
            UpdateTimestamp();
        }

        /// <summary>
        /// Lấy tên hiển thị của profile
        /// </summary>
        /// <returns>Tên hiển thị</returns>
        public override string GetDisplayName()
        {
            return PreferredName;
        }

        /// <summary>
        /// Validate profile
        /// </summary>
        /// <returns>True nếu hợp lệ</returns>
        public override bool IsValid()
        {
            return base.IsValid() 
                && UserId > 0
                && (DateOfBirth == null || DateOfBirth.Value <= DateTime.Today)
                && (Age == null || Age >= 0);
        }
    }
}
