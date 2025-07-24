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
        public bool Success { get; set; }
        public string? Message { get; set; }
        public bool SessionExpired { get; set; }
        public T? Data { get; set; }

        public static ServiceResult<T> Fail(string message, bool sessionExpired = false)
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                SessionExpired = sessionExpired
            };
        }

        public static ServiceResult<T> CreateSuccess(T data)
        {
            return new ServiceResult<T>
            {
                Success = true,
                Data = data
            };
        }
    }
}
