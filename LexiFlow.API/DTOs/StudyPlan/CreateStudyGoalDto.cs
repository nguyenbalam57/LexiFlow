using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.StudyPlan
{
    public class CreateStudyGoalDto
    {
        [Required]
        [StringLength(50)]
        public string GoalType { get; set; }

        public string Description { get; set; }

        [Required]
        public int TargetValue { get; set; }

        public DateTime? DueDate { get; set; }
    }
}
