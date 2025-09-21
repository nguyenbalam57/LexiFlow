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
    /// Vai trò trong hệ thống
    /// </summary>
    [Index(nameof(RoleName), IsUnique = true, Name = "IX_Role_Name")]
    public class Role : BaseEntity
    {
        /// <summary>
        /// Khóa chính của vai trò (tự tăng).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        /// <summary>
        /// Tên vai trò (bắt buộc, tối đa 50 ký tự, duy nhất).
        /// </summary>
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        /// <summary>
        /// Mô tả chi tiết về vai trò.
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// ID của vai trò cha trong hệ thống phân cấp (nếu có).
        /// </summary>
        public int? ParentRoleId { get; set; }

        /// <summary>
        /// Mức độ ưu tiên của vai trò (số lớn hơn nghĩa là ưu tiên cao hơn).
        /// </summary>
        public int Priority { get; set; } = 0;

        /// <summary>
        /// Xác định vai trò này có phải là vai trò hệ thống hay không (không thể xóa).
        /// </summary>
        public bool IsSystemRole { get; set; } = false;

        // =================== Navigation properties ===================

        /// <summary>
        /// Vai trò cha (nếu có).
        /// </summary>
        [ForeignKey("ParentRoleId")]
        public virtual Role ParentRole { get; set; }

        /// <summary>
        /// Danh sách các vai trò con trực thuộc.
        /// </summary>
        public virtual ICollection<Role> ChildRoles { get; set; }

        /// <summary>
        /// Danh sách người dùng thuộc vai trò này.
        /// </summary>
        public virtual ICollection<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Danh sách quyền gán cho vai trò này.
        /// </summary>
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}

