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
        /// ID duy nhất của danh mục (Primary Key)
        /// Được tự động tạo bởi database
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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
        public string Description { get; set; } = "";

        /// <summary>
        /// Metadata dạng JSON để lưu trữ thông tin bổ sung
        /// Có thể sử dụng để lưu trữ các thuộc tính động không cố định
        /// Cũng có thể dùng thuộc tính Metadata để truy cập dễ dàng hơn
        /// Bổ sung các thông tin có trong properties chẳng hạn
        /// Hay quyền truy cập, tags, categories, v.v.
        /// Cả những lần thay đổi nhỏ không cần tạo cột riêng trong DB
        /// Hay lưu trữ cấu hình tùy chỉnh cho entity
        /// Truy cập ngoại lệ thông tin động
        /// </summary>
        public string MetadataJson { get; set; } = "{}";

        /// <summary>
        /// Thứ tự sắp xếp
        /// </summary>
        public int? SortOrder { get; set; }

        /// <summary>
        /// Validate entity trước khi save
        /// Kiểm tra xem entity có hợp lệ để sử dụng không
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
            get
            {
                // Xử lý an toàn: Nếu JSON rỗng, null hoặc "null" thì trả về Dictionary mới
                if (string.IsNullOrEmpty(MetadataJson) || MetadataJson == "null")
                {
                    return new Dictionary<string, object>();
                }

                // Thử deserialize
                try
                {
                    return JsonSerializer.Deserialize<Dictionary<string, object>>(MetadataJson)
                           ?? new Dictionary<string, object>();
                }
                catch (JsonException)
                {
                    // Nếu JSON bị lỗi, trả về rỗng để tránh crash
                    return new Dictionary<string, object>();
                }
            }
            set
            {
                // Serialize lại thành chuỗi JSON
                MetadataJson = JsonSerializer.Serialize(value);
            }
        }

    }
}
