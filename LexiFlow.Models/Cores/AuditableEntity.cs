using LexiFlow.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LexiFlow.Models.Cores
{
    /// <summary>
    /// Lớp cơ sở với thông tin audit trail
    /// </summary>
    public abstract class AuditableEntity : BaseEntity
    {
        /// <summary>
        /// ID của người tạo entityy
        /// </summary>
        [Required]
        public int CreatedBy { get; set; }

        /// <summary>
        /// ID của người chỉnh sửa cuối cùng
        /// </summary>
        public int? UpdatedBy { get; set; }

        /// <summary>
        /// ID của người xóa (soft delete)
        /// </summary>
        public int? DeletedBy { get; set; }

        /// <summary>
        /// Lý do thay đổi
        /// </summary>
        public string ChangeReason { get; set; } = "";

        /// <summary>
        /// Phiên bản của entity
        /// Dùng để kiểm soát Version khi thay đổi và để xem lịch sử thay đổi
        /// Viết version tăng dần mỗi khi entity được chỉnh sửa
        /// </summary>
        public int Version { get; set; } = 1;

        /// <summary>
        /// Navigation properties
        /// </summary>
        /// 

        [ForeignKey(nameof(CreatedBy))]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey(nameof(UpdatedBy))]
        public virtual User UpdatedByUser { get; set; }

        [ForeignKey(nameof(DeletedBy))]
        public virtual User DeletedByUser { get; set; }

        /// <summary>
        /// Kiểm tra quyền ownership
        /// </summary>
        public virtual bool IsCreatedBy(int userId) => CreatedBy == userId;
        public virtual bool IsLastUpdatedBy(int userId) => UpdatedBy == userId;
        public virtual bool IsDeletedBy(int userId) => DeletedBy == userId;

        /// <summary>
        /// Cập nhật thông tin
        /// </summary>
        public virtual void SoftUpdate(int updatedBy, string reason = null)
        {
            UpdatedBy = updatedBy;
            ChangeReason = reason;
            Version++;
            UpdateTimestamp();
        }

        /// <summary>
        /// Soft delete với audit
        /// </summary>
        public virtual void SoftDelete(int deletedBy, string reason = null)
        {
            DeletedBy = deletedBy;
            ChangeReason = reason;
            base.SoftDelete();
        }

        /// <summary>
        /// Khôi phục từ soft delete với audit
        /// </summary>
        /// <param name="restoredBy"></param>
        /// <param name="reason"></param>
        public virtual void Restore(int restoredBy, string reason = null)
        {
            UpdatedBy = restoredBy;
            ChangeReason = reason;
            base.Restore();
        }

        /// <summary>
        /// Trạng thái kích hoạt với audit
        /// </summary>
        /// <param name="activatedBy"></param>
        /// <param name="reason"></param>
        public virtual void Activate(int activatedBy, string reason = null)
        {
            UpdatedBy = activatedBy;
            ChangeReason = reason;
            base.Activate();
        }

        /// <summary>
        /// Trạng thái vô hiệu hóa với audit
        /// </summary>
        /// <param name="deactivatedBy"></param>
        /// <param name="reason"></param>
        public virtual void Deactivate(int deactivatedBy, string reason = null)
        {
            UpdatedBy = deactivatedBy;
            ChangeReason = reason;
            base.Deactivate();
        }

        /// <summary>
        /// Lấy tên người thực hiện tạo mới
        /// </summary>
        public virtual string GetCreatedByName() => CreatedByUser?.Username ?? "Unknown";

        /// <summary>
        /// Lấy tên người thực hiện chỉnh sửa cuối
        /// </summary>
        /// <returns></returns>
        public virtual string GetModifiedByName() => UpdatedByUser?.Username ?? GetCreatedByName();

        /// <summary>
        /// Lấy tên người thực hiện xóa
        /// </summary>
        /// <returns></returns>
        public virtual string GetDeletedByName() => DeletedByUser?.Username ?? "Unknown";
    }
}
