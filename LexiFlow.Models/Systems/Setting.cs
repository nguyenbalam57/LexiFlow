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

namespace LexiFlow.Models.Systems
{
    /// <summary>
    /// Model đại diện cho cài đặt hệ thống trong ứng dụng LexiFlow.
    /// Quản lý tất cả các thiết lập cấu hình có thể thay đổi mà không cần deploy lại code.
    /// </summary>
    /// <remarks>
    /// Entity này cung cấp khả năng:
    /// - Lưu trữ cấu hình hệ thống dạng key-value với validation
    /// - Phân quyền truy cập và chỉnh sửa cài đặt
    /// - Hỗ trợ nhiều kiểu dữ liệu và validation rules
    /// - Quản lý phiên bản và lifecycle của các cài đặt
    /// - Phân nhóm và sắp xếp hiển thị trong admin panel
    /// </remarks>
    [Index(nameof(SettingKey), IsUnique = true, Name = "IX_Setting_Key")]
    public class Setting : BaseEntity
    {
        /// <summary>
        /// Khóa chính của bảng cài đặt hệ thống
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SettingId { get; set; }

        /// <summary>
        /// Khóa định danh duy nhất cho cài đặt (tên cài đặt)
        /// </summary>
        /// <value>
        /// Chuỗi không được trùng lặp, thường sử dụng format: "Group.SubGroup.SettingName"
        /// Ví dụ: "Email.Smtp.Host", "Learning.Session.MaxDuration"
        /// Tối đa 100 ký tự
        /// </value>
        [Required]
        [StringLength(100)]
        public string SettingKey { get; set; }

        /// <summary>
        /// Giá trị hiện tại của cài đặt
        /// </summary>
        /// <value>
        /// Chuỗi lưu trữ giá trị theo định dạng được chỉ định trong DataType.
        /// Có thể là: string thường, số, boolean (true/false), JSON, hoặc datetime
        /// </value>
        public string SettingValue { get; set; }

        /// <summary>
        /// Mô tả chi tiết về chức năng và tác dụng của cài đặt này
        /// </summary>
        /// <value>Văn bản giải thích cho admin hiểu rõ cài đặt này dùng để làm gì, tối đa 255 ký tự</value>
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Nhóm phân loại cài đặt để dễ quản lý và hiển thị
        /// </summary>
        /// <value>
        /// Tên nhóm như: "Email", "Security", "Learning", "UI", "Performance"
        /// Tối đa 50 ký tự
        /// </value>
        [StringLength(50)]
        public string Group { get; set; }

        #region Kiểu dữ liệu và validation

        /// <summary>
        /// Loại dữ liệu của giá trị cài đặt để validation và parse đúng cách
        /// </summary>
        /// <value>
        /// Các giá trị hợp lệ:
        /// - "String": Chuỗi văn bản thường
        /// - "Int": Số nguyên  
        /// - "Boolean": true/false
        /// - "JSON": Đối tượng JSON phức tạp
        /// - "Date": Ngày tháng format ISO
        /// - "Double": Số thực
        /// - "Email": Email hợp lệ
        /// - "Url": URL hợp lệ
        /// Tối đa 50 ký tự
        /// </value>
        [StringLength(50)]
        public string DataType { get; set; }

        /// <summary>
        /// Quy tắc validation được áp dụng cho giá trị cài đặt
        /// </summary>
        /// <value>
        /// Chuỗi JSON chứa các rule validation như:
        /// - {"required": true, "min": 1, "max": 100}
        /// - {"regex": "^[a-zA-Z0-9]+$", "length": {"min": 5, "max": 20}}
        /// - {"enum": ["option1", "option2", "option3"]}
        /// </value>
        public string ValidationRules { get; set; }

        /// <summary>
        /// Giá trị mặc định khi tạo mới hoặc reset cài đặt
        /// </summary>
        /// <value>Giá trị được sử dụng khi SettingValue bị null hoặc không hợp lệ</value>
        public string DefaultValue { get; set; }

        #endregion

        #region Hiển thị và quản lý

        /// <summary>
        /// Cho phép chỉnh sửa cài đặt này qua giao diện admin
        /// </summary>
        /// <value>
        /// - True: Có thể chỉnh sửa qua admin panel
        /// - False: Chỉ đọc, thường dành cho cài đặt hệ thống quan trọng
        /// Mặc định: true
        /// </value>
        public bool IsEditable { get; set; } = true;

        /// <summary>
        /// Hiển thị cài đặt này trong giao diện admin
        /// </summary>
        /// <value>
        /// - True: Hiển thị trong danh sách cài đặt
        /// - False: Ẩn khỏi giao diện, thường dành cho cài đặt nội bộ
        /// Mặc định: true
        /// </value>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// Yêu cầu khởi động lại ứng dụng sau khi thay đổi cài đặt này
        /// </summary>
        /// <value>
        /// - True: Cần restart để áp dụng thay đổi
        /// - False: Áp dụng ngay lập tức
        /// Mặc định: false
        /// </value>
        public bool RequiresRestart { get; set; } = false;

        /// <summary>
        /// Thứ tự hiển thị trong giao diện admin (số nhỏ hơn hiển thị trước)
        /// </summary>
        /// <value>Số nguyên dùng để sắp xếp, mặc định: 0</value>
        public int DisplayOrder { get; set; } = 0;

        #endregion

        #region Phân quyền và phạm vi

        /// <summary>
        /// Danh sách các role được phép truy cập và chỉnh sửa cài đặt này
        /// </summary>
        /// <value>
        /// Chuỗi các role phân cách bởi dấu phẩy như: "Admin,SuperAdmin,SystemManager"
        /// Null hoặc rỗng nghĩa là tất cả admin có thể truy cập
        /// </value>
        public string AccessRoles { get; set; }

        /// <summary>
        /// Cài đặt này có phạm vi toàn hệ thống hay chỉ áp dụng cho từng user
        /// </summary>
        /// <value>
        /// - True: Cài đặt chung cho toàn hệ thống
        /// - False: Mỗi user có thể có giá trị riêng
        /// Mặc định: true
        /// </value>
        public bool IsGlobal { get; set; } = true;

        /// <summary>
        /// Cho phép user ghi đè giá trị cài đặt global này trong profile cá nhân
        /// </summary>
        /// <value>
        /// - True: User có thể override trong cài đặt cá nhân
        /// - False: Bắt buộc sử dụng giá trị global
        /// Chỉ có hiệu lực khi IsGlobal = true. Mặc định: false
        /// </value>
        public bool IsUserOverridable { get; set; } = false;

        #endregion

        #region Quản lý phiên bản

        /// <summary>
        /// Phiên bản ứng dụng mà cài đặt này được thêm vào
        /// </summary>
        /// <value>
        /// Format version như: "1.0.0", "2.1.5"
        /// Dùng để tracking và migration khi upgrade
        /// </value>
        public string VersionAdded { get; set; }

        /// <summary>
        /// Phiên bản ứng dụng mà cài đặt này bị deprecated (không còn sử dụng)
        /// </summary>
        /// <value>
        /// Format version như: "3.0.0"
        /// Null nếu vẫn đang active. Dùng để chuẩn bị remove trong version tương lai
        /// </value>
        public string VersionDeprecated { get; set; }

        #endregion
    }
}