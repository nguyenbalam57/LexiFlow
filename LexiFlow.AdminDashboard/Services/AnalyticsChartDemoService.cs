using LexiFlow.AdminDashboard.Models.Charts;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LexiFlow.AdminDashboard.Services
{
    /// <summary>
    /// Service for generating demo chart data
    /// </summary>
    public class AnalyticsChartDemoService
    {
        /// <summary>
        /// Generate comprehensive demo data for testing charts
        /// </summary>
        public static StudyTimeSeriesData GenerateDemoData(int days = 30)
        {
            var data = new StudyTimeSeriesData();
            var random = new Random();
            var startDate = DateTime.Now.AddDays(-days);

            // Generate daily study time data with realistic patterns
            for (int i = 0; i < days; i++)
            {
                var date = startDate.AddDays(i);
                
                // More study time on weekends, less on weekdays
                var isWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
                var baseTime = isWeekend ? random.Next(60, 150) : random.Next(30, 90);
                
                // Add some variation
                var variation = random.Next(-15, 25);
                var studyTime = Math.Max(0, baseTime + variation);

                data.DailyStudyTime.Add(new ChartDataPoint
                {
                    Date = date,
                    Value = studyTime,
                    Label = date.ToString("MM/dd"),
                    Category = "StudyTime"
                });
            }

            // Generate vocabulary progress (cumulative)
            var vocabularyCount = 0;
            for (int i = 0; i < days; i++)
            {
                var date = startDate.AddDays(i);
                vocabularyCount += random.Next(3, 12); // 3-12 new words per day
                
                data.VocabularyProgress.Add(new ChartDataPoint
                {
                    Date = date,
                    Value = vocabularyCount,
                    Label = date.ToString("MM/dd"),
                    Category = "Vocabulary"
                });
            }

            // Generate Kanji progress (cumulative)
            var kanjiCount = 0;
            for (int i = 0; i < days; i++)
            {
                var date = startDate.AddDays(i);
                kanjiCount += random.Next(1, 5); // 1-5 new kanji per day
                
                data.KanjiProgress.Add(new ChartDataPoint
                {
                    Date = date,
                    Value = kanjiCount,
                    Label = date.ToString("MM/dd"),
                    Category = "Kanji"
                });
            }

            // Generate Grammar progress (cumulative)
            var grammarCount = 0;
            for (int i = 0; i < days; i++)
            {
                var date = startDate.AddDays(i);
                if (i % 2 == 0) // Grammar points every other day
                {
                    grammarCount += random.Next(1, 3);
                }
                
                data.GrammarProgress.Add(new ChartDataPoint
                {
                    Date = date,
                    Value = grammarCount,
                    Label = date.ToString("MM/dd"),
                    Category = "Grammar"
                });
            }

            // Generate performance trends for each skill
            var skills = new[] { "Vocabulary", "Kanji", "Grammar", "Reading", "Listening" };
            var baseAccuracies = new[] { 85f, 82f, 75f, 78f, 70f };

            for (int skillIndex = 0; skillIndex < skills.Length; skillIndex++)
            {
                var baseAccuracy = baseAccuracies[skillIndex];
                var currentAccuracy = baseAccuracy;
                
                for (int i = 0; i < days; i += 2) // Data points every 2 days
                {
                    var date = startDate.AddDays(i);
                    
                    // Simulate improvement over time with some fluctuation
                    var improvement = (i / (float)days) * 5; // Up to 5% improvement over the period
                    var dailyVariation = (random.NextDouble() - 0.5) * 8; // ±4% daily variation
                    
                    currentAccuracy = (float)Math.Max(60, Math.Min(95, baseAccuracy + improvement + dailyVariation));
                    
                    data.PerformanceTrend.Add(new ChartDataPoint
                    {
                        Date = date,
                        Value = currentAccuracy,
                        Label = date.ToString("MM/dd"),
                        Category = skills[skillIndex]
                    });
                }
            }

            return data;
        }

        /// <summary>
        /// Generate skill breakdown data for pie chart
        /// </summary>
        public static List<SkillBreakdownData> GenerateSkillBreakdownData()
        {
            return new List<SkillBreakdownData>
            {
                new SkillBreakdownData 
                { 
                    SkillName = "Vocabulary", 
                    AccuracyRate = 88.2, 
                    TotalAttempts = 1250, 
                    ImprovementRate = 15.4, 
                    Level = "N3",
                    Color = OxyColor.FromRgb(156, 39, 176)
                },
                new SkillBreakdownData 
                { 
                    SkillName = "Kanji", 
                    AccuracyRate = 85.7, 
                    TotalAttempts = 680, 
                    ImprovementRate = 18.2, 
                    Level = "N3",
                    Color = OxyColor.FromRgb(63, 81, 181)
                },
                new SkillBreakdownData 
                { 
                    SkillName = "Grammar", 
                    AccuracyRate = 72.1, 
                    TotalAttempts = 420, 
                    ImprovementRate = 8.5, 
                    Level = "N4",
                    Color = OxyColor.FromRgb(96, 125, 139)
                },
                new SkillBreakdownData 
                { 
                    SkillName = "Reading", 
                    AccuracyRate = 79.4, 
                    TotalAttempts = 320, 
                    ImprovementRate = 12.1, 
                    Level = "N4",
                    Color = OxyColor.FromRgb(255, 152, 0)
                },
                new SkillBreakdownData 
                { 
                    SkillName = "Listening", 
                    AccuracyRate = 68.9, 
                    TotalAttempts = 180, 
                    ImprovementRate = 6.2, 
                    Level = "N4",
                    Color = OxyColor.FromRgb(233, 30, 99)
                }
            };
        }

        /// <summary>
        /// Generate realistic study pattern data
        /// </summary>
        public static List<StudyPatternData> GenerateStudyPatternData()
        {
            var data = new List<StudyPatternData>();
            var random = new Random();

            // Define typical study hours with realistic patterns
            var studyHourPatterns = new Dictionary<int, int>
            {
                { 6, 5 },   // Early morning - light study
                { 7, 15 },  // Morning routine
                { 8, 25 },  // Peak morning
                { 9, 10 },  // Work/school starts
                { 10, 5 },
                { 11, 8 },
                { 12, 30 }, // Lunch break - peak
                { 13, 20 }, // After lunch
                { 14, 5 },
                { 15, 8 },
                { 16, 12 },
                { 17, 15 }, // After work/school
                { 18, 25 }, // Early evening
                { 19, 35 }, // Evening peak
                { 20, 45 }, // Prime study time
                { 21, 35 }, // Night study
                { 22, 20 }, // Late evening
                { 23, 8 },  // Late night
            };

            for (int hour = 0; hour < 24; hour++)
            {
                var baseMinutes = studyHourPatterns.ContainsKey(hour) ? studyHourPatterns[hour] : 2;
                var variation = random.Next(-5, 10);
                var minutes = Math.Max(0, baseMinutes + variation);

                data.Add(new StudyPatternData
                {
                    Hour = hour,
                    DayOfWeek = 0, // Can be extended for day-of-week patterns
                    StudyMinutes = minutes,
                    Sessions = minutes > 15 ? random.Next(1, 3) : (minutes > 0 ? 1 : 0)
                });
            }

            return data;
        }

        /// <summary>
        /// Test all chart types with demo data
        /// </summary>
        public static Dictionary<string, PlotModel> GenerateAllDemoCharts(int days = 30)
        {
            var charts = new Dictionary<string, PlotModel>();
            var timeSeriesData = GenerateDemoData(days);
            var skillData = GenerateSkillBreakdownData();
            var patternData = GenerateStudyPatternData();

            charts["StudyTimeTrend"] = AnalyticsChartFactory.CreateStudyTimeTrendChart(
                timeSeriesData.DailyStudyTime, "Daily Study Time Trend");

            charts["PerformanceComparison"] = AnalyticsChartFactory.CreatePerformanceComparisonChart(
                timeSeriesData.PerformanceTrend, "Performance Trends by Skill");

            charts["SkillBreakdown"] = AnalyticsChartFactory.CreateSkillBreakdownPieChart(
                skillData, "Skill Accuracy Breakdown");

            charts["StudyPattern"] = AnalyticsChartFactory.CreateStudyPatternColumnChart(
                patternData, "Daily Study Pattern");

            charts["ProgressComparison"] = AnalyticsChartFactory.CreateProgressComparisonAreaChart(
                timeSeriesData, "Learning Progress Comparison");

            return charts;
        }
    }
}