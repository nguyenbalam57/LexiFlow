using LexiFlow.Models.Core;
using LexiFlow.Models.Planning;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Progress
{
    /// <summary>
    /// Lịch sử tiến trình mục tiêu
    /// </summary>
    public class GoalProgress : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProgressId { get; set; }

        [Required]
        public int GoalId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int Value { get; set; }

        public float PercentageComplete { get; set; }

        [StringLength(255)]
        public string Notes { get; set; }

        // Navigation property
        [ForeignKey("GoalId")]
        public virtual StudyGoal Goal { get; set; }
    }
}
