using LexiFlow.Core.Entities;
using LexiFlow.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LexiFlow.Core.Models.Responses;

namespace LexiFlow.Core.Interfaces
{
    /// <summary>
    /// Interface cho dịch vụ xác thực
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Xác thực người dùng
        /// </summary>
        Task<ServiceResult<LoginResponse>> AuthenticateAsync(string username, string password);

        /// <summary>
        /// Đăng xuất
        /// </summary>
        void Logout();

        /// <summary>
        /// Kiểm tra xem người dùng đã đăng nhập chưa
        /// </summary>
        bool IsAuthenticated();

        /// <summary>
        /// Làm mới token
        /// </summary>
        Task<ServiceResult<string>> RefreshTokenAsync();

        /// <summary>
        /// Lấy token hiện tại
        /// </summary>
        string GetCurrentToken();

        /// <summary>
        /// Lấy thông tin người dùng hiện tại
        /// </summary>
        UserDto GetCurrentUser();
    }
}
