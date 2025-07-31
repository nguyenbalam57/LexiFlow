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
    /// Thành viên trong team
    /// </summary>
    [Index(nameof(UserId), nameof(TeamId), IsUnique = true, Name = "IX_UserTeam_UserTeam")]
    public class UserTeam : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserTeamId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int TeamId { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        // Cải tiến: Vai trò trong team
        [StringLength(50)]
        public string Role { get; set; }

        // Cải tiến: Hạn chế thời gian
        public DateTime? ExpiresAt { get; set; }

        // Cải tiến: Người thêm vào team
        public int? AddedBy { get; set; }

        // Cải tiến: Thời gian làm việc
        [StringLength(50)]
        public string TimeCommitment { get; set; } // Full-time, Part-time, Ad-hoc

        public int? AllocationPercentage { get; set; }

        // Cải tiến: Lý do tham gia
        [StringLength(255)]
        public string JoinReason { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("TeamId")]
        public virtual Team Team { get; set; }

        [ForeignKey("AddedBy")]
        public virtual User AddedByUser { get; set; }
    }
}
