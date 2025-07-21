using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Core.Models
{
    /// <summary>
    /// Lớp phản hồi chung cho các API request
    /// </summary>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Trạng thái thành công của request
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Thông báo lỗi nếu có
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Đánh dấu phiên đăng nhập đã hết hạn
        /// </summary>
        public bool SessionExpired { get; set; }

        /// <summary>
        /// Tạo phản hồi thành công
        /// </summary>
        public static ApiResponse<T> Success(T data)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data
            };
        }

        /// <summary>
        /// Tạo phản hồi thất bại
        /// </summary>
        public static ApiResponse<T> Fail(string message, bool sessionExpired = false)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                SessionExpired = sessionExpired
            };
        }
    }
}
