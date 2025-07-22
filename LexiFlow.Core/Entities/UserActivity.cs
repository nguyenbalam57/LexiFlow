using System;
using System.ComponentModel.DataAnnotations;

namespace LexiFlow.Core.Entities
{
    /// <summary>
    /// Represents a user activity in the system
    /// </summary>
    public class UserActivity : BaseEntity
    {
        /// <summary>
        /// ID of the user who performed the activity
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Username of the user who performed the activity
        /// </summary>
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Module or area of the application where the activity occurred
        /// </summary>
        [StringLength(50)]
        public string Module { get; set; } = string.Empty;

        /// <summary>
        /// Action performed by the user
        /// </summary>
        [StringLength(50)]
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp when the activity occurred
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Details about the activity
        /// </summary>
        public string Details { get; set; } = string.Empty;

        /// <summary>
        /// IP address of the user
        /// </summary>
        [StringLength(50)]
        public string IpAddress { get; set; } = string.Empty;

        /// <summary>
        /// User agent (browser/client) information
        /// </summary>
        [StringLength(255)]
        public string UserAgent { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property for the user
        /// </summary>
        public virtual User User { get; set; }
    }
}