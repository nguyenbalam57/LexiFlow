namespace LexiFlow.API.DTOs.StudyPlan
{
    public class StudyPlanDto
    {
        public int PlanID { get; set; }
        public string PlanName { get; set; }
        public string TargetLevel { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? TargetDate { get; set; }
        public string Description { get; set; }
        public int? MinutesPerDay { get; set; }
        public bool IsActive { get; set; }
        public string CurrentStatus { get; set; }
        public float CompletionPercentage { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<StudyGoalDto> Goals { get; set; } = new List<StudyGoalDto>();
    }
}
