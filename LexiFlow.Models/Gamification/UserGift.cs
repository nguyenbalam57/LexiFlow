using LexiFlow.Models.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Gamification
{
    /// <summary>
    /// Quà tặng giữa người dùng
    /// </summary>
    [Index(nameof(SenderUserId), nameof(ReceiverUserId), Name = "IX_UserGift_Sender_Receiver")]
    public class UserGift : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GiftId { get; set; }

        [Required]
        public int SenderUserId { get; set; }

        [Required]
        public int ReceiverUserId { get; set; }

        [StringLength(50)]
        public string GiftType { get; set; }

        public int? GiftValue { get; set; }

        // Cải tiến: Chi tiết quà tặng
        [StringLength(255)]
        public string GiftName { get; set; }

        [StringLength(255)]
        public string GiftImageUrl { get; set; }

        public string GiftProperties { get; set; }

        // Cải tiến: Trạng thái quà tặng
        [StringLength(50)]
        public string Status { get; set; } = "Sent"; // Sent, Received, Rejected, Expired

        public string Message { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public DateTime? ReceivedAt { get; set; }

        public bool IsUsed { get; set; } = false;

        public DateTime? UsedAt { get; set; }

        public bool IsExpired { get; set; } = false;

        public DateTime? ExpirationDate { get; set; }

        // Cải tiến: Theo dõi nguồn gốc
        public int? EventId { get; set; }
        public int? ChallengeId { get; set; }
        [StringLength(50)]
        public string SourceType { get; set; }
        public int? SourceId { get; set; }

        // Navigation properties
        [ForeignKey("SenderUserId")]
        public virtual User.User SenderUser { get; set; }

        [ForeignKey("ReceiverUserId")]
        public virtual User.User ReceiverUser { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        [ForeignKey("ChallengeId")]
        public virtual Challenge Challenge { get; set; }
    }
}
