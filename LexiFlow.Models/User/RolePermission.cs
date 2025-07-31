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

namespace LexiFlow.Models.User
{
    /// <summary>
    /// Quan hệ giữa vai trò và phân quyền
    /// </summary>
    [Index(nameof(RoleId), nameof(PermissionId), IsUnique = true, Name = "IX_RolePermission_RolePermission")]
    public class RolePermission : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RolePermissionId { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public int PermissionId { get; set; }

        // Cải tiến: Phạm vi giới hạn
        public string Scope { get; set; }

        // Cải tiến: Quyết định override
        public bool IsOverride { get; set; } = false;

        // Cải tiến: Theo dõi người cấp quyền
        public int? GrantedBy { get; set; }

        // Navigation properties
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }

        [ForeignKey("GrantedBy")]
        public virtual User GrantedByUser { get; set; }
    }
}
