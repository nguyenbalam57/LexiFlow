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
    /// Nhóm làm việc trong hệ thống
    /// </summary>
    [Index(nameof(TeamName), Name = "IX_Team_Name")]
    public class Team : AuditableEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamId { get; set; }

        [Required]
        [StringLength(100)]
        public string TeamName { get; set; }

        public int? DepartmentId { get; set; }

        public int? LeaderUserId { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        // Cải tiến: Thông tin chi tiết
        [StringLength(50)]
        public string TeamType { get; set; } // Project, Functional, Cross-functional

        [StringLength(100)]
        public string Mission { get; set; }

        public DateTime? FormationDate { get; set; }

        public DateTime? DisbandDate { get; set; }

        // Cải tiến: Giới hạn thành viên
        public int? MaxMembers { get; set; }

        public bool IsActive { get; set; } = true;

        // IActivatable implementation
        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;

        // Cải tiến: Trạng thái team
        [StringLength(50)]
        public string Status { get; set; } = "Active"; // Active, Dormant, Disbanded

        // Navigation properties
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        [ForeignKey("LeaderUserId")]
        public virtual User Leader { get; set; }

        public virtual ICollection<UserTeam> UserTeams { get; set; }
    }
}
