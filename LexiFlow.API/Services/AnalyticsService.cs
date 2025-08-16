using LexiFlow.Infrastructure.Data;
using LexiFlow.API.DTOs.Analytics;
using LexiFlow.API.Controllers; // Add this for GenerateReportRequestDto
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiFlow.API.Services
{
    /// <summary>
    /// Service for real-time analytics data processing and aggregation
    /// </summary>
    public interface IAnalyticsService
    {
        Task<LearningAnalyticsDashboardDto> GetLearningDashboardAsync(int userId, int days = 30);
        Task<DetailedReportDto> GenerateDetailedReportAsync(int userId, LexiFlow.API.Controllers.GenerateReportRequestDto request);
        Task<List<PerformanceTrendDto>> GetPerformanceTrendsAsync(int userId, DateTime startDate, DateTime endDate);
        Task<List<DailyStudyDto>> GetStudyPatternsAsync(int userId, DateTime startDate, DateTime endDate);
        Task<Dictionary<string, object>> GetAdvancedMetricsAsync(int userId, DateTime startDate, DateTime endDate);
        Task<List<string>> GeneratePersonalizedRecommendationsAsync(int userId);
        Task InvalidateCacheAsync(int userId);
    }

    public class AnalyticsService : IAnalyticsService
    {
        private readonly LexiFlowContext _context;
        private readonly ILogger<AnalyticsService> _logger;
        private readonly Dictionary<int, DateTime> _cacheTimestamps = new();

        public AnalyticsService(LexiFlowContext context, ILogger<AnalyticsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<LearningAnalyticsDashboardDto> GetLearningDashboardAsync(int userId, int days = 30)
        {
            try
            {
                var endDate = DateTime.UtcNow;
                var startDate = endDate.AddDays(-days);

                _logger.LogDebug("Generating analytics dashboard for user {UserId} from {StartDate} to {EndDate}", 
                    userId, startDate, endDate);

                var dashboard = new LearningAnalyticsDashboardDto
                {
                    Period = new AnalyticsPeriodDto { StartDate = startDate, EndDate = endDate },
                    OverallProgress = await GetOverallProgressAsync(userId, startDate, endDate),
                    StudyTimeAnalysis = await GetStudyTimeAnalysisAsync(userId, startDate, endDate),
                    PerformanceAnalysis = await GetPerformanceAnalysisAsync(userId, startDate, endDate),
                    GoalProgress = await GetGoalProgressAsync(userId),
                    RecentActivities = await GetRecentActivitiesAsync(userId, 10),
                    Recommendations = await GeneratePersonalizedRecommendationsAsync(userId)
                };

                _cacheTimestamps[userId] = DateTime.UtcNow;
                return dashboard;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating analytics dashboard for user {UserId}", userId);
                throw;
            }
        }

        public async Task<DetailedReportDto> GenerateDetailedReportAsync(int userId, LexiFlow.API.Controllers.GenerateReportRequestDto request)
        {
            try
            {
                var reportData = await GetAdvancedMetricsAsync(userId, request.StartDate, request.EndDate);
                var charts = await GenerateChartDataAsync(userId, request.ReportType, request.StartDate, request.EndDate);
                var summary = await GenerateReportSummaryAsync(userId, request.StartDate, request.EndDate);

                return new DetailedReportDto
                {
                    ReportType = request.ReportType,
                    Period = new AnalyticsPeriodDto { StartDate = request.StartDate, EndDate = request.EndDate },
                    ReportData = reportData,
                    Charts = charts,
                    Summary = summary,
                    GeneratedAt = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating detailed report for user {UserId}", userId);
                throw;
            }
        }

        private async Task<OverallProgressDto> GetOverallProgressAsync(int userId, DateTime startDate, DateTime endDate)
        {
            // Get study sessions in period
            var studySessions = await _context.LearningSessions
                .Where(ls => ls.UserId == userId && 
                           ls.StartTime >= startDate && 
                           ls.StartTime <= endDate)
                .ToListAsync();

            var totalStudyTime = studySessions.Sum(s => s.DurationMinutes);
            var studyDays = studySessions.Select(s => s.StartTime.Date).Distinct().Count();

            // Calculate streak
            var streakDays = await CalculateCurrentStreakAsync(userId, endDate);
            var longestStreak = await CalculateLongestStreakAsync(userId);

            // Get learning progress
            var vocabularyProgress = await _context.LearningProgresses
                .Where(lp => lp.UserId == userId && 
                           lp.LastStudied >= startDate && 
                           lp.MasteryLevel >= 4) // Considering mastery level 4+ as "learned"
                .CountAsync();

            var kanjiProgress = await _context.UserKanjiProgresses
                .Where(ukp => ukp.UserId == userId && 
                            ukp.LastPracticed >= startDate && 
                            ukp.RecognitionLevel >= 7) // Considering level 7+ as "learned"
                .CountAsync();

            var grammarProgress = await _context.UserGrammarProgresses
                .Where(ugp => ugp.UserId == userId && 
                            ugp.LastStudied >= startDate && 
                            ugp.UnderstandingLevel >= 4) // Use UnderstandingLevel instead of MasteryLevel
                .CountAsync();

            // Get test results
            var testResults = await _context.TestResults
                .Where(tr => tr.UserId == userId && 
                           tr.TestDate >= startDate && 
                           tr.TestDate <= endDate)
                .ToListAsync();

            var testsCompleted = testResults.Count;
            var averageTestScore = testsCompleted > 0 ? testResults.Average(tr => tr.Score) : 0;

            // Calculate progress rate (improvement over time)
            var progressRate = await CalculateProgressRateAsync(userId, startDate, endDate);

            return new OverallProgressDto
            {
                TotalStudyTime = totalStudyTime,
                StudyDays = studyDays,
                StreakDays = streakDays,
                LongestStreak = longestStreak,
                VocabularyLearned = vocabularyProgress,
                KanjiLearned = kanjiProgress,
                GrammarLearned = grammarProgress,
                TestsCompleted = testsCompleted,
                AverageTestScore = (float)averageTestScore,
                ProgressRate = progressRate
            };
        }

        private async Task<StudyTimeAnalysisDto> GetStudyTimeAnalysisAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var studySessions = await _context.LearningSessions
                .Where(ls => ls.UserId == userId && 
                           ls.StartTime >= startDate && 
                           ls.StartTime <= endDate)
                .ToListAsync();

            var totalMinutes = studySessions.Sum(s => s.DurationMinutes);
            var days = (endDate - startDate).Days + 1;
            var dailyAverage = days > 0 ? totalMinutes / days : 0;

            // Weekly and monthly totals
            var weeklyTotal = studySessions
                .Where(s => s.StartTime >= endDate.AddDays(-7))
                .Sum(s => s.DurationMinutes);

            var monthlyTotal = studySessions
                .Where(s => s.StartTime >= endDate.AddDays(-30))
                .Sum(s => s.DurationMinutes);

            // Peak study hour analysis
            var hourlyData = studySessions
                .GroupBy(s => s.StartTime.Hour)
                .Select(g => new { Hour = g.Key, Minutes = g.Sum(s => s.DurationMinutes) })
                .OrderByDescending(h => h.Minutes)
                .ToList();

            var peakStudyHour = hourlyData.FirstOrDefault()?.Hour ?? 12;

            // Most/least active days
            var dailyData = studySessions
                .GroupBy(s => s.StartTime.DayOfWeek)
                .Select(g => new { DayOfWeek = g.Key, Minutes = g.Sum(s => s.DurationMinutes) })
                .ToList();

            var mostActiveDay = dailyData.OrderByDescending(d => d.Minutes).FirstOrDefault()?.DayOfWeek.ToString() ?? "Monday";
            var leastActiveDay = dailyData.OrderBy(d => d.Minutes).FirstOrDefault()?.DayOfWeek.ToString() ?? "Sunday";

            // Time distribution by session type
            var timeDistribution = studySessions
                .Where(s => !string.IsNullOrEmpty(s.SessionType))
                .GroupBy(s => s.SessionType)
                .ToDictionary(
                    g => g.Key,
                    g => (int)(g.Sum(s => s.DurationMinutes) * 100.0 / totalMinutes)
                );

            // Generate daily pattern
            var dailyPattern = hourlyData.Select(h => new HourlyStudyDto
            {
                Hour = h.Hour,
                Minutes = h.Minutes,
                Sessions = studySessions.Count(s => s.StartTime.Hour == h.Hour)
            }).ToList();

            // Generate weekly pattern
            var weeklyPattern = await GetWeeklyPatternAsync(userId, startDate, endDate);

            return new StudyTimeAnalysisDto
            {
                DailyAverage = dailyAverage,
                WeeklyTotal = weeklyTotal,
                MonthlyTotal = monthlyTotal,
                PeakStudyHour = peakStudyHour,
                MostActiveDay = mostActiveDay,
                LeastActiveDay = leastActiveDay,
                TimeDistribution = timeDistribution,
                DailyPattern = dailyPattern,
                WeeklyPattern = weeklyPattern
            };
        }

        private async Task<AnalyticsPerformanceDto> GetPerformanceAnalysisAsync(int userId, DateTime startDate, DateTime endDate)
        {
            // Get test results for accuracy calculation
            var testResults = await _context.TestResults
                .Where(tr => tr.UserId == userId && 
                           tr.TestDate >= startDate && 
                           tr.TestDate <= endDate)
                .ToListAsync();

            var overallAccuracy = testResults.Any() ? (float)testResults.Average(tr => tr.Score) : 0f;

            // Calculate improvement rate
            var improvementRate = await CalculateImprovementRateAsync(userId, startDate, endDate);

            // Get strength and weak areas
            var strengthAreas = await GetSkillAreasAsync(userId, startDate, endDate, true);
            var weakAreas = await GetSkillAreasAsync(userId, startDate, endDate, false);

            // Get performance trends
            var performanceTrends = await GetPerformanceTrendsAsync(userId, startDate, endDate);

            // Skill breakdown
            var skillBreakdown = await GetSkillBreakdownAsync(userId, startDate, endDate);

            return new AnalyticsPerformanceDto
            {
                OverallAccuracy = overallAccuracy,
                ImprovementRate = improvementRate,
                StrengthAreas = strengthAreas,
                WeakAreas = weakAreas,
                PerformanceTrends = performanceTrends,
                SkillBreakdown = skillBreakdown
            };
        }

        private async Task<List<StudyGoalProgressDto>> GetGoalProgressAsync(int userId)
        {
            var goals = await _context.StudyGoals
                .Where(sg => sg.StudyPlan.UserId == userId && sg.IsActive)
                .Include(sg => sg.StudyPlan)
                .ToListAsync();

            return goals.Select(g => new StudyGoalProgressDto
            {
                GoalId = g.GoalId,
                GoalName = g.GoalName,
                GoalType = g.GoalType ?? "General",
                ProgressPercentage = g.ProgressPercentage,
                TargetDate = g.TargetDate,
                IsCompleted = g.IsCompleted,
                IsOverdue = g.TargetDate.HasValue && DateTime.UtcNow > g.TargetDate.Value && !g.IsCompleted,
                DaysRemaining = g.TargetDate.HasValue ? Math.Max(0, (int)(g.TargetDate.Value - DateTime.UtcNow).TotalDays) : 0
            }).ToList();
        }

        private async Task<List<RecentActivityDto>> GetRecentActivitiesAsync(int userId, int count)
        {
            var activities = new List<RecentActivityDto>();

            // Get recent learning sessions
            var sessions = await _context.LearningSessions
                .Where(ls => ls.UserId == userId)
                .OrderByDescending(ls => ls.StartTime)
                .Take(count / 2)
                .ToListAsync();

            activities.AddRange(sessions.Select(s => new RecentActivityDto
            {
                Timestamp = s.StartTime,
                ActivityType = s.SessionType ?? "Study",
                Description = $"Study session: {s.SessionType}",
                Score = null,
                DurationMinutes = s.DurationMinutes
            }));

            // Get recent test results
            var testResults = await _context.TestResults
                .Where(tr => tr.UserId == userId)
                .OrderByDescending(tr => tr.TestDate)
                .Take(count / 2)
                .ToListAsync();

            activities.AddRange(testResults.Select(tr => new RecentActivityDto
            {
                Timestamp = tr.TestDate,
                ActivityType = "Test",
                Description = $"{tr.TestType} Test",
                Score = (int)tr.Score,
                DurationMinutes = tr.DurationMinutes ?? 0
            }));

            return activities.OrderByDescending(a => a.Timestamp).Take(count).ToList();
        }

        public async Task<List<string>> GeneratePersonalizedRecommendationsAsync(int userId)
        {
            var recommendations = new List<string>();

            try
            {
                // Analyze weak areas
                var weakAreas = await GetSkillAreasAsync(userId, DateTime.UtcNow.AddDays(-30), DateTime.UtcNow, false);
                if (weakAreas.Any())
                {
                    var weakestSkill = weakAreas.OrderBy(wa => wa.AccuracyRate).First();
                    recommendations.Add($"Focus more on {weakestSkill.SkillName} practice - your weakest area ({weakestSkill.AccuracyRate:F1}% accuracy)");
                }

                // Analyze study consistency
                var recentSessions = await _context.LearningSessions
                    .Where(ls => ls.UserId == userId && ls.StartTime >= DateTime.UtcNow.AddDays(-7))
                    .CountAsync();

                if (recentSessions < 3)
                {
                    recommendations.Add("Try to study more consistently - aim for at least 4-5 sessions per week");
                }

                // Analyze performance trends
                var improvementRate = await CalculateImprovementRateAsync(userId, DateTime.UtcNow.AddDays(-30), DateTime.UtcNow);
                if (improvementRate > 10)
                {
                    recommendations.Add("Great progress! Consider taking a practice test to assess your current level");
                }

                // Goal-based recommendations
                var overdueGoals = await _context.StudyGoals
                    .Where(sg => sg.StudyPlan.UserId == userId && 
                               sg.TargetDate < DateTime.UtcNow && 
                               !sg.IsCompleted)
                    .CountAsync();

                if (overdueGoals > 0)
                {
                    recommendations.Add($"You have {overdueGoals} overdue goal(s). Consider reviewing and updating your study plan");
                }

                // Default recommendations if none generated
                if (!recommendations.Any())
                {
                    recommendations.AddRange(new[]
                    {
                        "Keep up the good work with your studies!",
                        "Try mixing different study methods for better retention",
                        "Regular review sessions help consolidate your learning"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating recommendations for user {UserId}", userId);
                recommendations.Add("Continue with your current study routine - you're doing great!");
            }

            return recommendations;
        }

        // Helper methods
        private async Task<int> CalculateCurrentStreakAsync(int userId, DateTime endDate)
        {
            var sessions = await _context.LearningSessions
                .Where(ls => ls.UserId == userId && ls.StartTime <= endDate)
                .Select(ls => ls.StartTime.Date)
                .Distinct()
                .OrderByDescending(d => d)
                .ToListAsync();

            int streak = 0;
            var currentDate = endDate.Date;

            foreach (var sessionDate in sessions)
            {
                if (sessionDate == currentDate || sessionDate == currentDate.AddDays(-1))
                {
                    streak++;
                    currentDate = sessionDate.AddDays(-1);
                }
                else
                {
                    break;
                }
            }

            return streak;
        }

        private async Task<int> CalculateLongestStreakAsync(int userId)
        {
            var sessions = await _context.LearningSessions
                .Where(ls => ls.UserId == userId)
                .Select(ls => ls.StartTime.Date)
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync();

            int longestStreak = 0;
            int currentStreak = 0;
            DateTime? lastDate = null;

            foreach (var sessionDate in sessions)
            {
                if (lastDate == null || sessionDate == lastDate.Value.AddDays(1))
                {
                    currentStreak++;
                }
                else
                {
                    longestStreak = Math.Max(longestStreak, currentStreak);
                    currentStreak = 1;
                }
                lastDate = sessionDate;
            }

            return Math.Max(longestStreak, currentStreak);
        }

        private async Task<float> CalculateProgressRateAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var startProgress = await _context.LearningProgresses
                .Where(lp => lp.UserId == userId && lp.LastStudied <= startDate)
                .AverageAsync(lp => (double?)lp.MasteryLevel) ?? 0;

            var endProgress = await _context.LearningProgresses
                .Where(lp => lp.UserId == userId && lp.LastStudied <= endDate)
                .AverageAsync(lp => (double?)lp.MasteryLevel) ?? 0;

            return startProgress > 0 ? (float)((endProgress - startProgress) / startProgress * 100) : 0;
        }

        private async Task<float> CalculateImprovementRateAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var midPoint = startDate.AddDays((endDate - startDate).Days / 2);

            var firstHalfAvg = await _context.TestResults
                .Where(tr => tr.UserId == userId && tr.TestDate >= startDate && tr.TestDate < midPoint)
                .AverageAsync(tr => (double?)tr.Score) ?? 0;

            var secondHalfAvg = await _context.TestResults
                .Where(tr => tr.UserId == userId && tr.TestDate >= midPoint && tr.TestDate <= endDate)
                .AverageAsync(tr => (double?)tr.Score) ?? 0;

            return firstHalfAvg > 0 ? (float)((secondHalfAvg - firstHalfAvg) / firstHalfAvg * 100) : 0;
        }

        private async Task<List<SkillAreaDto>> GetSkillAreasAsync(int userId, DateTime startDate, DateTime endDate, bool isStrength)
        {
            var testDetails = await _context.TestDetails
                .Include(td => td.TestResult)
                .Where(td => td.TestResult.UserId == userId && 
                           td.TestResult.TestDate >= startDate && 
                           td.TestResult.TestDate <= endDate)
                .ToListAsync();

            var skillGroups = testDetails
                .GroupBy(td => td.SkillType ?? "General")
                .Select(g => new SkillAreaDto
                {
                    SkillName = g.Key,
                    AccuracyRate = (float)g.Average(td => td.IsCorrect ? 100 : 0),
                    TotalAttempts = g.Count(),
                    ImprovementRate = 0, // TODO: Calculate based on time trends
                    Level = "N3" // TODO: Determine based on content difficulty
                })
                .ToList();

            return isStrength 
                ? skillGroups.OrderByDescending(s => s.AccuracyRate).Take(3).ToList()
                : skillGroups.OrderBy(s => s.AccuracyRate).Take(3).ToList();
        }

        public async Task<List<PerformanceTrendDto>> GetPerformanceTrendsAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var testResults = await _context.TestResults
                .Where(tr => tr.UserId == userId && 
                           tr.TestDate >= startDate && 
                           tr.TestDate <= endDate)
                .OrderBy(tr => tr.TestDate)
                .ToListAsync();

            return testResults.Select(tr => new PerformanceTrendDto
            {
                Date = tr.TestDate,
                AccuracyRate = (float)tr.Score,
                StudyMinutes = tr.DurationMinutes ?? 0,
                ItemsLearned = 0 // TODO: Calculate based on session data
            }).ToList();
        }

        public async Task<List<DailyStudyDto>> GetStudyPatternsAsync(int userId, DateTime startDate, DateTime endDate)
        {
            return await GetWeeklyPatternAsync(userId, startDate, endDate);
        }

        private async Task<List<DailyStudyDto>> GetWeeklyPatternAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var sessions = await _context.LearningSessions
                .Where(ls => ls.UserId == userId && 
                           ls.StartTime >= startDate && 
                           ls.StartTime <= endDate)
                .ToListAsync();

            var groupedSessions = sessions
                .GroupBy(s => s.StartTime.Date)
                .Select(g => new DailyStudyDto
                {
                    Date = g.Key,
                    Minutes = g.Sum(s => s.DurationMinutes),
                    Sessions = g.Count(),
                    VocabularyItems = 0, // TODO: Get from session details
                    KanjiItems = 0,
                    GrammarItems = 0
                })
                .OrderBy(d => d.Date)
                .ToList();

            return groupedSessions;
        }

        private async Task<Dictionary<string, float>> GetSkillBreakdownAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var skillAreas = await GetSkillAreasAsync(userId, startDate, endDate, true);
            skillAreas.AddRange(await GetSkillAreasAsync(userId, startDate, endDate, false));

            return skillAreas.GroupBy(sa => sa.SkillName)
                .ToDictionary(g => g.Key, g => g.First().AccuracyRate);
        }

        private async Task<List<ChartDataDto>> GenerateChartDataAsync(int userId, string reportType, DateTime startDate, DateTime endDate)
        {
            var trends = await GetPerformanceTrendsAsync(userId, startDate, endDate);
            
            return new List<ChartDataDto>
            {
                new ChartDataDto
                {
                    ChartType = "line",
                    Title = "Performance Over Time",
                    DataPoints = trends.Select(t => new DataPointDto
                    {
                        Label = t.Date.ToString("MM/dd"),
                        Value = t.AccuracyRate,
                        Timestamp = t.Date
                    }).ToList()
                }
            };
        }

        private async Task<List<string>> GenerateReportSummaryAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var summary = new List<string>();

            var totalStudyTime = await _context.LearningSessions
                .Where(ls => ls.UserId == userId && ls.StartTime >= startDate && ls.StartTime <= endDate)
                .SumAsync(ls => ls.DurationMinutes);

            var averageScore = await _context.TestResults
                .Where(tr => tr.UserId == userId && tr.TestDate >= startDate && tr.TestDate <= endDate)
                .AverageAsync(tr => (double?)tr.Score) ?? 0;

            summary.Add($"Total study time: {totalStudyTime} minutes over {(endDate - startDate).Days} days");
            summary.Add($"Average test score: {averageScore:F1}%");

            if (averageScore > 80)
                summary.Add("Excellent performance! Keep up the great work.");
            else if (averageScore > 60)
                summary.Add("Good progress. Focus on weak areas for improvement.");
            else
                summary.Add("Consider adjusting study methods and increasing practice time.");

            return summary;
        }

        public async Task<Dictionary<string, object>> GetAdvancedMetricsAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var metrics = new Dictionary<string, object>();

            // Study efficiency (score improvement per hour studied)
            var totalStudyHours = await _context.LearningSessions
                .Where(ls => ls.UserId == userId && ls.StartTime >= startDate && ls.StartTime <= endDate)
                .SumAsync(ls => ls.DurationMinutes) / 60.0;

            var scoreImprovement = await CalculateImprovementRateAsync(userId, startDate, endDate);
            
            metrics["studyEfficiency"] = totalStudyHours > 0 ? scoreImprovement / totalStudyHours : 0;
            metrics["totalStudyHours"] = totalStudyHours;
            metrics["scoreImprovement"] = scoreImprovement;

            // Retention rate (how well knowledge is maintained over time)
            var retentionRate = await CalculateRetentionRateAsync(userId, startDate, endDate);
            metrics["retentionRate"] = retentionRate;

            // Learning velocity (new items learned per day)
            var newItemsLearned = await _context.LearningProgresses
                .Where(lp => lp.UserId == userId && lp.FirstStudied >= startDate && lp.FirstStudied <= endDate)
                .CountAsync();

            metrics["learningVelocity"] = newItemsLearned / (double)(endDate - startDate).Days;
            metrics["newItemsLearned"] = newItemsLearned;

            return metrics;
        }

        private async Task<double> CalculateRetentionRateAsync(int userId, DateTime startDate, DateTime endDate)
        {
            // Items learned before the period that were still remembered during the period
            var itemsLearnedBefore = await _context.LearningProgresses
                .Where(lp => lp.UserId == userId && 
                           lp.FirstStudied < startDate && 
                           lp.MasteryLevel >= 4)
                .CountAsync();

            var itemsRetained = await _context.LearningProgresses
                .Where(lp => lp.UserId == userId && 
                           lp.FirstStudied < startDate && 
                           lp.LastStudied >= startDate && 
                           lp.MasteryLevel >= 4)
                .CountAsync();

            return itemsLearnedBefore > 0 ? (double)itemsRetained / itemsLearnedBefore * 100 : 100;
        }

        public async Task InvalidateCacheAsync(int userId)
        {
            _cacheTimestamps.Remove(userId);
            _logger.LogDebug("Cache invalidated for user {UserId}", userId);
        }
    }
}