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
    /// Lớp cơ sở cho tất cả các entity trong hệ thống LexiFlow
    /// Cung cấp các thuộc tính chung như: trạng thái hoạt động, thời gian tạo/cập nhật, phiên bản hàng
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Trạng thái hoạt động của entity
        /// true = đang hoạt động, false = không hoạt động
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Thời gian tạo entity (UTC)
        /// Được tự động set khi tạo mới
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Thời gian cập nhật cuối cùng (UTC)
        /// Được tự động cập nhật mỗi khi entity thay đổi
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Phiên bản hàng cho Optimistic Concurrency Control
        /// Được Entity Framework tự động quản lý
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }

        /// <summary>
        /// Mô tả/ghi chú cho entity (tùy chọn)
        /// Có thể được các entity con override để có ý nghĩa cụ thể
        /// </summary>
        [StringLength(1000)]
        public virtual string Description { get; set; }

        /// <summary>
        /// Metadata dạng JSON để lưu trữ thông tin bổ sung
        /// Sử dụng cho các thuộc tính động không cần thiết phải tạo column riêng
        /// </summary>
        public virtual string Metadata { get; set; }

        /// <summary>
        /// Thứ tự sắp xếp (tùy chọn)
        /// Có thể được sử dụng để sắp xếp các entity
        /// </summary>
        public virtual int? SortOrder { get; set; }

        /// <summary>
        /// Phương thức ảo để validate entity trước khi save
        /// Override trong các entity con để thực hiện validation tùy chỉnh
        /// </summary>
        /// <returns>True nếu hợp lệ, False nếu có lỗi</returns>
        public virtual bool IsValid()
        {
            return IsActive; // Mặc định chỉ check IsActive
        }

        /// <summary>
        /// Phương thức ảo để lấy display name của entity
        /// Override trong các entity con để trả về tên hiển thị phù hợp
        /// </summary>
        /// <returns>Tên hiển thị của entity</returns>
        public virtual string GetDisplayName()
        {
            return GetType().Name;
        }

        /// <summary>
        /// Cập nhật thời gian UpdatedAt
        /// Gọi method này trước khi save changes
        /// </summary>
        public virtual void UpdateTimestamp()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
