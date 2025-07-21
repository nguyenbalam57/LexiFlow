using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Core.Models.Responses
{
    /// <summary>
    /// Phản hồi đồng bộ dữ liệu
    /// </summary>
    public class SyncResponse
    {
        /// <summary>
        /// Trạng thái thành công
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Thông báo
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Kết quả đồng bộ
        /// </summary>
        public SyncResult Data { get; set; }
    }
}
