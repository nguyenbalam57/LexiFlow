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
        /// ID của người tạo entity
        /// </summary>
        [Required]
        public int CreatedBy { get; set; }

        /// <summary>
        /// ID của người chỉnh sửa cuối cùng
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// ID của người xóa (soft delete)
        /// </summary>
        public int? DeletedBy { get; set; }

        /// <summary>
        /// Lý do thay đổi
        /// </summary>
        [StringLength(500)]
        public virtual string ChangeReason { get; set; } = "";

        /// <summary>
        /// Phiên bản của entity
        /// </summary>
        public virtual int Version { get; set; } = 1;

        /// <summary>
        /// Navigation properties
        /// </summary>
        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual User ModifiedByUser { get; set; }

        [ForeignKey("DeletedBy")]
        public virtual User DeletedByUser { get; set; }

        /// <summary>
        /// Kiểm tra quyền ownership
        /// </summary>
        public virtual bool IsCreatedBy(int userId) => CreatedBy == userId;
        public virtual bool IsLastModifiedBy(int userId) => ModifiedBy == userId;
        public virtual bool IsDeletedBy(int userId) => DeletedBy == userId;

        /// <summary>
        /// Cập nhật thông tin modification
        /// </summary>
        public virtual void UpdateModification(int modifiedBy, string reason = null)
        {
            ModifiedBy = modifiedBy;
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
            ModifiedBy = restoredBy;
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
            ModifiedBy = activatedBy;
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
            ModifiedBy = deactivatedBy;
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
        public virtual string GetModifiedByName() => ModifiedByUser?.Username ?? GetCreatedByName();
        /// <summary>
        /// Lấy tên người thực hiện xóa
        /// </summary>
        /// <returns></returns>
        public virtual string GetDeletedByName() => DeletedByUser?.Username ?? "Unknown";
    }
}
