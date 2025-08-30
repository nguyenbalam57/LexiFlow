using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Core
{
    /// <summary>
    /// Interface định nghĩa các thuộc tính cần thiết cho chức năng xóa mềm
    /// </summary>
    public interface ISoftDeletable
    {
        // <summary>
        /// Trạng thái đánh dấu bản ghi đã bị xóa mềm hay chưa
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// Thời điểm thực hiện xóa mềm
        /// </summary>
        DateTime? DeletedAt { get; set; }

        /// <summary>
        /// ID của người dùng thực hiện xóa mềm
        /// </summary>
        int? DeletedBy { get; set; }
    }
}
