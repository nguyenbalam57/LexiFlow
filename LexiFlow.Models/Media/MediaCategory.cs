using LexiFlow.Models.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Media
{
    /// <summary>
    /// Danh mục lưu trữ media
    /// </summary>
    [Index(nameof(CategoryName), IsUnique = true, Name = "IX_MediaCategory_Name")]
    public class MediaCategory : BaseEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        // Cải tiến: Phân cấp danh mục
        public int? ParentCategoryId { get; set; }

        public int DisplayOrder { get; set; } = 0;

        // Cải tiến: Phân loại và giới hạn
        [StringLength(50)]
        public string MediaTypes { get; set; } // Audio, Image, Video, All

        public long? MaxFileSizeBytes { get; set; } // Giới hạn kích thước file

        public string AllowedExtensions { get; set; } // Định dạng file được phép

        // Cải tiến: Thuộc tính lưu trữ
        [StringLength(255)]
        public string StoragePath { get; set; } // Đường dẫn lưu trữ

        [StringLength(50)]
        public string StorageType { get; set; } // Local, S3, Azure, etc.

        public string StorageOptions { get; set; } // Tùy chọn lưu trữ

        // Cải tiến: Xử lý media
        public bool EnableCompression { get; set; } = true; // Bật nén

        public bool CreateThumbnails { get; set; } = true; // Tạo thumbnails

        public string ProcessingRules { get; set; } // Quy tắc xử lý

        // Cải tiến: Phân quyền
        public bool IsPublic { get; set; } = true; // Công khai

        public string ViewPermissions { get; set; } // Quyền xem

        public string UploadPermissions { get; set; } // Quyền tải lên

        public bool IsActive { get; set; } = true;

        // IActivatable implementation
        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;

        // Cải tiến: Thống kê
        public int? FileCount { get; set; } = 0; // Số lượng file

        public long? TotalSizeBytes { get; set; } = 0; // Tổng kích thước

        public DateTime? LastUpload { get; set; } // Lần upload cuối

        // Navigation properties
        [ForeignKey("ParentCategoryId")]
        public virtual MediaCategory ParentCategory { get; set; }

        public virtual ICollection<MediaCategory> ChildCategories { get; set; }
        public virtual ICollection<MediaFile> MediaFiles { get; set; }
    }
}
