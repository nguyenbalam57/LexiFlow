using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.User
{
    /// <summary>
    /// DTO để cập nhật thông tin nhóm làm việc
    /// </summary>
    public class UpdateTeamDto : CreateTeamDto
    {
        /// <summary>
        /// Chuỗi phiên bản hàng (dùng cho kiểm soát đồng thời)
        /// </summary>
        [Required]
        public string RowVersionString { get; set; } = string.Empty;
    }
}
