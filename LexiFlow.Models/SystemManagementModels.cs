using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.Models
{
    #region System Management Models

    /// <summary>
    /// Represents a system setting
    /// </summary>
    public class Setting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SettingID { get; set; }

        [Required]
        [StringLength(100)]
        public string SettingKey { get; set; }

        public string SettingValue { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Group { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

    /// <summary>
    /// Represents an activity log entry
    /// </summary>
    public class ActivityLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogID { get; set; }

        public int? UserID { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;

        [StringLength(100)]
        public string Action { get; set; }

        [StringLength(50)]
        public string Module { get; set; }

        public string Details { get; set; }

        [StringLength(45)]
        public string IPAddress { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }

    /// <summary>
    /// Represents a sync log entry
    /// </summary>
    public class SyncLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SyncID { get; set; }

        [Required]
        public int UserID { get; set; }

        [StringLength(100)]
        public string TableName { get; set; }

        public DateTime? LastSyncAt { get; set; }

        public int? RecordsSynced { get; set; }

        [StringLength(20)]
        public string SyncDirection { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        public string ErrorMessage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }

    #endregion
}