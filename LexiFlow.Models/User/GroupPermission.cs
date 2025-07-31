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
    /// Quyền của nhóm
    /// </summary>
    [Index(nameof(GroupId), nameof(PermissionId), IsUnique = true, Name = "IX_GroupPermission_Group_Permission")]
    public class GroupPermission : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupPermissionId { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public int PermissionId { get; set; }

        public int? GrantedByUserId { get; set; }

        public DateTime GrantedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ExpiresAt { get; set; }

        // Cải tiến: Phạm vi giới hạn
        [StringLength(255)]
        public string Scope { get; set; }

        // Cải tiến: Ghi lại lý do cấp quyền
        [StringLength(255)]
        public string GrantReason { get; set; }

        public bool IsActive { get; set; } = true;

        public string Notes { get; set; }

        // Navigation properties
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }

        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }

        [ForeignKey("GrantedByUserId")]
        public virtual User GrantedByUser { get; set; }
    }
}
