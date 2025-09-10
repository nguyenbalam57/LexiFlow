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
    /// Phân nhóm quyền hạn trong hệ thống
    /// </summary>
    [Index(nameof(GroupName), IsUnique = true, Name = "IX_PermissionGroup_Name")]
    public class PermissionGroup : BaseEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PermissionGroupId { get; set; }

        [Required]
        [StringLength(100)]
        public string GroupName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(50)]
        public string ModuleArea { get; set; }

        public int DisplayOrder { get; set; } = 0;

        // Cải tiến: Hiển thị và phân loại
        [StringLength(255)]
        public string IconPath { get; set; }

        [StringLength(20)]
        public string ColorCode { get; set; }

        // Cải tiến: Phân cấp nhóm quyền
        public int? ParentGroupId { get; set; }

        // Cải tiến: Quản lý hệ thống
        public bool IsSystemGroup { get; set; } = false;

        public bool IsActive { get; set; } = true;

        // IActivatable implementation
        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;

        // Navigation properties
        [ForeignKey("ParentGroupId")]
        public virtual PermissionGroup ParentGroup { get; set; }

        public virtual ICollection<PermissionGroup> ChildGroups { get; set; }
        public virtual ICollection<PermissionGroupMapping> PermissionMappings { get; set; }
    }
}
