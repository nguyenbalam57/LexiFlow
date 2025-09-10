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

namespace LexiFlow.Models.Learning.Vocabulary
{
    /// <summary>
    /// Danh mục từ vựng, tổ chức theo cấu trúc cây phân cấp
    /// Quản lý việc phân loại và nhóm từ vựng theo chủ đề, cấp độ và loại hình
    /// Hỗ trợ cấu trúc cha-con để tạo danh mục lồng nhau
    /// </summary>
    [Index(nameof(CategoryName), Name = "IX_Category_Name")]
    public class Category : AuditableEntity, IActivatable
    {
        /// <summary>
        /// ID duy nhất của danh mục (Primary Key)
        /// Được tự động tạo bởi database
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        /// <summary>
        /// Tên danh mục từ vựng
        /// Ví dụ: "JLPT N5", "Đồ ăn và đồ uống", "Động từ thường dùng"
        /// Bắt buộc, tối đa 100 ký tự
        /// </summary>
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        /// <summary>
        /// Mô tả chi tiết về danh mục
        /// Giải thích nội dung, mục đích sử dụng của danh mục
        /// Tối đa 255 ký tự, có thể null
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Cấp độ học tập của danh mục
        /// Ví dụ: "N5", "N4", "Beginner", "Intermediate", "Advanced"
        /// Tối đa 20 ký tự, có thể null
        /// </summary>
        [StringLength(20)]
        public string Level { get; set; }

        /// <summary>
        /// Thứ tự hiển thị của danh mục
        /// Số nhỏ hơn sẽ hiển thị trước, dùng để sắp xếp danh mục
        /// Có thể null, khi đó sẽ sắp xếp theo tên
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Trạng thái hoạt động của danh mục
        /// true = đang hoạt động, false = bị ẩn/vô hiệu hóa
        /// Mặc định là true
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Kích hoạt danh mục - Implementation of IActivatable interface
        /// </summary>
        public void Activate()
        {
            IsActive = true;
        }

        /// <summary>
        /// Vô hiệu hóa danh mục - Implementation of IActivatable interface
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }

        /// <summary>
        /// ID của danh mục cha (tùy chọn)
        /// Null = danh mục gốc, có giá trị = danh mục con
        /// Tạo cấu trúc cây phân cấp: Thực phẩm > Đồ ăn > Món Nhật
        /// </summary>
        public int? ParentCategoryId { get; set; }

        /// <summary>
        /// Đường dẫn đến icon biểu thị danh mục
        /// Hiển thị icon trực quan cho danh mục trên giao diện
        /// Ví dụ: "/images/icons/food.png", "/icons/jlpt-n5.svg"
        /// Tối đa 255 ký tự
        /// </summary>
        [StringLength(255)]
        public string IconPath { get; set; }

        /// <summary>
        /// Mã màu sắc đại diện cho danh mục
        /// Sử dụng hex color code để phân biệt danh mục bằng màu
        /// Ví dụ: "#FF5722", "#4CAF50", "#2196F3"
        /// Tối đa 20 ký tự
        /// </summary>
        [StringLength(20)]
        public string ColorCode { get; set; }

        /// <summary>
        /// Loại danh mục (phân loại theo chức năng)
        /// Ví dụ: "Grammar" (ngữ pháp), "Vocabulary" (từ vựng), 
        /// "Kanji" (chữ Hán), "Listening" (nghe), "Reading" (đọc)
        /// Tối đa 50 ký tự
        /// </summary>
        [StringLength(50)]
        public string CategoryType { get; set; }

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
        public bool IsRestricted { get; set; } = false;

        /// <summary>
        /// Danh sách vai trò được phép truy cập (khi IsRestricted = true)
        /// Lưu trữ dạng chuỗi JSON hoặc phân cách bởi dấu phẩy
        /// Ví dụ: "Admin,Teacher,Premium" hoặc ["Admin","Teacher","Premium"]
        /// Null khi IsRestricted = false
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

        /// <summary>
        /// Thời gian cập nhật nội dung cuối cùng trong danh mục
        /// Tự động cập nhật khi có thay đổi từ vựng/nội dung bên trong
        /// Dùng để hiển thị "danh mục mới cập nhật" và sắp xếp
        /// </summary>
        public DateTime? LastUpdatedContent { get; set; }

        // Navigation properties - Các mối quan hệ với bảng khác

        /// <summary>
        /// Thông tin danh mục cha (nếu có)
        /// Quan hệ nhiều-1 với chính bảng Category (self-reference)
        /// Dùng để tạo cấu trúc cây phân cấp
        /// </summary>
        [ForeignKey("ParentCategoryId")]
        public virtual Category ParentCategory { get; set; }

        /// <summary>
        /// Danh sách các danh mục con
        /// Quan hệ 1-nhiều với chính bảng Category
        /// Một danh mục có thể có nhiều danh mục con
        /// </summary>
        public virtual ICollection<Category> ChildCategories { get; set; }

        /// <summary>
        /// Danh sách từ vựng thuộc danh mục này
        /// Quan hệ 1-nhiều với bảng Vocabulary
        /// Một danh mục chứa nhiều từ vựng
        /// </summary>
        public virtual ICollection<Vocabulary> Vocabularies { get; set; }

        /// <summary>
        /// Danh sách nhóm từ vựng thuộc danh mục
        /// Quan hệ 1-nhiều với bảng VocabularyGroup
        /// Một danh mục có thể có nhiều nhóm từ vựng con
        /// </summary>
        public virtual ICollection<VocabularyGroup> VocabularyGroups { get; set; }

        /// <summary>
        /// Danh sách kết quả bài test liên quan đến danh mục
        /// Quan hệ 1-nhiều với bảng TestResult (trong namespace Practice)
        /// Theo dõi kết quả làm bài test của danh mục này
        /// </summary>
        public virtual ICollection<Practice.TestResult> TestResults { get; set; }
    }
}