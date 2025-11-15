using LexiFlow.Models.Cores;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LexiFlow.Models.Learning.Vocabularys;
using LexiFlow.Models.Users;
using LexiFlow.Models.Medias;
using LexiFlow.Models.Practices;
using LexiFlow.Models.Enums.LevelEnums;

namespace LexiFlow.Models.Learning.Commons
{
    /// <summary>
    /// Danh mục từ vựng, tổ chức theo cấu trúc cây phân cấp
    /// Quản lý việc phân loại và nhóm từ vựng theo chủ đề, cấp độ và loại hình
    /// Hỗ trợ cấu trúc cha-con để tạo danh mục lồng nhau
    /// </summary>
    [Index(nameof(CategoryName), Name = "IX_Category_Name")]
    public class Category : BaseLearning
    {
        /// <summary>
        /// Tên danh mục từ vựng
        /// Ví dụ: "JLPT N5", "Đồ ăn và đồ uống", "Động từ thường dùng", "Ngữ pháp cơ bản", "Từ vựng chuyên ngành", etc.
        /// Bắt buộc, tối đa 100 ký tự
        /// </summary>
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        /// <summary>
        /// Cấp độ học tập của danh mục
        /// Ví dụ: "N5", "N4",..
        /// </summary>
        public JlptLevel Level { get; set; } = JlptLevel.None;

        /// <summary>
        /// Loại hình học tập của danh mục
        /// </summary>
        public TypeLearning CategoryType { get; set; } = TypeLearning.None;

        /// <summary>
        /// ID của danh mục cha (tùy chọn)
        /// Null = danh mục gốc, có giá trị = danh mục con
        /// Tạo cấu trúc cây phân cấp: Thực phẩm > Đồ ăn > Món Nhật
        /// </summary>
        public int? ParentCategoryId { get; set; }

        /// <summary>
        /// ID của tệp phương tiện đại diện cho danh mục (nếu có)
        /// Ví dụ: Hình ảnh biểu tượng, ảnh minh họa cho danh mục
        /// </summary>
        public int? MediaFileId { get; set; } // ID tệp phương tiện đại diện cho danh mục (nếu có)

        /// <summary>
        /// ID của phòng ban liên quan (nếu có)
        /// Dùng để phân quyền và quản lý theo phòng ban
        /// </summary>
        public int? DepartmentId { get; set; }  // Phòng ban liên quan

        /// <summary>
        /// Mã màu sắc đại diện cho danh mục
        /// Sử dụng hex color code để phân biệt danh mục bằng màu
        /// Ví dụ: "#FF5722", "#4CAF50", "#2196F3"
        /// Tối đa 20 ký tự
        /// </summary>
        [StringLength(20)]
        public string ColorCode { get; set; }

        /// <summary>
        /// Thẻ tag để gắn nhãn và tìm kiếm danh mục
        /// Lưu trữ dạng chuỗi phân cách bởi dấu phẩy
        /// Ví dụ: "jlpt,basic,food,daily-life"
        /// Tối đa 100 ký tự
        /// </summary>
        [StringLength(100)]
        public string Tags { get; set; }

        /// <summary>
        /// Có giới hạn quyền truy cập không
        /// true = danh mục có giới hạn, chỉ những user/role được phép mới truy cập được
        /// false = danh mục công khai, mọi user đều có thể truy cập
        /// Mặc định là false
        /// </summary>
        public bool IsPublic { get; set; } = false;

        /// <summary>
        /// Danh sách vai trò được phép truy cập (khi IsPublic = true)
        /// Lưu trữ dạng chuỗi JSON hoặc phân cách bởi dấu phẩy
        /// Ví dụ: "Admin,Teacher,Premium" hoặc ["Admin","Teacher","Premium"]
        /// Null khi IsPublic = false
        /// </summary>
        public string AllowedRoles { get; set; }

        /// <summary>
        /// Danh sách ID user cụ thể được phép truy cập
        /// Lưu trữ dạng chuỗi phân cách bởi dấu phẩy
        /// Ví dụ: "123,456,789" - cho phép user có ID 123, 456, 789
        /// Sử dụng cho trường hợp cần cấp quyền cá nhân
        /// </summary>
        public string AllowedUserIds { get; set; }

        /// <summary>
        /// Số lượng item (từ vựng/nội dung) trong danh mục
        /// Được tự động cập nhật khi thêm/xóa nội dung
        /// Dùng để hiển thị thống kê và sắp xếp danh mục
        /// Mặc định là 0
        /// </summary>
        public int? ItemCount { get; set; } = 0;

        // Navigation properties - Các mối quan hệ với bảng khác

        /// <summary>
        /// Thông tin danh mục cha (nếu có)
        /// Quan hệ nhiều-1 với chính bảng Category (self-reference)
        /// Dùng để tạo cấu trúc cây phân cấp
        /// </summary>
        [ForeignKey(nameof(ParentCategoryId))]
        public virtual Category ParentCategory { get; set; }

        /// <summary>
        /// Phòng ban liên quan (nếu có)
        /// </summary>
        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }

        /// <summary>
        /// Tệp phương tiện đại diện cho danh mục (nếu có)
        /// </summary>
        [ForeignKey(nameof(MediaFileId))]
        public virtual MediaFile MediaFile { get; set; }

        /// <summary>
        /// Danh sách các danh mục con
        /// Quan hệ 1-nhiều với chính bảng Category
        /// Một danh mục có thể có nhiều danh mục con
        /// </summary>
        public virtual ICollection<Category> ChildCategories { get; set; }

    }
}