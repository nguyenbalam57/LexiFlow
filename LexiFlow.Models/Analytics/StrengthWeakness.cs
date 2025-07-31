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

namespace LexiFlow.Models.Analytics
{
    /// <summary>
    /// Điểm mạnh và điểm yếu của người dùng
    /// </summary>
    [Index(nameof(UserId), nameof(SkillType), nameof(SpecificSkill), IsUnique = true, Name = "IX_StrengthWeakness_User_Skill")]
    public class StrengthWeakness : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SWId { get; set; }

        [Required]
        public int UserId { get; set; }

        [StringLength(50)]
        public string SkillType { get; set; }

        [StringLength(100)]
        public string SpecificSkill { get; set; }

        public int? ProficiencyLevel { get; set; }

        public string RecommendedMaterials { get; set; }

        public string ImprovementNotes { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // Cải tiến: Chi tiết phân tích
        public bool IsStrength { get; set; } = true;
        public int? ConfidenceLevel { get; set; }
        public string EvidenceData { get; set; }

        // Cải tiến: Tiến trình cải thiện
        public int? InitialLevel { get; set; }
        public int? TargetLevel { get; set; }
        public float? ImprovementRate { get; set; }

        // Cải tiến: Gắn liên kết mục tiêu
        public int? RelatedGoalId { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("RelatedGoalId")]
        public virtual Planning.StudyGoal RelatedGoal { get; set; }
    }
}
