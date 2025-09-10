using LexiFlow.Models.Core;
using LexiFlow.Models.User.UserRelations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.User
{
    /// <summary>
    /// Vai trò trong hệ thống
    /// </summary>
    [Index(nameof(RoleName), IsUnique = true, Name = "IX_Role_Name")]
    public class Role : BaseEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        // Cải tiến: Phân cấp vai trò
        public int? ParentRoleId { get; set; }

        // Cải tiến: Mức độ ưu tiên
        public int Priority { get; set; } = 0;

        // Cải tiến: Vai trò hệ thống không thể xóa
        public bool IsSystemRole { get; set; } = false;

        public bool IsActive { get; set; } = true;

        // IActivatable implementation
        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;

        // Navigation properties
        [ForeignKey("ParentRoleId")]
        public virtual Role ParentRole { get; set; }

        public virtual ICollection<Role> ChildRoles { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
