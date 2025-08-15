using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using LexiFlow.API.Services;
using System.Security.Claims;

namespace LexiFlow.API.Hubs
{
    /// <summary>
    /// SignalR Hub for real-time analytics updates
    /// </summary>
    [Authorize]
    public class AnalyticsHub : Hub
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly ILogger<AnalyticsHub> _logger;

        public AnalyticsHub(IAnalyticsService analyticsService, ILogger<AnalyticsHub> logger)
        {
            _analyticsService = analyticsService;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = GetCurrentUserId();
            var connectionId = Context.ConnectionId;
            
            _logger.LogInformation("User {UserId} connected to Analytics Hub with connection {ConnectionId}", 
                userId, connectionId);

            // Add user to their personal group for targeted updates
            await Groups.AddToGroupAsync(connectionId, $"User_{userId}");
            
            // Send initial analytics data
            try
            {
                var dashboard = await _analyticsService.GetLearningDashboardAsync(userId, 7); // Last 7 days for real-time
                await Clients.Caller.SendAsync("InitialData", dashboard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending initial data to user {UserId}", userId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = GetCurrentUserId();
            var connectionId = Context.ConnectionId;
            
            _logger.LogInformation("User {UserId} disconnected from Analytics Hub (Connection: {ConnectionId})", 
                userId, connectionId);

            if (exception != null)
            {
                _logger.LogError(exception, "User {UserId} disconnected with error", userId);
            }

            // Remove from groups
            await Groups.RemoveFromGroupAsync(connectionId, $"User_{userId}");
            
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Client requests real-time dashboard update
        /// </summary>
        public async Task RequestDashboardUpdate(int days = 7)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogDebug("User {UserId} requested dashboard update for {Days} days", userId, days);

                var dashboard = await _analyticsService.GetLearningDashboardAsync(userId, days);
                await Clients.Caller.SendAsync("DashboardUpdate", dashboard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing dashboard update request");
                await Clients.Caller.SendAsync("Error", "Failed to update dashboard");
            }
        }

        /// <summary>
        /// Client requests real-time performance metrics
        /// </summary>
        public async Task RequestPerformanceUpdate()
        {
            try
            {
                var userId = GetCurrentUserId();
                var endDate = DateTime.UtcNow;
                var startDate = endDate.AddDays(-30);

                var trends = await _analyticsService.GetPerformanceTrendsAsync(userId, startDate, endDate);
                var metrics = await _analyticsService.GetAdvancedMetricsAsync(userId, startDate, endDate);

                await Clients.Caller.SendAsync("PerformanceUpdate", new
                {
                    trends = trends,
                    metrics = metrics,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing performance update request");
                await Clients.Caller.SendAsync("Error", "Failed to update performance metrics");
            }
        }

        /// <summary>
        /// Client subscribes to specific analytics channel
        /// </summary>
        public async Task SubscribeToChannel(string channelName)
        {
            try
            {
                var userId = GetCurrentUserId();
                var connectionId = Context.ConnectionId;

                // Validate channel name
                var allowedChannels = new[] { "study-sessions", "test-results", "goal-progress", "daily-stats" };
                if (!allowedChannels.Contains(channelName))
                {
                    await Clients.Caller.SendAsync("Error", $"Invalid channel: {channelName}");
                    return;
                }

                var groupName = $"User_{userId}_{channelName}";
                await Groups.AddToGroupAsync(connectionId, groupName);
                
                _logger.LogDebug("User {UserId} subscribed to channel {Channel}", userId, channelName);
                await Clients.Caller.SendAsync("ChannelSubscribed", channelName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error subscribing to channel {Channel}", channelName);
                await Clients.Caller.SendAsync("Error", "Failed to subscribe to channel");
            }
        }

        /// <summary>
        /// Client unsubscribes from specific analytics channel
        /// </summary>
        public async Task UnsubscribeFromChannel(string channelName)
        {
            try
            {
                var userId = GetCurrentUserId();
                var connectionId = Context.ConnectionId;

                var groupName = $"User_{userId}_{channelName}";
                await Groups.RemoveFromGroupAsync(connectionId, groupName);
                
                _logger.LogDebug("User {UserId} unsubscribed from channel {Channel}", userId, channelName);
                await Clients.Caller.SendAsync("ChannelUnsubscribed", channelName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unsubscribing from channel {Channel}", channelName);
            }
        }

        /// <summary>
        /// Client reports completion of study activity
        /// </summary>
        public async Task ReportStudyActivity(object activityData)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogDebug("User {UserId} reported study activity", userId);

                // Invalidate cache to ensure fresh data
                await _analyticsService.InvalidateCacheAsync(userId);

                // Notify other components about the activity
                await Clients.Group($"User_{userId}").SendAsync("StudyActivityReported", new
                {
                    userId = userId,
                    activity = activityData,
                    timestamp = DateTime.UtcNow
                });

                // Send updated metrics to study-sessions channel subscribers
                var groupName = $"User_{userId}_study-sessions";
                await Clients.Group(groupName).SendAsync("StudySessionUpdate", activityData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing study activity report");
                await Clients.Caller.SendAsync("Error", "Failed to process study activity");
            }
        }

        /// <summary>
        /// Client reports completion of test
        /// </summary>
        public async Task ReportTestCompletion(object testData)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogDebug("User {UserId} reported test completion", userId);

                // Invalidate cache
                await _analyticsService.InvalidateCacheAsync(userId);

                // Send updated performance metrics
                var groupName = $"User_{userId}_test-results";
                await Clients.Group(groupName).SendAsync("TestResultUpdate", testData);

                // Update overall dashboard
                await RequestDashboardUpdate();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing test completion report");
                await Clients.Caller.SendAsync("Error", "Failed to process test completion");
            }
        }

        /// <summary>
        /// Client requests goal progress update
        /// </summary>
        public async Task RequestGoalUpdate(int goalId)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                // TODO: Implement goal-specific update logic
                var groupName = $"User_{userId}_goal-progress";
                await Clients.Group(groupName).SendAsync("GoalProgressUpdate", new
                {
                    goalId = goalId,
                    userId = userId,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing goal update request for goal {GoalId}", goalId);
                await Clients.Caller.SendAsync("Error", "Failed to update goal progress");
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 1;
        }
    }

    /// <summary>
    /// Service for sending analytics updates to connected clients
    /// </summary>
    public interface IAnalyticsHubService
    {
        Task NotifyStudySessionCompleted(int userId, object sessionData);
        Task NotifyTestCompleted(int userId, object testData);
        Task NotifyGoalProgressUpdated(int userId, int goalId, float progress);
        Task NotifyDailyStatsUpdated(int userId, object statsData);
        Task BroadcastSystemMetrics(object metricsData);
    }

    public class AnalyticsHubService : IAnalyticsHubService
    {
        private readonly IHubContext<AnalyticsHub> _hubContext;
        private readonly ILogger<AnalyticsHubService> _logger;

        public AnalyticsHubService(IHubContext<AnalyticsHub> hubContext, ILogger<AnalyticsHubService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task NotifyStudySessionCompleted(int userId, object sessionData)
        {
            try
            {
                var groupName = $"User_{userId}_study-sessions";
                await _hubContext.Clients.Group(groupName).SendAsync("StudySessionCompleted", sessionData);
                _logger.LogDebug("Notified study session completion for user {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error notifying study session completion for user {UserId}", userId);
            }
        }

        public async Task NotifyTestCompleted(int userId, object testData)
        {
            try
            {
                var groupName = $"User_{userId}_test-results";
                await _hubContext.Clients.Group(groupName).SendAsync("TestCompleted", testData);
                _logger.LogDebug("Notified test completion for user {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error notifying test completion for user {UserId}", userId);
            }
        }

        public async Task NotifyGoalProgressUpdated(int userId, int goalId, float progress)
        {
            try
            {
                var groupName = $"User_{userId}_goal-progress";
                await _hubContext.Clients.Group(groupName).SendAsync("GoalProgressUpdated", new
                {
                    goalId = goalId,
                    progress = progress,
                    timestamp = DateTime.UtcNow
                });
                _logger.LogDebug("Notified goal progress update for user {UserId}, goal {GoalId}", userId, goalId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error notifying goal progress for user {UserId}, goal {GoalId}", userId, goalId);
            }
        }

        public async Task NotifyDailyStatsUpdated(int userId, object statsData)
        {
            try
            {
                var groupName = $"User_{userId}_daily-stats";
                await _hubContext.Clients.Group(groupName).SendAsync("DailyStatsUpdated", statsData);
                _logger.LogDebug("Notified daily stats update for user {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error notifying daily stats for user {UserId}", userId);
            }
        }

        public async Task BroadcastSystemMetrics(object metricsData)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("SystemMetricsUpdate", metricsData);
                _logger.LogDebug("Broadcasted system metrics update");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error broadcasting system metrics");
            }
        }
    }
}