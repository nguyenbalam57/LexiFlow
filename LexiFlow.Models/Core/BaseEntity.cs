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
    /// Cung cấp các thuộc tính chung: trạng thái, thời gian, soft delete
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Trạng thái hoạt động của entity
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Soft delete flag - đánh dấu xóa mềm
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Thời gian tạo entity (UTC)
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Thời gian cập nhật cuối cùng (UTC)
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Thời gian xóa mềm (UTC) - null nếu chưa bị xóa
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// Phiên bản hàng cho Optimistic Concurrency Control
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }

        /// <summary>
        /// Mô tả/ghi chú cho entity
        /// </summary>
        [StringLength(1000)]
        public virtual string Description { get; set; }

        /// <summary>
        /// Metadata dạng JSON để lưu trữ thông tin bổ sung
        /// </summary>
        public virtual string Metadata { get; set; }

        /// <summary>
        /// Thứ tự sắp xếp
        /// </summary>
        public virtual int? SortOrder { get; set; }

        /// <summary>
        /// Validate entity trước khi save
        /// </summary>
        public virtual bool IsValid()
        {
            return IsActive && !IsDeleted;
        }

        /// <summary>
        /// Lấy display name của entity
        /// </summary>
        public virtual string GetDisplayName()
        {
            return GetType().Name;
        }

        /// <summary>
        /// Cập nhật timestamp
        /// </summary>
        public virtual void UpdateTimestamp()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Soft delete entity
        /// </summary>
        public virtual void SoftDelete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            IsActive = false;
            UpdateTimestamp();
        }

        /// <summary>
        /// Restore từ soft delete
        /// </summary>
        public virtual void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
            IsActive = true;
            UpdateTimestamp();
        }
    }
}
