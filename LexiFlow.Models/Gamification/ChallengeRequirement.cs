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
    /// Yêu cầu hoàn thành thử thách
    /// </summary>
    public class ChallengeRequirement : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequirementId { get; set; }

        [Required]
        public int ChallengeId { get; set; }

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
        public string ComparisonOperator { get; set; } // Equal, GreaterThan, LessThan, etc.
        public string ValueType { get; set; } // Count, Score, Time, etc.

        // Cải tiến: Trọng số
        public int Weight { get; set; } = 1;
        public bool CountsTowardProgress { get; set; } = true;

        public bool IsMandatory { get; set; } = true;

        // Navigation properties
        [ForeignKey("ChallengeId")]
        public virtual Challenge Challenge { get; set; }
    }
}
