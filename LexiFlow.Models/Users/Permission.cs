using LexiFlow.Models.Cores;
using LexiFlow.Models.Users.UserRelations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Users
{
    /// <summary>
    /// Phân quyền trong hệ thống
    /// </summary>
    [Index(nameof(PermissionName), IsUnique = true, Name = "IX_Permission_Name")]
    [Index(nameof(Module), Name = "IX_Permission_Module")]
    public class Permission : BaseEntity
    {
        /// <summary>
        /// Khóa chính của quyền (tự tăng).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PermissionId { get; set; }

        /// <summary>
        /// Tên quyền (bắt buộc, tối đa 100 ký tự, duy nhất).
        /// </summary>
        [Required]
        [StringLength(100)]
        public string PermissionName { get; set; }

        /// <summary>
        /// Mô tả chi tiết về quyền.
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Tên module mà quyền này thuộc về (ví dụ: User, Document, Report...).
        /// </summary>
        [StringLength(50)]
        public string Module { get; set; }

        /// <summary>
        /// Nhóm phân loại chi tiết của quyền (ví dụ: Quản trị, Nghiệp vụ...).
        /// </summary>
        [StringLength(50)]
        public string Category { get; set; }

        /// <summary>
        /// Loại tài nguyên mà quyền áp dụng (ví dụ: File, Project, User...).
        /// </summary>
        [StringLength(100)]
        public string ResourceType { get; set; }

        /// <summary>
        /// Hành động được cho phép (Create, Read, Update, Delete...).
        /// </summary>
        [StringLength(50)]
        public string Action { get; set; }

        /// <summary>
        /// Xác định đây có phải là quyền hệ thống hay không (mặc định: false).
        /// </summary>
        public bool IsSystemPermission { get; set; } = false;

        // =================== Navigation properties ===================

        /// <summary>
        /// Danh sách quyền gán cho vai trò.
        /// </summary>
        public virtual ICollection<RolePermission> RolePermissions { get; set; }

        /// <summary>
        /// Danh sách quyền gán trực tiếp cho người dùng.
        /// </summary>
        public virtual ICollection<UserPermission> UserPermissions { get; set; }
    }
}

