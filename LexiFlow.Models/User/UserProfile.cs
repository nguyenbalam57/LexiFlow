using LexiFlow.Models.Core;
using LexiFlow.Models.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.User
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
        public string FirstName { get; set; }

        /// <summary>
        /// Họ của người dùng
        /// </summary>
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        /// <summary>
        /// Tên đệm/tên lót
        /// </summary>
        [StringLength(100)]
        public string MiddleName { get; set; }

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
        /// Số điện thoại
        /// </summary>
        [StringLength(20)]
        [Phone]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Số điện thoại phụ/công việc
        /// </summary>
        [StringLength(20)]
        [Phone]
        public string WorkPhoneNumber { get; set; }

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
        /// Quốc tịch
        /// </summary>
        [StringLength(100)]
        public string Nationality { get; set; }

        /// <summary>
        /// Ngôn ngữ mẹ đẻ
        /// </summary>
        [StringLength(50)]
        public string NativeLanguage { get; set; }

        /// <summary>
        /// Các ngôn ngữ biết (JSON format)
        /// </summary>
        public string KnownLanguagesJson { get; set; }

        /// <summary>
        /// Địa chỉ nhà
        /// </summary>
        [StringLength(500)]
        public string HomeAddress { get; set; }

        /// <summary>
        /// Thành phố
        /// </summary>
        [StringLength(100)]
        public string City { get; set; }

        /// <summary>
        /// Tỉnh/Bang
        /// </summary>
        [StringLength(100)]
        public string State { get; set; }

        /// <summary>
        /// Quốc gia
        /// </summary>
        [StringLength(100)]
        public string Country { get; set; }

        /// <summary>
        /// Mã bưu điện
        /// </summary>
        [StringLength(20)]
        public string PostalCode { get; set; }

        /// <summary>
        /// Múi giờ của người dùng
        /// </summary>
        [StringLength(50)]
        public string TimeZone { get; set; } = "Asia/Ho_Chi_Minh";

        /// <summary>
        /// Vị trí công việc/chức danh
        /// </summary>
        [StringLength(100)]
        public string JobTitle { get; set; }

        /// <summary>
        /// Tên công ty/tổ chức
        /// </summary>
        [StringLength(200)]
        public string Company { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        [StringLength(100)]
        public string DepartmentName { get; set; }

        /// <summary>
        /// ID phòng ban
        /// </summary>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// ID của file media avatar
        /// </summary>
        public int? AvatarMediaId { get; set; }

        /// <summary>
        /// URL avatar dự phòng (nếu không có file media)
        /// </summary>
        [StringLength(500)]
        public string AvatarUrl { get; set; }

        /// <summary>
        /// ID của file media cover/banner
        /// </summary>
        public int? CoverImageMediaId { get; set; }

        /// <summary>
        /// URL cover/banner dự phòng
        /// </summary>
        [StringLength(500)]
        public string CoverImageUrl { get; set; }

        /// <summary>
        /// Website cá nhân
        /// </summary>
        [StringLength(255)]
        public string PersonalWebsite { get; set; }

        /// <summary>
        /// Liên kết mạng xã hội (JSON format)
        /// </summary>
        public string SocialLinksJson { get; set; }

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
        /// Cho phép kết bạn
        /// </summary>
        public bool AllowFriendRequests { get; set; } = true;

        /// <summary>
        /// Cài đặt quyền riêng tư (JSON format)
        /// </summary>
        public string PrivacySettingsJson { get; set; }

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
        /// Ngôn ngữ giao diện
        /// </summary>
        [StringLength(10)]
        public string InterfaceLanguage { get; set; } = "vi";

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
        /// Sở thích cá nhân (JSON format)
        /// </summary>
        public string InterestsJson { get; set; }

        /// <summary>
        /// Kỹ năng đặc biệt (JSON format)
        /// </summary>
        public string SkillsJson { get; set; }

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
        /// Số lần xem profile
        /// </summary>
        public int ProfileViews { get; set; } = 0;

        /// <summary>
        /// Số người theo dõi
        /// </summary>
        public int FollowerCount { get; set; } = 0;

        /// <summary>
        /// Số người đang theo dõi
        /// </summary>
        public int FollowingCount { get; set; } = 0;

        /// <summary>
        /// Thống kê bổ sung (JSON format)
        /// </summary>
        public string StatisticsJson { get; set; }

        /// <summary>
        /// Cài đặt tùy chỉnh khác (JSON format)
        /// </summary>
        public string CustomSettingsJson { get; set; }

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
        /// Phòng ban của người dùng
        /// </summary>
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        /// <summary>
        /// File media avatar
        /// </summary>
        [ForeignKey("AvatarMediaId")]
        public virtual MediaFile AvatarMedia { get; set; }

        /// <summary>
        /// File media cover image
        /// </summary>
        [ForeignKey("CoverImageMediaId")]
        public virtual MediaFile CoverImageMedia { get; set; }

        /// <summary>
        /// Người xác thực profile
        /// </summary>
        [ForeignKey("VerifiedBy")]
        public virtual User VerifiedByUser { get; set; }

        // Computed Properties
        /// <summary>
        /// Tên đầy đủ của người dùng
        /// </summary>
        [NotMapped]
        public string FullName 
        { 
            get 
            {
                var parts = new List<string>();
                if (!string.IsNullOrEmpty(FirstName)) parts.Add(FirstName);
                if (!string.IsNullOrEmpty(MiddleName)) parts.Add(MiddleName);
                if (!string.IsNullOrEmpty(LastName)) parts.Add(LastName);
                return string.Join(" ", parts);
            }
        }

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
        /// Địa chỉ đầy đủ
        /// </summary>
        [NotMapped]
        public string FullAddress
        {
            get
            {
                var parts = new List<string>();
                if (!string.IsNullOrEmpty(HomeAddress)) parts.Add(HomeAddress);
                if (!string.IsNullOrEmpty(City)) parts.Add(City);
                if (!string.IsNullOrEmpty(State)) parts.Add(State);
                if (!string.IsNullOrEmpty(Country)) parts.Add(Country);
                return string.Join(", ", parts);
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
                return !string.IsNullOrEmpty(FirstName) 
                    && !string.IsNullOrEmpty(LastName)
                    && !string.IsNullOrEmpty(Biography)
                    && DateOfBirth.HasValue
                    && !string.IsNullOrEmpty(Country);
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

                if (!string.IsNullOrEmpty(FirstName)) completed++;
                if (!string.IsNullOrEmpty(LastName)) completed++;
                if (!string.IsNullOrEmpty(Biography)) completed++;
                if (DateOfBirth.HasValue) completed++;
                if (!string.IsNullOrEmpty(Country)) completed++;
                if (!string.IsNullOrEmpty(PhoneNumber)) completed++;
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
        /// Tăng số lượt xem profile
        /// </summary>
        public virtual void IncrementProfileViews()
        {
            ProfileViews++;
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
                && !string.IsNullOrWhiteSpace(FirstName)
                && !string.IsNullOrWhiteSpace(LastName)
                && UserId > 0
                && (DateOfBirth == null || DateOfBirth.Value <= DateTime.Today)
                && (Age == null || Age >= 0);
        }
    }
}
