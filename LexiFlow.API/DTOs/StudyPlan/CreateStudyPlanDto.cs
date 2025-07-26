using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.StudyPlan
{
    public class CreateStudyPlanDto
    {
        [Required]
        [StringLength(100)]
        public string PlanName { get; set; }

        [StringLength(10)]
        public string TargetLevel { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? TargetDate { get; set; }

        public string Description { get; set; }

        public int? MinutesPerDay { get; set; }

        public List<CreateStudyGoalDto> Goals { get; set; } = new List<CreateStudyGoalDto>();
    }
}
