using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.User
{
    /// <summary>
    /// DTO để cập nhật thông tin vai trò
    /// </summary>
    public class UpdateRoleDto : CreateRoleDto
    {
        /// <summary>
        /// Chuỗi phiên bản hàng (dùng cho kiểm soát đồng thời)
        /// </summary>
        [Required]
        public string RowVersionString { get; set; } = string.Empty;
    }
}
