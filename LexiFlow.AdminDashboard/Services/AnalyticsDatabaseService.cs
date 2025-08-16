using LexiFlow.AdminDashboard.Models.Charts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LexiFlow.AdminDashboard.Services
{
    /// <summary>
    /// Simplified RealTimeActivityData model
    /// </summary>
    public class RealTimeActivityData
    {
        public string ActivityType { get; set; }
        public DateTime Timestamp { get; set; }
        public int UserId { get; set; }
        public string Data { get; set; }
        public Dictionary<string, object> Properties { get; set; } = new();
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
    }

    /// <summary>
    /// Service for advanced analytics database operations and real-time data integration
    /// </summary>
    public interface IAnalyticsDatabaseService
    {
        Task<StudyTimeSeriesData> GetRealTimeStudyDataAsync(int userId, int days = 30);
        Task<List<PerformanceTrendData>> GetRealTimePerformanceDataAsync(int userId, int days = 30);
        Task<Dictionary<string, object>> GetRealTimeMetricsAsync(int userId);
        Task<List<RealTimeActivityData>> GetRealTimeActivitiesAsync(int userId, int count = 10);
        Task<bool> InvalidateUserCacheAsync(int userId);
        Task<AnalyticsSnapshot> CreateSnapshotAsync(int userId);
        Task<List<AnalyticsSnapshot>> GetSnapshotHistoryAsync(int userId, int days = 30);
        Task<bool> OptimizeAnalyticsDataAsync();
        Task<DatabaseHealthMetrics> GetDatabaseHealthMetricsAsync();
        Task<List<UserEngagementMetrics>> GetUserEngagementMetricsAsync(DateTime startDate, DateTime endDate);
        Task<SystemPerformanceMetrics> GetSystemPerformanceMetricsAsync();
    }

    public class AnalyticsDatabaseService : IAnalyticsDatabaseService
    {
        private readonly ILogger<AnalyticsDatabaseService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public AnalyticsDatabaseService(ILogger<AnalyticsDatabaseService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<StudyTimeSeriesData> GetRealTimeStudyDataAsync(int userId, int days = 30)
        {
            try
            {
                _logger.LogDebug("Fetching real-time study data for user {UserId} for {Days} days", userId, days);

                var data = new StudyTimeSeriesData();
                var startDate = DateTime.UtcNow.AddDays(-days);

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                // Get daily study time data
                var dailyStudyQuery = @"
                    WITH DailyStudy AS (
                        SELECT 
                            CAST(StartTime AS DATE) as StudyDate,
                            SUM(ISNULL(DurationMinutes, 0)) as TotalMinutes,
                            COUNT(*) as SessionCount
                        FROM LearningSessions 
                        WHERE UserId = @UserId 
                            AND StartTime >= @StartDate
                            AND StartTime <= @EndDate
                        GROUP BY CAST(StartTime AS DATE)
                    ),
                    DateRange AS (
                        SELECT CAST(DATEADD(day, number, @StartDate) AS DATE) as StudyDate
                        FROM master.dbo.spt_values 
                        WHERE type = 'P' 
                            AND number <= DATEDIFF(day, @StartDate, @EndDate)
                    )
                    SELECT 
                        dr.StudyDate,
                        ISNULL(ds.TotalMinutes, 0) as TotalMinutes,
                        ISNULL(ds.SessionCount, 0) as SessionCount
                    FROM DateRange dr
                    LEFT JOIN DailyStudy ds ON dr.StudyDate = ds.StudyDate
                    ORDER BY dr.StudyDate";

                using var dailyCommand = new SqlCommand(dailyStudyQuery, connection);
                dailyCommand.Parameters.AddWithValue("@UserId", userId);
                dailyCommand.Parameters.AddWithValue("@StartDate", startDate);
                dailyCommand.Parameters.AddWithValue("@EndDate", DateTime.UtcNow);

                using var dailyReader = await dailyCommand.ExecuteReaderAsync();
                while (await dailyReader.ReadAsync())
                {
                    data.DailyStudyTime.Add(new ChartDataPoint
                    {
                        Date = (DateTime)dailyReader["StudyDate"],
                        Value = Convert.ToDouble(dailyReader["TotalMinutes"]),
                        Label = ((DateTime)dailyReader["StudyDate"]).ToString("MM/dd"),
                        Category = "StudyTime",
                        Metadata = new Dictionary<string, object>
                        {
                            ["SessionCount"] = Convert.ToInt32(dailyReader["SessionCount"])
                        }
                    });
                }
                dailyReader.Close();

                // Get vocabulary progress data
                var vocabularyQuery = @"
                    SELECT 
                        CAST(lp.LastStudied AS DATE) as StudyDate,
                        COUNT(*) as NewWords
                    FROM LearningProgresses lp
                    WHERE lp.UserId = @UserId 
                        AND lp.FirstStudied >= @StartDate
                        AND lp.FirstStudied <= @EndDate
                        AND lp.MasteryLevel >= 1
                    GROUP BY CAST(lp.LastStudied AS DATE)
                    ORDER BY StudyDate";

                using var vocabCommand = new SqlCommand(vocabularyQuery, connection);
                vocabCommand.Parameters.AddWithValue("@UserId", userId);
                vocabCommand.Parameters.AddWithValue("@StartDate", startDate);
                vocabCommand.Parameters.AddWithValue("@EndDate", DateTime.UtcNow);

                var cumulativeVocab = 0;
                using var vocabReader = await vocabCommand.ExecuteReaderAsync();
                while (await vocabReader.ReadAsync())
                {
                    cumulativeVocab += Convert.ToInt32(vocabReader["NewWords"]);
                    data.VocabularyProgress.Add(new ChartDataPoint
                    {
                        Date = (DateTime)vocabReader["StudyDate"],
                        Value = cumulativeVocab,
                        Label = ((DateTime)vocabReader["StudyDate"]).ToString("MM/dd"),
                        Category = "Vocabulary"
                    });
                }
                vocabReader.Close();

                // Get Kanji progress data
                var kanjiQuery = @"
                    SELECT 
                        CAST(ukp.LastPracticed AS DATE) as StudyDate,
                        COUNT(*) as NewKanji
                    FROM UserKanjiProgresses ukp
                    WHERE ukp.UserId = @UserId 
                        AND ukp.FirstLearned >= @StartDate
                        AND ukp.FirstLearned <= @EndDate
                        AND ukp.RecognitionLevel >= 3
                    GROUP BY CAST(ukp.LastPracticed AS DATE)
                    ORDER BY StudyDate";

                using var kanjiCommand = new SqlCommand(kanjiQuery, connection);
                kanjiCommand.Parameters.AddWithValue("@UserId", userId);
                kanjiCommand.Parameters.AddWithValue("@StartDate", startDate);
                kanjiCommand.Parameters.AddWithValue("@EndDate", DateTime.UtcNow);

                var cumulativeKanji = 0;
                using var kanjiReader = await kanjiCommand.ExecuteReaderAsync();
                while (await kanjiReader.ReadAsync())
                {
                    cumulativeKanji += Convert.ToInt32(kanjiReader["NewKanji"]);
                    data.KanjiProgress.Add(new ChartDataPoint
                    {
                        Date = (DateTime)kanjiReader["StudyDate"],
                        Value = cumulativeKanji,
                        Label = ((DateTime)kanjiReader["StudyDate"]).ToString("MM/dd"),
                        Category = "Kanji"
                    });
                }
                kanjiReader.Close();

                // Get Grammar progress data
                var grammarQuery = @"
                    SELECT 
                        CAST(ugp.LastStudied AS DATE) as StudyDate,
                        COUNT(*) as NewGrammar
                    FROM UserGrammarProgresses ugp
                    WHERE ugp.UserId = @UserId 
                        AND ugp.FirstLearned >= @StartDate
                        AND ugp.FirstLearned <= @EndDate
                        AND ugp.UnderstandingLevel >= 3
                    GROUP BY CAST(ugp.LastStudied AS DATE)
                    ORDER BY StudyDate";

                using var grammarCommand = new SqlCommand(grammarQuery, connection);
                grammarCommand.Parameters.AddWithValue("@UserId", userId);
                grammarCommand.Parameters.AddWithValue("@StartDate", startDate);
                grammarCommand.Parameters.AddWithValue("@EndDate", DateTime.UtcNow);

                var cumulativeGrammar = 0;
                using var grammarReader = await grammarCommand.ExecuteReaderAsync();
                while (await grammarReader.ReadAsync())
                {
                    cumulativeGrammar += Convert.ToInt32(grammarReader["NewGrammar"]);
                    data.GrammarProgress.Add(new ChartDataPoint
                    {
                        Date = (DateTime)grammarReader["StudyDate"],
                        Value = cumulativeGrammar,
                        Label = ((DateTime)grammarReader["StudyDate"]).ToString("MM/dd"),
                        Category = "Grammar"
                    });
                }

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching real-time study data for user {UserId}", userId);
                throw;
            }
        }

        public async Task<List<PerformanceTrendData>> GetRealTimePerformanceDataAsync(int userId, int days = 30)
        {
            try
            {
                var data = new List<PerformanceTrendData>();
                var startDate = DateTime.UtcNow.AddDays(-days);

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var query = @"
                    WITH PerformanceData AS (
                        SELECT 
                            CAST(tr.TestDate AS DATE) as TestDate,
                            ISNULL(td.SkillType, 'General') as SkillType,
                            AVG(tr.Score) as AverageScore,
                            COUNT(*) as TestCount,
                            AVG(CASE WHEN td.IsCorrect = 1 THEN 100.0 ELSE 0.0 END) as AccuracyRate
                        FROM TestResults tr
                        LEFT JOIN TestDetails td ON tr.TestResultId = td.TestResultId
                        WHERE tr.UserId = @UserId 
                            AND tr.TestDate >= @StartDate
                            AND tr.TestDate <= @EndDate
                        GROUP BY CAST(tr.TestDate AS DATE), ISNULL(td.SkillType, 'General')
                    )
                    SELECT 
                        TestDate,
                        SkillType,
                        AverageScore,
                        AccuracyRate,
                        TestCount
                    FROM PerformanceData
                    ORDER BY TestDate, SkillType";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", DateTime.UtcNow);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    data.Add(new PerformanceTrendData
                    {
                        Date = (DateTime)reader["TestDate"],
                        SkillType = reader["SkillType"].ToString(),
                        AverageScore = Convert.ToDouble(reader["AverageScore"]),
                        AccuracyRate = Convert.ToDouble(reader["AccuracyRate"]),
                        TestCount = Convert.ToInt32(reader["TestCount"])
                    });
                }

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching real-time performance data for user {UserId}", userId);
                throw;
            }
        }

        public async Task<Dictionary<string, object>> GetRealTimeMetricsAsync(int userId)
        {
            try
            {
                var metrics = new Dictionary<string, object>();

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                // Get comprehensive metrics in a single query
                var query = @"
                    DECLARE @Today DATE = CAST(GETUTCDATE() AS DATE);
                    DECLARE @WeekAgo DATE = DATEADD(day, -7, @Today);
                    DECLARE @MonthAgo DATE = DATEADD(day, -30, @Today);

                    -- Study metrics
                    SELECT 
                        'TotalStudyTime' as MetricName,
                        SUM(ISNULL(DurationMinutes, 0)) as Value
                    FROM LearningSessions 
                    WHERE UserId = @UserId AND StartTime >= @MonthAgo
                    
                    UNION ALL
                    
                    SELECT 
                        'StudyDaysThisMonth' as MetricName,
                        COUNT(DISTINCT CAST(StartTime AS DATE)) as Value
                    FROM LearningSessions 
                    WHERE UserId = @UserId AND StartTime >= @MonthAgo
                    
                    UNION ALL
                    
                    SELECT 
                        'StudyTimeToday' as MetricName,
                        SUM(ISNULL(DurationMinutes, 0)) as Value
                    FROM LearningSessions 
                    WHERE UserId = @UserId AND CAST(StartTime AS DATE) = @Today
                    
                    UNION ALL
                    
                    SELECT 
                        'StudyTimeThisWeek' as MetricName,
                        SUM(ISNULL(DurationMinutes, 0)) as Value
                    FROM LearningSessions 
                    WHERE UserId = @UserId AND StartTime >= @WeekAgo
                    
                    UNION ALL
                    
                    -- Vocabulary metrics
                    SELECT 
                        'VocabularyLearned' as MetricName,
                        COUNT(*) as Value
                    FROM LearningProgresses 
                    WHERE UserId = @UserId AND MasteryLevel >= 4
                    
                    UNION ALL
                    
                    SELECT 
                        'VocabularyThisMonth' as MetricName,
                        COUNT(*) as Value
                    FROM LearningProgresses 
                    WHERE UserId = @UserId AND LastStudied >= @MonthAgo AND MasteryLevel >= 4
                    
                    UNION ALL
                    
                    -- Kanji metrics
                    SELECT 
                        'KanjiLearned' as MetricName,
                        COUNT(*) as Value
                    FROM UserKanjiProgresses 
                    WHERE UserId = @UserId AND RecognitionLevel >= 7
                    
                    UNION ALL
                    
                    SELECT 
                        'KanjiThisMonth' as MetricName,
                        COUNT(*) as Value
                    FROM UserKanjiProgresses 
                    WHERE UserId = @UserId AND LastPracticed >= @MonthAgo AND RecognitionLevel >= 7
                    
                    UNION ALL
                    
                    -- Grammar metrics
                    SELECT 
                        'GrammarLearned' as MetricName,
                        COUNT(*) as Value
                    FROM UserGrammarProgresses 
                    WHERE UserId = @UserId AND UnderstandingLevel >= 4
                    
                    UNION ALL
                    
                    -- Test metrics
                    SELECT 
                        'TestsCompleted' as MetricName,
                        COUNT(*) as Value
                    FROM TestResults 
                    WHERE UserId = @UserId AND TestDate >= @MonthAgo
                    
                    UNION ALL
                    
                    SELECT 
                        'AverageTestScore' as MetricName,
                        ROUND(AVG(Score), 1) as Value
                    FROM TestResults 
                    WHERE UserId = @UserId AND TestDate >= @MonthAgo
                    
                    UNION ALL
                    
                    -- Streak calculation
                    SELECT 
                        'CurrentStreak' as MetricName,
                        (
                            SELECT COUNT(*)
                            FROM (
                                SELECT DISTINCT CAST(StartTime AS DATE) as StudyDate
                                FROM LearningSessions 
                                WHERE UserId = @UserId 
                                    AND StartTime >= DATEADD(day, -60, GETUTCDATE())
                            ) t
                            WHERE StudyDate >= (
                                SELECT MAX(BreakDate) + 1
                                FROM (
                                    SELECT StudyDate as BreakDate
                                    FROM (
                                        SELECT DISTINCT CAST(StartTime AS DATE) as StudyDate
                                        FROM LearningSessions 
                                        WHERE UserId = @UserId 
                                            AND StartTime >= DATEADD(day, -60, GETUTCDATE())
                                    ) dates
                                    WHERE NOT EXISTS (
                                        SELECT 1 
                                        FROM (
                                            SELECT DISTINCT CAST(StartTime AS DATE) as StudyDate
                                            FROM LearningSessions 
                                            WHERE UserId = @UserId 
                                                AND StartTime >= DATEADD(day, -60, GETUTCDATE())
                                        ) nextDay
                                        WHERE nextDay.StudyDate = DATEADD(day, 1, dates.StudyDate)
                                    )
                                    UNION ALL
                                    SELECT DATEADD(day, -1, MIN(StudyDate))
                                    FROM (
                                        SELECT DISTINCT CAST(StartTime AS DATE) as StudyDate
                                        FROM LearningSessions 
                                        WHERE UserId = @UserId 
                                            AND StartTime >= DATEADD(day, -60, GETUTCDATE())
                                    ) allDates
                                ) breaks
                            )
                        ) as Value";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var metricName = reader["MetricName"].ToString();
                    var value = reader["Value"];
                    
                    if (value != DBNull.Value)
                    {
                        metrics[metricName] = value;
                    }
                }

                // Add calculated metrics
                if (metrics.ContainsKey("TotalStudyTime") && metrics.ContainsKey("StudyDaysThisMonth"))
                {
                    var totalStudyTime = Convert.ToDouble(metrics["TotalStudyTime"]);
                    var studyDays = Convert.ToInt32(metrics["StudyDaysThisMonth"]);
                    
                    metrics["AverageStudyTimePerDay"] = studyDays > 0 ? Math.Round(totalStudyTime / studyDays, 1) : 0;
                }

                // Add real-time timestamp
                metrics["LastUpdated"] = DateTime.UtcNow;
                metrics["DataFreshness"] = "Real-time";

                return metrics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching real-time metrics for user {UserId}", userId);
                throw;
            }
        }

        public async Task<List<RealTimeActivityData>> GetRealTimeActivitiesAsync(int userId, int count = 10)
        {
            try
            {
                var activities = new List<RealTimeActivityData>();

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var query = @"
                    WITH RecentActivities AS (
                        -- Learning Sessions
                        SELECT 
                            StartTime as ActivityTime,
                            'Study Session' as ActivityType,
                            CONCAT(SessionType, ' - ', DurationMinutes, ' minutes') as Description,
                            'Book' as Icon,
                            '#FF4CAF50' as Color,
                            1 as ActivityOrder
                        FROM LearningSessions 
                        WHERE UserId = @UserId 
                            AND StartTime >= DATEADD(day, -7, GETUTCDATE())
                        
                        UNION ALL
                        
                        -- Test Results
                        SELECT 
                            TestDate as ActivityTime,
                            'Test' as ActivityType,
                            CONCAT(TestType, ' Test - Score: ', ROUND(Score, 1), '%') as Description,
                            'TestTube' as Icon,
                            CASE 
                                WHEN Score >= 80 THEN '#FF4CAF50'
                                WHEN Score >= 60 THEN '#FFFF9800'
                                ELSE '#FFF44336'
                            END as Color,
                            2 as ActivityOrder
                        FROM TestResults 
                        WHERE UserId = @UserId 
                            AND TestDate >= DATEADD(day, -7, GETUTCDATE())
                        
                        UNION ALL
                        
                        -- Goal Completions
                        SELECT 
                            UpdatedAt as ActivityTime,
                            'Goal Progress' as ActivityType,
                            CONCAT('Goal progress updated: ', ProgressPercentage, '%') as Description,
                            'Target' as Icon,
                            '#FF9C27B0' as Color,
                            3 as ActivityOrder
                        FROM StudyGoals sg
                        INNER JOIN StudyPlans sp ON sg.PlanId = sp.PlanId
                        WHERE sp.UserId = @UserId 
                            AND sg.UpdatedAt >= DATEADD(day, -7, GETUTCDATE())
                            AND sg.IsActive = 1
                    )
                    SELECT TOP (@Count)
                        ActivityTime as Timestamp,
                        ActivityType,
                        Description,
                        Icon,
                        Color
                    FROM RecentActivities 
                    ORDER BY ActivityTime DESC";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Count", count);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    activities.Add(new RealTimeActivityData
                    {
                        Timestamp = (DateTime)reader["Timestamp"],
                        ActivityType = reader["ActivityType"].ToString(),
                        Description = reader["Description"].ToString(),
                        Icon = reader["Icon"].ToString(),
                        Color = reader["Color"].ToString()
                    });
                }

                return activities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching real-time activities for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> InvalidateUserCacheAsync(int userId)
        {
            try
            {
                _logger.LogDebug("Invalidating cache for user {UserId}", userId);

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                // Update cache invalidation timestamp
                var query = @"
                    MERGE UserCacheInvalidation AS target
                    USING (SELECT @UserId as UserId, GETUTCDATE() as InvalidatedAt) AS source
                    ON target.UserId = source.UserId
                    WHEN MATCHED THEN
                        UPDATE SET InvalidatedAt = source.InvalidatedAt
                    WHEN NOT MATCHED THEN
                        INSERT (UserId, InvalidatedAt) VALUES (source.UserId, source.InvalidatedAt);";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                await command.ExecuteNonQueryAsync();

                _logger.LogInformation("Cache invalidated for user {UserId}", userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error invalidating cache for user {UserId}", userId);
                return false;
            }
        }

        public async Task<AnalyticsSnapshot> CreateSnapshotAsync(int userId)
        {
            try
            {
                _logger.LogDebug("Creating analytics snapshot for user {UserId}", userId);

                var metrics = await GetRealTimeMetricsAsync(userId);
                var studyData = await GetRealTimeStudyDataAsync(userId, 7); // Last 7 days
                var performanceData = await GetRealTimePerformanceDataAsync(userId, 7);

                var snapshot = new AnalyticsSnapshot
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    Metrics = metrics,
                    StudyTimeData = studyData.DailyStudyTime,
                    PerformanceData = performanceData,
                    DataHash = CalculateDataHash(metrics, studyData, performanceData)
                };

                // Save snapshot to database
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var query = @"
                    INSERT INTO AnalyticsSnapshots 
                    (UserId, CreatedAt, MetricsJson, StudyDataJson, PerformanceDataJson, DataHash)
                    VALUES (@UserId, @CreatedAt, @MetricsJson, @StudyDataJson, @PerformanceDataJson, @DataHash)";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@CreatedAt", snapshot.CreatedAt);
                command.Parameters.AddWithValue("@MetricsJson", System.Text.Json.JsonSerializer.Serialize(snapshot.Metrics));
                command.Parameters.AddWithValue("@StudyDataJson", System.Text.Json.JsonSerializer.Serialize(snapshot.StudyTimeData));
                command.Parameters.AddWithValue("@PerformanceDataJson", System.Text.Json.JsonSerializer.Serialize(snapshot.PerformanceData));
                command.Parameters.AddWithValue("@DataHash", snapshot.DataHash);

                await command.ExecuteNonQueryAsync();

                _logger.LogInformation("Analytics snapshot created for user {UserId}", userId);
                return snapshot;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating analytics snapshot for user {UserId}", userId);
                throw;
            }
        }

        public async Task<List<AnalyticsSnapshot>> GetSnapshotHistoryAsync(int userId, int days = 30)
        {
            try
            {
                var snapshots = new List<AnalyticsSnapshot>();
                var startDate = DateTime.UtcNow.AddDays(-days);

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var query = @"
                    SELECT 
                        UserId, CreatedAt, MetricsJson, StudyDataJson, PerformanceDataJson, DataHash
                    FROM AnalyticsSnapshots 
                    WHERE UserId = @UserId 
                        AND CreatedAt >= @StartDate
                    ORDER BY CreatedAt DESC";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@StartDate", startDate);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var snapshot = new AnalyticsSnapshot
                    {
                        UserId = (int)reader["UserId"],
                        CreatedAt = (DateTime)reader["CreatedAt"],
                        DataHash = reader["DataHash"].ToString()
                    };

                    // Deserialize JSON data
                    if (reader["MetricsJson"] != DBNull.Value)
                    {
                        snapshot.Metrics = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(
                            reader["MetricsJson"].ToString());
                    }

                    snapshots.Add(snapshot);
                }

                return snapshots;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching snapshot history for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> OptimizeAnalyticsDataAsync()
        {
            try
            {
                _logger.LogInformation("Starting analytics data optimization");

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                // Archive old snapshots
                var archiveQuery = @"
                    DELETE FROM AnalyticsSnapshots 
                    WHERE CreatedAt < DATEADD(day, -90, GETUTCDATE())";

                using var archiveCommand = new SqlCommand(archiveQuery, connection);
                var archivedRows = await archiveCommand.ExecuteNonQueryAsync();

                // Update statistics
                var updateStatsQuery = @"
                    UPDATE STATISTICS LearningSessions;
                    UPDATE STATISTICS TestResults;
                    UPDATE STATISTICS LearningProgresses;
                    UPDATE STATISTICS UserKanjiProgresses;
                    UPDATE STATISTICS UserGrammarProgresses;";

                using var statsCommand = new SqlCommand(updateStatsQuery, connection);
                await statsCommand.ExecuteNonQueryAsync();

                _logger.LogInformation("Analytics data optimization completed. Archived {ArchivedRows} old snapshots", archivedRows);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error optimizing analytics data");
                return false;
            }
        }

        public async Task<DatabaseHealthMetrics> GetDatabaseHealthMetricsAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var query = @"
                    SELECT 
                        -- Table sizes
                        SUM(CASE WHEN t.name = 'LearningSessions' THEN p.rows ELSE 0 END) as LearningSessionsCount,
                        SUM(CASE WHEN t.name = 'TestResults' THEN p.rows ELSE 0 END) as TestResultsCount,
                        SUM(CASE WHEN t.name = 'LearningProgresses' THEN p.rows ELSE 0 END) as LearningProgressesCount,
                        SUM(CASE WHEN t.name = 'AnalyticsSnapshots' THEN p.rows ELSE 0 END) as SnapshotsCount,
                        
                        -- Database size info
                        (SELECT SUM(size * 8.0 / 1024) FROM sys.database_files WHERE type = 0) as DatabaseSizeMB,
                        (SELECT SUM(size * 8.0 / 1024) FROM sys.database_files WHERE type = 1) as LogSizeMB
                    FROM sys.tables t
                    INNER JOIN sys.partitions p ON t.object_id = p.object_id AND p.index_id IN (0, 1)
                    WHERE t.name IN ('LearningSessions', 'TestResults', 'LearningProgresses', 'AnalyticsSnapshots')";

                using var command = new SqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new DatabaseHealthMetrics
                    {
                        LearningSessionsCount = Convert.ToInt64(reader["LearningSessionsCount"]),
                        TestResultsCount = Convert.ToInt64(reader["TestResultsCount"]),
                        LearningProgressesCount = Convert.ToInt64(reader["LearningProgressesCount"]),
                        SnapshotsCount = Convert.ToInt64(reader["SnapshotsCount"]),
                        DatabaseSizeMB = Convert.ToDouble(reader["DatabaseSizeMB"]),
                        LogSizeMB = Convert.ToDouble(reader["LogSizeMB"]),
                        LastUpdated = DateTime.UtcNow
                    };
                }

                return new DatabaseHealthMetrics { LastUpdated = DateTime.UtcNow };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting database health metrics");
                throw;
            }
        }

        public async Task<List<UserEngagementMetrics>> GetUserEngagementMetricsAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var metrics = new List<UserEngagementMetrics>();

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var query = @"
                    WITH UserActivity AS (
                        SELECT 
                            u.UserId,
                            u.Username,
                            COUNT(DISTINCT CAST(ls.StartTime AS DATE)) as ActiveDays,
                            SUM(ISNULL(ls.DurationMinutes, 0)) as TotalStudyMinutes,
                            COUNT(ls.SessionId) as TotalSessions,
                            COUNT(tr.TestResultId) as TestsCompleted,
                            AVG(tr.Score) as AverageScore,
                            MAX(ls.StartTime) as LastActivity
                        FROM Users u
                        LEFT JOIN LearningSessions ls ON u.UserId = ls.UserId 
                            AND ls.StartTime >= @StartDate AND ls.StartTime <= @EndDate
                        LEFT JOIN TestResults tr ON u.UserId = tr.UserId 
                            AND tr.TestDate >= @StartDate AND tr.TestDate <= @EndDate
                        GROUP BY u.UserId, u.Username
                    )
                    SELECT 
                        UserId,
                        Username,
                        ActiveDays,
                        TotalStudyMinutes,
                        TotalSessions,
                        TestsCompleted,
                        ISNULL(AverageScore, 0) as AverageScore,
                        LastActivity,
                        CASE 
                            WHEN ActiveDays >= 20 THEN 'High'
                            WHEN ActiveDays >= 10 THEN 'Medium'
                            ELSE 'Low'
                        END as EngagementLevel
                    FROM UserActivity
                    WHERE TotalSessions > 0
                    ORDER BY TotalStudyMinutes DESC";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    metrics.Add(new UserEngagementMetrics
                    {
                        UserId = (int)reader["UserId"],
                        Username = reader["Username"].ToString(),
                        ActiveDays = Convert.ToInt32(reader["ActiveDays"]),
                        TotalStudyMinutes = Convert.ToInt32(reader["TotalStudyMinutes"]),
                        TotalSessions = Convert.ToInt32(reader["TotalSessions"]),
                        TestsCompleted = Convert.ToInt32(reader["TestsCompleted"]),
                        AverageScore = Convert.ToDouble(reader["AverageScore"]),
                        LastActivity = reader["LastActivity"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["LastActivity"],
                        EngagementLevel = reader["EngagementLevel"].ToString()
                    });
                }

                return metrics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user engagement metrics");
                throw;
            }
        }

        public async Task<SystemPerformanceMetrics> GetSystemPerformanceMetricsAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var query = @"
                    SELECT 
                        -- Active connections
                        (SELECT COUNT(*) FROM sys.dm_exec_sessions WHERE is_user_process = 1) as ActiveConnections,
                        
                        -- Average query execution time (in milliseconds)
                        (SELECT AVG(total_elapsed_time/execution_count/1000) 
                         FROM sys.dm_exec_query_stats 
                         WHERE creation_time >= DATEADD(hour, -1, GETUTCDATE())) as AvgQueryTimeMs,
                        
                        -- Recent activity
                        (SELECT COUNT(*) FROM LearningSessions WHERE StartTime >= DATEADD(hour, -1, GETUTCDATE())) as RecentSessions,
                        (SELECT COUNT(*) FROM TestResults WHERE TestDate >= DATEADD(hour, -1, GETUTCDATE())) as RecentTests,
                        
                        -- System info
                        GETUTCDATE() as CurrentTime";

                using var command = new SqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new SystemPerformanceMetrics
                    {
                        ActiveConnections = Convert.ToInt32(reader["ActiveConnections"]),
                        AverageQueryTimeMs = reader["AvgQueryTimeMs"] == DBNull.Value ? 0 : Convert.ToDouble(reader["AvgQueryTimeMs"]),
                        RecentSessionsLastHour = Convert.ToInt32(reader["RecentSessions"]),
                        RecentTestsLastHour = Convert.ToInt32(reader["RecentTests"]),
                        Timestamp = (DateTime)reader["CurrentTime"]
                    };
                }

                return new SystemPerformanceMetrics { Timestamp = DateTime.UtcNow };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting system performance metrics");
                throw;
            }
        }

        private string CalculateDataHash(Dictionary<string, object> metrics, StudyTimeSeriesData studyData, List<PerformanceTrendData> performanceData)
        {
            // Simple hash calculation for data integrity
            var combined = $"{metrics.Count}_{studyData.DailyStudyTime.Count}_{performanceData.Count}_{DateTime.UtcNow.Ticks}";
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(combined));
        }
    }

    // Data models for database service
    public class AnalyticsSnapshot
    {
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Dictionary<string, object> Metrics { get; set; } = new();
        public List<ChartDataPoint> StudyTimeData { get; set; } = new();
        public List<PerformanceTrendData> PerformanceData { get; set; } = new();
        public string DataHash { get; set; }
    }

    public class PerformanceTrendData
    {
        public DateTime Date { get; set; }
        public string SkillType { get; set; }
        public double AverageScore { get; set; }
        public double AccuracyRate { get; set; }
        public int TestCount { get; set; }
    }

    public class DatabaseHealthMetrics
    {
        public long LearningSessionsCount { get; set; }
        public long TestResultsCount { get; set; }
        public long LearningProgressesCount { get; set; }
        public long SnapshotsCount { get; set; }
        public double DatabaseSizeMB { get; set; }
        public double LogSizeMB { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class UserEngagementMetrics
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int ActiveDays { get; set; }
        public int TotalStudyMinutes { get; set; }
        public int TotalSessions { get; set; }
        public int TestsCompleted { get; set; }
        public double AverageScore { get; set; }
        public DateTime? LastActivity { get; set; }
        public string EngagementLevel { get; set; }
    }

    public class SystemPerformanceMetrics
    {
        public int ActiveConnections { get; set; }
        public double AverageQueryTimeMs { get; set; }
        public int RecentSessionsLastHour { get; set; }
        public int RecentTestsLastHour { get; set; }
        public DateTime Timestamp { get; set; }
    }
}