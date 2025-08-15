using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LexiFlow.Models.User;

namespace LexiFlow.Models.Core
{
    /// <summary>
    /// Lớp cơ sở với thông tin người tạo/cập nhật
    /// Kế thừa từ BaseEntity và bổ sung thông tin audit
    /// Sử dụng cho các entity cần theo dõi ai tạo và ai chỉnh sửa
    /// </summary>
    public abstract class AuditableEntity : BaseEntity
    {
        /// <summary>
        /// ID của người tạo entity
        /// Bắt buộc phải có khi tạo mới
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// ID của người chỉnh sửa cuối cùng
        /// Có thể null nếu chưa ai chỉnh sửa sau khi tạo
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// Thông tin người tạo entity
        /// Navigation property tới bảng User
        /// </summary>
        [ForeignKey("CreatedBy")]
        public virtual User.User CreatedByUser { get; set; }

        /// <summary>
        /// Thông tin người chỉnh sửa cuối cùng
        /// Navigation property tới bảng User
        /// </summary>
        [ForeignKey("ModifiedBy")]
        public virtual User.User ModifiedByUser { get; set; }

        /// <summary>
        /// Ghi chú về thay đổi
        /// Lưu trữ thông tin về lý do thay đổi hoặc mô tả thay đổi
        /// </summary>
        [StringLength(500)]
        public virtual string ChangeReason { get; set; }

        /// <summary>
        /// Phiên bản của entity
        /// Tự động tăng mỗi khi có thay đổi
        /// </summary>
        public virtual int Version { get; set; } = 1;

        /// <summary>
        /// Kiểm tra xem entity có được tạo bởi user hiện tại không
        /// </summary>
        /// <param name="userId">ID của user cần kiểm tra</param>
        /// <returns>True nếu user là người tạo</returns>
        public virtual bool IsCreatedBy(int userId)
        {
            return CreatedBy == userId;
        }

        /// <summary>
        /// Kiểm tra xem entity có được chỉnh sửa bởi user hiện tại không
        /// </summary>
        /// <param name="userId">ID của user cần kiểm tra</param>
        /// <returns>True nếu user là người chỉnh sửa cuối</returns>
        public virtual bool IsLastModifiedBy(int userId)
        {
            return ModifiedBy == userId;
        }

        /// <summary>
        /// Cập nhật thông tin modification
        /// </summary>
        /// <param name="modifiedBy">ID của người chỉnh sửa</param>
        /// <param name="reason">Lý do thay đổi</param>
        public virtual void UpdateModification(int modifiedBy, string reason = null)
        {
            ModifiedBy = modifiedBy;
            ChangeReason = reason;
            Version++;
            UpdateTimestamp();
        }

        /// <summary>
        /// Lấy tên người tạo (nếu có)
        /// </summary>
        /// <returns>Tên người tạo hoặc "Unknown"</returns>
        public virtual string GetCreatedByName()
        {
            return CreatedByUser?.Username ?? "Unknown";
        }

        /// <summary>
        /// Lấy tên người chỉnh sửa cuối (nếu có)
        /// </summary>
        /// <returns>Tên người chỉnh sửa cuối hoặc "Unknown"</returns>
        public virtual string GetModifiedByName()
        {
            return ModifiedByUser?.Username ?? GetCreatedByName();
        }
    }
}
