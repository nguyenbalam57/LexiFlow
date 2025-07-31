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

namespace LexiFlow.Models.Gamification
{
    /// <summary>
    /// Điểm của người dùng
    /// </summary>
    [Index(nameof(UserId), nameof(PointTypeId), Name = "IX_UserPoint_User_Type")]
    public class UserPoint : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserPointId { get; set; }

        [Required]
        public int UserId { get; set; }

        public int? PointTypeId { get; set; }

        [Required]
        public int Points { get; set; }

        public DateTime EarnedAt { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string Source { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int? RelatedEntityId { get; set; }

        [StringLength(50)]
        public string RelatedEntityType { get; set; }

        // Cải tiến: Phân loại và theo dõi
        public bool IsBonus { get; set; } = false;
        public float? Multiplier { get; set; } = 1.0f;

        // Cải tiến: Hạn chế sử dụng
        public bool IsExpirable { get; set; } = false;
        public DateTime? ExpirationDate { get; set; }

        // Cải tiến: Lịch sử chi tiêu
        public bool IsSpent { get; set; } = false;
        public DateTime? SpentAt { get; set; }
        public string SpentOn { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("PointTypeId")]
        public virtual PointType PointType { get; set; }
    }
}
