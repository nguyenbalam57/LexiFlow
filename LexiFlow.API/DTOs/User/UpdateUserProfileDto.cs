using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.User
{
    /// <summary>
    /// DTO cho cập nhật hồ sơ người dùng
    /// </summary>
    public class UpdateUserProfileDto
    {
        /// <summary>
        /// Email
        /// </summary>
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Họ
        /// </summary>
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Tên
        /// </summary>
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Phòng ban
        /// </summary>
        [StringLength(100)]
        public string Department { get; set; } = string.Empty;

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [StringLength(50)]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Chức vụ
        /// </summary>
        [StringLength(100)]
        public string Position { get; set; } = string.Empty;

        /// <summary>
        /// URL ảnh đại diện
        /// </summary>
        [StringLength(255)]
        public string AvatarUrl { get; set; } = string.Empty;

        /// <summary>
        /// Chuỗi phiên bản hàng
        /// </summary>
        public string RowVersionString { get; set; } = string.Empty;
    }
}
