using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Core.Models
{
    /// <summary>
    /// Lớp kết quả chung cho các dịch vụ
    /// </summary>
    public class ServiceResult<T>
    {
        /// <summary>
        /// Trạng thái thành công của thao tác
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
        /// Tạo kết quả thành công
        /// </summary>
        public static ServiceResult<T> Success(T data)
        {
            return new ServiceResult<T>
            {
                Success = true,
                Data = data
            };
        }

        /// <summary>
        /// Tạo kết quả thất bại
        /// </summary>
        public static ServiceResult<T> Fail(string message, bool sessionExpired = false)
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                SessionExpired = sessionExpired
            };
        }
    }
}
