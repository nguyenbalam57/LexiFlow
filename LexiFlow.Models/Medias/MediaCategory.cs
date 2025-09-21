using LexiFlow.Models.Cores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Medias
{
    /// <summary>
    /// Model đại diện cho danh mục phân loại và quản lý file media trong hệ thống LexiFlow.
    /// Cung cấp cấu trúc phân cấp để tổ chức, cấu hình và kiểm soát việc lưu trữ media.
    /// </summary>
    /// <remarks>
    /// Entity này hỗ trợ:
    /// - Phân cấp danh mục theo cây (parent-child hierarchy)
    /// - Cấu hình riêng biệt cho từng loại media (Audio, Image, Video)
    /// - Quản lý storage và processing rules
    /// - Phân quyền truy cập và upload theo category
    /// - Thống kê và monitoring dung lượng
    /// - Tích hợp với nhiều storage backend (Local, S3, Azure, CDN)
    /// </remarks>
    [Index(nameof(CategoryName), IsUnique = true, Name = "IX_MediaCategory_Name")]
    public class MediaCategory : AuditableEntity
    {
        /// <summary>
        /// Khóa chính của bảng media category
        /// Tự động tăng
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        #region Thông tin cơ bản

        /// <summary>
        /// Tên danh mục media (duy nhất trong hệ thống)
        /// </summary>
        /// <value>
        /// Tên không được trùng lặp, thường sử dụng format có ý nghĩa như:
        /// - "Profile Pictures"
        /// - "Lesson Audio"
        /// - "Exercise Images" 
        /// - "Grammar Examples"
        /// - "User Uploads"
        /// Tối đa 100 ký tự
        /// </value>
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        /// <summary>
        /// Mô tả chi tiết về mục đích và cách sử dụng danh mục này
        /// </summary>
        /// <value>
        /// Văn bản giải thích danh mục dùng để lưu loại media gì, cho ai, mục đích gì
        /// Ví dụ: "Lưu trữ hình ảnh minh họa cho các bài học từ vựng"
        /// Tối đa 255 ký tự
        /// </value>
        [StringLength(255)]
        public string Description { get; set; }

        #endregion

        #region Phân cấp danh mục

        /// <summary>
        /// ID của danh mục cha trong cấu trúc phân cấp
        /// </summary>
        /// <value>
        /// Null nếu đây là danh mục gốc (root category).
        /// Có giá trị nếu đây là danh mục con
        /// Ví dụ: "Learning Materials" > "Vocabulary" > "Images"
        /// </value>
        public int? ParentCategoryId { get; set; }

        /// <summary>
        /// Thứ tự hiển thị trong cùng cấp danh mục
        /// </summary>
        /// <value>Số nguyên để sắp xếp, số nhỏ hơn hiển thị trước. Mặc định: 0</value>
        public int DisplayOrder { get; set; } = 0;

        #endregion

        #region Phân loại và giới hạn media

        /// <summary>
        /// Các loại media được phép trong danh mục này
        /// </summary>
        /// <value>
        /// Chuỗi phân cách bởi dấu phẩy:
        /// - "Audio": Chỉ file âm thanh
        /// - "Image": Chỉ hình ảnh
        /// - "Video": Chỉ video
        /// - "Document": Chỉ tài liệu
        /// - "Audio,Image": Cho phép âm thanh và hình ảnh
        /// - "All": Tất cả loại media
        /// Tối đa 50 ký tự
        /// </value>
        [StringLength(50)]
        public string MediaTypes { get; set; }

        /// <summary>
        /// Giới hạn kích thước file tối đa cho danh mục này (tính bằng bytes)
        /// </summary>
        /// <value>
        /// Null nếu không giới hạn.
        /// Ví dụ: 
        /// - 5MB = 5,242,880 bytes
        /// - 100MB = 104,857,600 bytes
        /// - 1GB = 1,073,741,824 bytes
        /// </value>
        public long? MaxFileSizeBytes { get; set; }

        /// <summary>
        /// Các định dạng file được phép upload vào danh mục này
        /// </summary>
        /// <value>
        /// Chuỗi extensions phân cách bởi dấu phẩy, bao gồm dấu chấm:
        /// - ".jpg,.jpeg,.png,.gif,.webp" cho hình ảnh
        /// - ".mp3,.wav,.ogg,.m4a" cho âm thanh
        /// - ".mp4,.webm,.avi,.mov" cho video
        /// - ".pdf,.doc,.docx,.txt" cho tài liệu
        /// </value>
        public string AllowedExtensions { get; set; }

        #endregion

        #region Cấu hình lưu trữ

        /// <summary>
        /// Đường dẫn thư mục lưu trữ file cho danh mục này
        /// </summary>
        /// <value>
        /// Đường dẫn tương đối từ root storage:
        /// - "uploads/profiles" cho ảnh profile
        /// - "media/lessons/audio" cho âm thanh bài học
        /// - "user-content/images" cho hình ảnh do user upload
        /// Tối đa 255 ký tự
        /// </value>
        [StringLength(255)]
        public string StoragePath { get; set; }

        /// <summary>
        /// Loại storage backend được sử dụng cho danh mục này
        /// </summary>
        /// <value>
        /// Các giá trị hợp lệ:
        /// - "Local": Lưu trên server local
        /// - "S3": Amazon S3
        /// - "Azure": Azure Blob Storage
        /// - "GCS": Google Cloud Storage
        /// - "CDN": Content Delivery Network
        /// Tối đa 50 ký tự
        /// </value>
        [StringLength(50)]
        public string StorageType { get; set; }

        /// <summary>
        /// Các tùy chọn cấu hình storage được lưu dưới dạng JSON
        /// </summary>
        /// <value>
        /// Chuỗi JSON chứa cấu hình như:
        /// - S3: {"bucket": "my-bucket", "region": "us-east-1", "encryption": true}
        /// - Azure: {"container": "media", "account": "mystorageaccount"}
        /// - Local: {"backup": true, "compress": false}
        /// </value>
        public string StorageOptions { get; set; }

        #endregion

        #region Xử lý media

        /// <summary>
        /// Bật tính năng nén file tự động khi upload
        /// </summary>
        /// <value>
        /// - True: Tự động nén/optimize file (giảm dung lượng, tăng tốc tải)
        /// - False: Giữ nguyên file gốc
        /// Mặc định: true
        /// </value>
        public bool EnableCompression { get; set; } = true;

        /// <summary>
        /// Tự động tạo thumbnails cho hình ảnh và video
        /// </summary>
        /// <value>
        /// - True: Tạo thumbnails với các kích thước khác nhau
        /// - False: Không tạo thumbnails
        /// Mặc định: true
        /// </value>
        public bool CreateThumbnails { get; set; } = true;

        /// <summary>
        /// Quy tắc xử lý media được định nghĩa dưới dạng JSON
        /// </summary>
        /// <value>
        /// Chuỗi JSON chứa rules như:
        /// - Hình ảnh: {"resize": {"max_width": 1920, "max_height": 1080}, "quality": 85, "formats": ["webp", "jpg"]}
        /// - Audio: {"bitrate": 128, "sample_rate": 44100, "normalize": true}
        /// - Video: {"resolution": "720p", "codec": "h264", "thumbnail_count": 3}
        /// </value>
        public string ProcessingRules { get; set; }

        #endregion

        #region Phân quyền truy cập

        /// <summary>
        /// Danh mục có được truy cập công khai không cần đăng nhập
        /// </summary>
        /// <value>
        /// - True: Public, ai cũng có thể xem media trong category này
        /// - False: Cần đăng nhập và có quyền mới xem được
        /// Mặc định: true
        /// </value>
        public bool IsPublic { get; set; } = true;

        /// <summary>
        /// Danh sách roles được phép xem media trong danh mục này
        /// </summary>
        /// <value>
        /// Chuỗi roles phân cách bởi dấu phẩy như: "Student,Teacher,Admin"
        /// Null hoặc rỗng nghĩa là tất cả đều có thể xem (nếu IsPublic = true)
        /// </value>
        public string ViewPermissions { get; set; }

        /// <summary>
        /// Danh sách roles được phép upload media vào danh mục này
        /// </summary>
        /// <value>
        /// Chuỗi roles phân cách bởi dấu phẩy như: "Teacher,Admin,ContentCreator"
        /// Null hoặc rỗng nghĩa là chỉ Admin có thể upload
        /// </value>
        public string UploadPermissions { get; set; }

        #endregion

        #region Thống kê và monitoring

        /// <summary>
        /// Tổng số file media hiện có trong danh mục này
        /// </summary>
        /// <value>
        /// Số nguyên >= 0. Được cập nhật tự động khi có file mới/xóa.
        /// Null nếu chưa tính toán. Mặc định: 0
        /// </value>
        public int? FileCount { get; set; } = 0;

        /// <summary>
        /// Tổng dung lượng của tất cả file trong danh mục (tính bằng bytes)
        /// </summary>
        /// <value>
        /// Số bytes >= 0. Được cập nhật tự động.
        /// Null nếu chưa tính toán. Mặc định: 0
        /// </value>
        public long? TotalSizeBytes { get; set; } = 0;

        /// <summary>
        /// Thời điểm có file upload cuối cùng vào danh mục này
        /// </summary>
        /// <value>Null nếu chưa có file nào được upload</value>
        public DateTime? LastUpload { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// Thông tin danh mục cha (nếu có)
        /// </summary>
        [ForeignKey("ParentCategoryId")]
        public virtual MediaCategory ParentCategory { get; set; }

        /// <summary>
        /// Danh sách các danh mục con
        /// </summary>
        /// <value>Collection các danh mục có ParentCategoryId trùng với CategoryId này</value>
        public virtual ICollection<MediaCategory> ChildCategories { get; set; }

        /// <summary>
        /// Danh sách tất cả file media thuộc danh mục này
        /// </summary>
        /// <value>Collection các MediaFile có CategoryId trùng với CategoryId này</value>
        public virtual ICollection<MediaFile> MediaFiles { get; set; }

        #endregion
    }
}