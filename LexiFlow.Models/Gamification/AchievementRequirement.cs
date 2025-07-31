using LexiFlow.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Gamification
{
    /// <summary>
    /// Yêu cầu hoàn thành thành tựu
    /// </summary>
    public class AchievementRequirement : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequirementId { get; set; }

        [Required]
        public int AchievementId { get; set; }

        [StringLength(50)]
        public string RequirementType { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int? TargetValue { get; set; }

        [StringLength(50)]
        public string EntityType { get; set; }

        public int? EntityId { get; set; }

        // Cải tiến: Tiêu chí kiểm tra
        public string VerificationMethod { get; set; }
        public string VerificationQuery { get; set; }

        // Cải tiến: Giá trị và điều kiện
        [StringLength(50)]
        public string ComparisonOperator { get; set; }
        public string ValueType { get; set; }

        // Cải tiến: Áp dụng cho tier
        public int? ApplicableTier { get; set; }
        public bool ApplyToAllTiers { get; set; } = true;

        public bool IsMandatory { get; set; } = true;

        // Navigation properties
        [ForeignKey("AchievementId")]
        public virtual Achievement Achievement { get; set; }
    }
}
