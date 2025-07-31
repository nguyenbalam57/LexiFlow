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
    /// Phân quyền trong hệ thống
    /// </summary>
    [Index(nameof(PermissionName), IsUnique = true, Name = "IX_Permission_Name")]
    [Index(nameof(Module), Name = "IX_Permission_Module")]
    public class Permission : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PermissionId { get; set; }

        [Required]
        [StringLength(100)]
        public string PermissionName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Module { get; set; }

        // Cải tiến: Phân loại chi tiết
        [StringLength(50)]
        public string Category { get; set; }

        // Cải tiến: Resource-based permission
        [StringLength(100)]
        public string ResourceType { get; set; }

        [StringLength(50)]
        public string Action { get; set; } // Create, Read, Update, Delete, etc.

        // Cải tiến: Quyền hệ thống
        public bool IsSystemPermission { get; set; } = false;

        // Navigation properties
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
        public virtual ICollection<UserPermission> UserPermissions { get; set; }
        public virtual ICollection<PermissionGroupMapping> PermissionGroupMappings { get; set; }
    }
}
