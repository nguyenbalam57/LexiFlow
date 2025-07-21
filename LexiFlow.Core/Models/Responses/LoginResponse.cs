using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Core.Models.Responses
{
    /// <summary>
    /// Phản hồi đăng nhập
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Trạng thái thành công
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Token truy cập
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Thời gian hết hạn
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Thông tin người dùng
        /// </summary>
        public UserDto User { get; set; }
    }

    /// <summary>
    /// Thông tin người dùng
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// ID người dùng
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Tên đầy đủ
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Vai trò
        /// </summary>
        public string Role { get; set; }
    }
}
