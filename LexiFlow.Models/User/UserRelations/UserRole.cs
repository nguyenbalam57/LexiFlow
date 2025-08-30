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

namespace LexiFlow.Models.User.UserRelations
{
    /// <summary>
    /// Liên kết giữa người dùng và vai trò trong hệ thống
    /// Quản lý việc gán vai trò cho người dùng với các thuộc tính mở rộng
    /// như thời gian hiệu lực, phạm vi áp dụng và người thực hiện gán
    /// </summary>
    [Index(nameof(UserId), nameof(RoleId), IsUnique = true, Name = "IX_UserRole_UserRole")]
    public class UserRole : BaseEntity
    {
        /// <summary>
        /// ID duy nhất của bản ghi liên kết User-Role (Primary Key)
        /// Được tự động tạo bởi database
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserRoleId { get; set; }

        /// <summary>
        /// ID của người dùng được gán vai trò
        /// Bắt buộc phải có, tham chiếu đến bảng Users
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// ID của vai trò được gán cho người dùng
        /// Bắt buộc phải có, tham chiếu đến bảng Roles
        /// </summary>
        [Required]
        public int RoleId { get; set; }

        /// <summary>
        /// Thời gian gán vai trò cho người dùng
        /// Được tự động thiết lập khi tạo bản ghi, sử dụng UTC
        /// Dùng để theo dõi lịch sử gán quyền
        /// </summary>
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Thời gian hết hạn của vai trò (tùy chọn)
        /// Nếu có giá trị, vai trò sẽ tự động hết hiệu lực sau thời gian này
        /// Null = vai trò không có thời hạn
        /// Hữu ích cho các vai trò tạm thời như "Thực tập sinh 3 tháng"
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// ID của người thực hiện việc gán vai trò (tùy chọn)
        /// Null = được hệ thống tự động gán hoặc gán khi tạo tài khoản
        /// Có giá trị = được admin/manager cụ thể gán
        /// Dùng để audit và truy vết trách nhiệm
        /// </summary>
        public int? AssignedBy { get; set; }

        /// <summary>
        /// Phạm vi áp dụng của vai trò (tùy chọn)
        /// Ví dụ: "Global", "Department:IT", "Project:LexiFlow", "Group:N5"
        /// Null = áp dụng toàn hệ thống
        /// Có giá trị = giới hạn phạm vi hoạt động của vai trò
        /// Tối đa 255 ký tự
        /// </summary>
        [StringLength(255)]
        public string Scope { get; set; }

        /// <summary>
        /// Trạng thái kích hoạt của việc gán vai trò
        /// true = vai trò đang có hiệu lực
        /// false = vai trò bị tạm ngưng (không xóa để giữ lịch sử)
        /// Mặc định là true
        /// </summary>
        public bool IsActive { get; set; } = true;

        // Navigation properties - Các mối quan hệ với bảng khác

        /// <summary>
        /// Thông tin người dùng được gán vai trò
        /// Quan hệ nhiều-1 với bảng Users
        /// Một user có thể có nhiều vai trò
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// Thông tin vai trò được gán
        /// Quan hệ nhiều-1 với bảng Roles
        /// Một vai trò có thể được gán cho nhiều user
        /// </summary>
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        /// <summary>
        /// Thông tin người thực hiện việc gán vai trò
        /// Quan hệ nhiều-1 với bảng Users (self-reference)
        /// Dùng để biết admin/manager nào đã gán vai trò này
        /// </summary>
        [ForeignKey("AssignedBy")]
        public virtual User AssignedByUser { get; set; }
    }
}