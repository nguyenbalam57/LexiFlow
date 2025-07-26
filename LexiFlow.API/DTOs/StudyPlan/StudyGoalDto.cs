namespace LexiFlow.API.DTOs.StudyPlan
{
    public class StudyGoalDto
    {
        public int GoalID { get; set; }
        public int PlanID { get; set; }
        public string GoalType { get; set; }
        public string Description { get; set; }
        public int TargetValue { get; set; }
        public int CurrentValue { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
