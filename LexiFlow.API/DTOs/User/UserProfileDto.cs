using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.User
{
    /// <summary>
    /// DTO cho hồ sơ người dùng chi tiết
    /// </summary>
    public class UserProfileDto
    {
        /// <summary>
        /// ID người dùng
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Họ
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Tên
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Họ và tên đầy đủ
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Phòng ban
        /// </summary>
        public string Department { get; set; } = string.Empty;

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Chức vụ
        /// </summary>
        public string Position { get; set; } = string.Empty;

        /// <summary>
        /// URL ảnh đại diện
        /// </summary>
        public string AvatarUrl { get; set; } = string.Empty;

        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Email đã xác thực
        /// </summary>
        public bool IsEmailVerified { get; set; }

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Thời gian đăng nhập cuối
        /// </summary>
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// Danh sách ID vai trò
        /// </summary>
        public List<int> RoleIds { get; set; } = new List<int>();

        /// <summary>
        /// Danh sách tên vai trò
        /// </summary>
        public List<string> RoleNames { get; set; } = new List<string>();

        /// <summary>
        /// Thống kê học tập
        /// </summary>
        public UserLearningStatsDto LearningStats { get; set; } = new UserLearningStatsDto();

        /// <summary>
        /// Chuỗi phiên bản hàng
        /// </summary>
        public string RowVersionString { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO cho thống kê học tập của người dùng
    /// </summary>
    public class UserLearningStatsDto
    {
        /// <summary>
        /// Tổng số từ vựng đã học
        /// </summary>
        public int TotalVocabularyLearned { get; set; }

        /// <summary>
        /// Tổng số kanji đã học
        /// </summary>
        public int TotalKanjiLearned { get; set; }

        /// <summary>
        /// Tổng số điểm ngữ pháp đã học
        /// </summary>
        public int TotalGrammarLearned { get; set; }

        /// <summary>
        /// Số bài kiểm tra đã hoàn thành
        /// </summary>
        public int CompletedTests { get; set; }

        /// <summary>
        /// Điểm trung bình các bài kiểm tra
        /// </summary>
        public double AverageTestScore { get; set; }

        /// <summary>
        /// Thời gian học tập (phút)
        /// </summary>
        public int TotalStudyTimeMinutes { get; set; }

        /// <summary>
        /// Ngày học gần nhất
        /// </summary>
        public DateTime? LastStudyDate { get; set; }
    }
}
