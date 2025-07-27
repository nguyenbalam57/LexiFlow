using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.User
{
    /// <summary>
    /// DTO để cập nhật thông tin quyền
    /// </summary>
    public class UpdatePermissionDto : CreatePermissionDto
    {
        /// <summary>
        /// Chuỗi phiên bản hàng (dùng cho kiểm soát đồng thời)
        /// </summary>
        [Required]
        public string RowVersionString { get; set; } = string.Empty;
    }
}
