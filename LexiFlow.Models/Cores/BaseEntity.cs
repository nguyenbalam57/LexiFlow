using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LexiFlow.Models.Cores
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
        public virtual string Description { get; set; } = "";

        /// <summary>
        /// Metadata dạng JSON để lưu trữ thông tin bổ sung
        /// </summary>
        public virtual string MetadataJson { get; set; } = "{}";

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
        /// Kích hoạt entity
        /// </summary>
        public virtual void Activate()
        {
            IsActive = true; // kích hoạt entity
            UpdateTimestamp();
        }

        /// <summary>
        /// Vô hiệu hóa entity
        /// </summary>
        public virtual void Deactivate()
        {
            IsActive = false; // vô hiệu hóa entity
            UpdateTimestamp();
        }

        /// <summary>
        /// Soft delete entity
        /// Xóa mềm (không xóa vật lý) bằng cách đánh dấu IsDeleted = true
        /// </summary>
        public virtual void SoftDelete()
        {
            IsDeleted = true; // đánh dấu là đã bị xóa
            DeletedAt = DateTime.UtcNow; // ghi lại thời điểm xóa (UTC)
            IsActive = false; // vô hiệu hóa thực thể này
            UpdateTimestamp(); // cập nhật UpdatedAt (nếu có)
        }

        /// <summary>
        /// Restore từ soft delete
        /// Khôi phục dữ liệu đã bị soft delete
        /// </summary>
        public virtual void Restore()
        {
            IsDeleted = false; // đánh dấu object này KHÔNG bị xóa nữa
            DeletedAt = null; // bỏ timestamp đã xóa (xem như chưa từng bị xóa)
            IsActive = true; // kích hoạt lại (thường dùng cho business logic)
            UpdateTimestamp(); // cập nhật thời gian chỉnh sửa cuối cùng
        }

        /// <summary>
        /// Metadata của file dưới dạng Dictionary để dễ truy cập từ code
        /// </summary>
        /// <value>
        /// Tự động parse từ MetadataJson thành Dictionary.
        /// Trả về empty Dictionary nếu MetadataJson null hoặc rỗng
        /// </value>
        [NotMapped]
        public Dictionary<string, object> Metadata
        {
            get => string.IsNullOrEmpty(MetadataJson)
                ? new Dictionary<string, object>()
                : JsonSerializer.Deserialize<Dictionary<string, object>>(MetadataJson);
            set => MetadataJson = JsonSerializer.Serialize(value);
        }

    }
}
