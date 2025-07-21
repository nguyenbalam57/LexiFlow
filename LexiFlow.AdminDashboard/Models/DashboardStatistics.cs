using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.AdminDashboard.Models
{
    public class DashboardStatistics
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalVocabularyItems { get; set; }
        public int TotalKanji { get; set; }
        public int TotalGrammarPoints { get; set; }
        public int UserSubmissions { get; set; }
        public int PendingApprovals { get; set; }
        public int CompletedTests { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.Now;

        public List<UserActivity> RecentUserActivities { get; set; } = new List<UserActivity>();
        public List<MonthlyStatistic> MonthlyUserStats { get; set; } = new List<MonthlyStatistic>();
        public List<MonthlyStatistic> MonthlyContentStats { get; set; } = new List<MonthlyStatistic>();
        public Dictionary<string, int> UsersByLevel { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> ContentByCategory { get; set; } = new Dictionary<string, int>();
    }

    public class UserActivity
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string ActivityType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }

    public class MonthlyStatistic
    {
        public DateTime Month { get; set; }
        public int Value { get; set; }
        public string Category { get; set; } = string.Empty;
    }

    public class UserManagementStats
    {
        public int TotalUsers { get; set; }
        public int AdminUsers { get; set; }
        public int TeacherUsers { get; set; }
        public int StudentUsers { get; set; }
        public int NewUsersThisMonth { get; set; }
        public int ActiveUsersToday { get; set; }
        public int InactiveUsers { get; set; }
        public List<UserRoleDistribution> RoleDistribution { get; set; } = new List<UserRoleDistribution>();
        public List<UserRegistrationStats> RegistrationTrend { get; set; } = new List<UserRegistrationStats>();
    }

    public class UserRoleDistribution
    {
        public string Role { get; set; } = string.Empty;
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public class UserRegistrationStats
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}
