using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models
{
    #region Notification System Models

    /// <summary>
    /// Represents a notification type
    /// </summary>
    public class NotificationType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(255)]
        public string IconPath { get; set; }

        [StringLength(20)]
        public string ColorCode { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<Notification> Notifications { get; set; }
    }

    /// <summary>
    /// Represents a notification priority
    /// </summary>
    public class NotificationPriority
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PriorityID { get; set; }

        [Required]
        [StringLength(50)]
        public string PriorityName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int? DisplayOrder { get; set; }

        [StringLength(20)]
        public string ColorCode { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<Notification> Notifications { get; set; }
    }

    /// <summary>
    /// Represents a notification
    /// </summary>
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationID { get; set; }

        public int? SenderUserID { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string Content { get; set; }

        public int? TypeID { get; set; }

        public int? PriorityID { get; set; }

        public bool AllowResponses { get; set; } = false;

        public DateTime? ExpirationDate { get; set; }

        [StringLength(255)]
        public string AttachmentURL { get; set; }

        public bool IsSystemGenerated { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("SenderUserID")]
        public virtual User SenderUser { get; set; }

        [ForeignKey("TypeID")]
        public virtual NotificationType Type { get; set; }

        [ForeignKey("PriorityID")]
        public virtual NotificationPriority Priority { get; set; }

        public virtual ICollection<NotificationRecipient> NotificationRecipients { get; set; }
    }

    /// <summary>
    /// Represents a notification status
    /// </summary>
    public class NotificationStatuse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusID { get; set; }

        [Required]
        [StringLength(50)]
        public string StatusName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(20)]
        public string ColorCode { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<NotificationRecipient> NotificationRecipients { get; set; }
    }

    /// <summary>
    /// Represents a notification recipient
    /// </summary>
    public class NotificationRecipient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecipientID { get; set; }

        [Required]
        public int NotificationID { get; set; }

        public int? UserID { get; set; }

        public int? GroupID { get; set; }

        public int? StatusID { get; set; }

        public DateTime ReceivedAt { get; set; } = DateTime.Now;

        public DateTime? ReadAt { get; set; }

        public bool IsArchived { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("NotificationID")]
        public virtual Notification Notification { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("GroupID")]
        public virtual Group Group { get; set; }

        [ForeignKey("StatusID")]
        public virtual NotificationStatuse Status { get; set; }

        public virtual ICollection<NotificationResponse> NotificationResponses { get; set; }
    }

    /// <summary>
    /// Represents a notification response
    /// </summary>
    public class NotificationResponse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResponseID { get; set; }

        [Required]
        public int RecipientID { get; set; }

        public string ResponseContent { get; set; }

        public int? ResponseTypeID { get; set; }

        public DateTime ResponseTime { get; set; } = DateTime.Now;

        [StringLength(255)]
        public string AttachmentURL { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("RecipientID")]
        public virtual NotificationRecipient Recipient { get; set; }
    }

    #endregion
}
