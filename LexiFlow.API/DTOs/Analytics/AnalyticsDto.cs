using System;
using System.Collections.Generic;

namespace LexiFlow.API.DTOs.Analytics
{
    /// <summary>
    /// Base DTO cho các thông tin phân tích
    /// </summary>
    public class AnalyticsPeriodDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Days => (EndDate - StartDate).Days + 1;
    }

    /// <summary>
    /// DTO cho t?ng quan ti?n ?? h?c t?p
    /// </summary>
    public class OverallProgressDto
    {
        public int TotalStudyTime { get; set; } // minutes
        public int StudyDays { get; set; }
        public int StreakDays { get; set; }
        public int LongestStreak { get; set; }
        public int VocabularyLearned { get; set; }
        public int KanjiLearned { get; set; }
        public int GrammarLearned { get; set; }
        public int TestsCompleted { get; set; }
        public float AverageTestScore { get; set; }
        public float ProgressRate { get; set; }
    }

    /// <summary>
    /// DTO cho phân tích th?i gian h?c t?p
    /// </summary>
    public class StudyTimeAnalysisDto
    {
        public int DailyAverage { get; set; } // minutes
        public int WeeklyTotal { get; set; } // minutes
        public int MonthlyTotal { get; set; } // minutes
        public int PeakStudyHour { get; set; } // 0-23
        public string MostActiveDay { get; set; }
        public string LeastActiveDay { get; set; }
        public Dictionary<string, int> TimeDistribution { get; set; }
        public List<HourlyStudyDto> DailyPattern { get; set; }
        public List<DailyStudyDto> WeeklyPattern { get; set; }
    }

    /// <summary>
    /// DTO cho d? li?u h?c t?p theo gi?
    /// </summary>
    public class HourlyStudyDto
    {
        public int Hour { get; set; }
        public int Minutes { get; set; }
        public int Sessions { get; set; }
    }

    /// <summary>
    /// DTO cho d? li?u h?c t?p theo ngày
    /// </summary>
    public class DailyStudyDto
    {
        public DateTime Date { get; set; }
        public int Minutes { get; set; }
        public int Sessions { get; set; }
        public int VocabularyItems { get; set; }
        public int KanjiItems { get; set; }
        public int GrammarItems { get; set; }
    }

    /// <summary>
    /// DTO cho phân tích hi?u su?t
    /// </summary>
    public class AnalyticsPerformanceDto
    {
        public float OverallAccuracy { get; set; }
        public float ImprovementRate { get; set; }
        public List<SkillAreaDto> StrengthAreas { get; set; }
        public List<SkillAreaDto> WeakAreas { get; set; }
        public List<PerformanceTrendDto> PerformanceTrends { get; set; }
        public Dictionary<string, float> SkillBreakdown { get; set; }
    }

    /// <summary>
    /// DTO cho khu v?c k? n?ng
    /// </summary>
    public class SkillAreaDto
    {
        public string SkillName { get; set; }
        public float AccuracyRate { get; set; }
        public int TotalAttempts { get; set; }
        public float ImprovementRate { get; set; }
        public string Level { get; set; }
    }

    /// <summary>
    /// DTO cho xu h??ng hi?u su?t
    /// </summary>
    public class PerformanceTrendDto
    {
        public DateTime Date { get; set; }
        public float AccuracyRate { get; set; }
        public int StudyMinutes { get; set; }
        public int ItemsLearned { get; set; }
    }

    /// <summary>
    /// DTO t?ng h?p cho dashboard phân tích h?c t?p
    /// </summary>
    public class LearningAnalyticsDashboardDto
    {
        public AnalyticsPeriodDto Period { get; set; }
        public OverallProgressDto OverallProgress { get; set; }
        public StudyTimeAnalysisDto StudyTimeAnalysis { get; set; }
        public AnalyticsPerformanceDto PerformanceAnalysis { get; set; }
        public List<StudyGoalProgressDto> GoalProgress { get; set; }
        public List<RecentActivityDto> RecentActivities { get; set; }
        public List<string> Recommendations { get; set; }
    }

    /// <summary>
    /// DTO cho ti?n ?? m?c tiêu h?c t?p
    /// </summary>
    public class StudyGoalProgressDto
    {
        public int GoalId { get; set; }
        public string GoalName { get; set; }
        public string GoalType { get; set; }
        public float ProgressPercentage { get; set; }
        public DateTime? TargetDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsOverdue { get; set; }
        public int DaysRemaining { get; set; }
    }

    /// <summary>
    /// DTO cho ho?t ??ng g?n ?ây
    /// </summary>
    public class RecentActivityDto
    {
        public DateTime Timestamp { get; set; }
        public string ActivityType { get; set; }
        public string Description { get; set; }
        public int? Score { get; set; }
        public int DurationMinutes { get; set; }
    }

    /// <summary>
    /// DTO cho báo cáo chi ti?t
    /// </summary>
    public class DetailedReportDto
    {
        public string ReportType { get; set; }
        public AnalyticsPeriodDto Period { get; set; }
        public Dictionary<string, object> ReportData { get; set; }
        public List<ChartDataDto> Charts { get; set; }
        public List<string> Summary { get; set; }
        public DateTime GeneratedAt { get; set; }
    }

    /// <summary>
    /// DTO cho d? li?u bi?u ??
    /// </summary>
    public class ChartDataDto
    {
        public string ChartType { get; set; }
        public string Title { get; set; }
        public List<DataPointDto> DataPoints { get; set; }
        public Dictionary<string, string> Options { get; set; }
    }

    /// <summary>
    /// DTO cho ?i?m d? li?u
    /// </summary>
    public class DataPointDto
    {
        public string Label { get; set; }
        public double Value { get; set; }
        public string Category { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}