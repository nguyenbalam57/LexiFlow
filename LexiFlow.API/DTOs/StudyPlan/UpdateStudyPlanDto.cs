using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.StudyPlan
{
    public class UpdateStudyPlanDto
    {
        [StringLength(100)]
        public string PlanName { get; set; }

        [StringLength(10)]
        public string TargetLevel { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? TargetDate { get; set; }

        public string Description { get; set; }

        public int? MinutesPerDay { get; set; }

        public bool? IsActive { get; set; }

        public string RowVersionString { get; set; }
    }
}
