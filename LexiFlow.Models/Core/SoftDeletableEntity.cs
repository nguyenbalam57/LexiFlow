using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Core
{
    /// <summary>
    /// Lớp cơ sở cung cấp chức năng xóa mềm cho các entity
    /// Xóa mềm không xóa dữ liệu khỏi database mà chỉ đánh dấu trạng thái đã xóa
    /// </summary>
    public abstract class SoftDeletableEntity : BaseEntity, ISoftDeletable
    {
        /// <summary>
        /// Cờ đánh dấu bản ghi đã bị xóa mềm (mặc định là false - chưa bị xóa)
        /// </summary>
        [Required]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Thời điểm thực hiện xóa mềm (null nếu chưa bị xóa)
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// ID của người dùng thực hiện thao tác xóa mềm
        /// </summary>
        public int? DeletedBy { get; set; }

        /// <summary>
        /// Navigation property đến thông tin người dùng thực hiện xóa
        /// </summary>
        [ForeignKey("DeletedBy")]
        public virtual User.User DeletedByUser { get; set; }

        /// <summary>
        /// Thực hiện xóa mềm bản ghi
        /// </summary>
        /// <param name="deletedByUserId">ID của người thực hiện xóa</param>
        public virtual void SoftDelete(int? deletedByUserId = null)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = deletedByUserId;
        }

        /// <summary>
        /// Khôi phục bản ghi đã bị xóa mềm
        /// </summary>
        public virtual void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
            DeletedBy = null;
        }

        /// <summary>
        /// Kiểm tra bản ghi có bị xóa mềm hay không
        /// </summary>
        /// <returns>True nếu bản ghi đã bị xóa mềm</returns>
        [NotMapped]
        public bool IsSoftDeleted => IsDeleted && DeletedAt.HasValue;
    }
}
