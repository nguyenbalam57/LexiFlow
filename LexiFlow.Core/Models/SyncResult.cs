using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Core.Models
{
    /// <summary>
    /// Kết quả đồng bộ
    /// </summary>
    public class SyncResult
    {
        /// <summary>
        /// Trạng thái đồng bộ
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Thời gian đồng bộ
        /// </summary>
        public DateTime SyncedAt { get; set; }

        /// <summary>
        /// Số lượng bản ghi đã tải lên
        /// </summary>
        public int UploadedCount { get; set; }

        /// <summary>
        /// Số lượng bản ghi đã tải về
        /// </summary>
        public int DownloadedCount { get; set; }

        /// <summary>
        /// Số lượng bản ghi đã xóa
        /// </summary>
        public int DeletedCount { get; set; }

        /// <summary>
        /// Số lượng bản ghi bị lỗi
        /// </summary>
        public int FailedCount { get; set; }

        /// <summary>
        /// Thông báo lỗi nếu có
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
