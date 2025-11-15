using LexiFlow.Models.Cores;
using LexiFlow.Models.Enums;
using LexiFlow.Models.Users;
using LexiFlow.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LexiFlow.Models.Learning.Commons
{
    public abstract class BaseLearning : AuditableEntity
    {
        /// <summary>
        /// Trạng thái đã được xác minh
        /// </summary>
        public bool IsVerified { get; set; } = false; // Đã được xác minh

        /// <summary>
        /// Người xác minh
        /// </summary>
        public int? VerifiedBy { get; set; } // Người xác minh

        /// <summary>
        /// Thời gian xác minh
        /// </summary>
        public DateTime? VerifiedAt { get; set; } // Thời gian phê duyệt

        /// <summary>
        /// Trạng thái hiện tại
        /// </summary>
        /// <value>
        /// Các trạng thái hợp lệ:
        /// - "Active": Đang hoạt động (mặc định)
        /// - "Draft": Bản nháp, chưa publish
        /// - "Review": Đang chờ duyệt
        /// - "Deprecated": Không còn sử dụng
        /// </value>
        public ContentStatus Status { get; set; } = ContentStatus.Review; // Active, Draft, Review, Deprecated

        /// <summary>
        /// Đánh dấu đã xác minh
        /// </summary>
        [ForeignKey(nameof(VerifiedBy))]
        public virtual User VerifiedByUser { get; set; }

        /// <summary>
        /// Xác thực 
        /// </summary>
        /// <param name="verifiedBy">ID người xác thực</param>
        public virtual void Verify(int verifiedBy)
        {
            IsVerified = true;
            VerifiedAt = DateTime.UtcNow;
            VerifiedBy = verifiedBy;
            Status = ContentStatus.Active;
            UpdateTimestamp();
        }

        /// <summary>
        /// Hủy xác thực
        /// Có nhiều lý do để hủy xác thực, ví dụ như phát hiện lỗi, nội dung không chính xác, v.v.
        /// Chỉ giới hạn những nội dung có trạng thái
        /// </summary>
        public virtual void Unverify(int unverifiedBy, ContentStatus status, string reason = null)
        {
            IsVerified = false;
            VerifiedAt = DateTime.UtcNow;
            VerifiedBy = unverifiedBy;
            Status = status;

            var reasons = new List<string>();

            reasons.Add(status.GetDescription());
            if (!string.IsNullOrEmpty(reason))
            {
                reasons.Add($"Lý do: {reason}");
            }

            SoftUpdate(unverifiedBy, $"Hủy xác minh nội dung: {string.Join(", ", reasons)}");
        }

        /// <summary>
        /// Soft delete Kanji
        /// </summary>
        /// <param name="deletedBy">ID người xóa</param>
        /// <param name="reason">Lý do xóa</param>
        public virtual void SoftDelete(int deletedBy, string reason = null)
        {
            Status = ContentStatus.Deleted;
            base.SoftDelete(deletedBy, reason);
        }

        /// <summary>
        /// Khôi phục nội dung đã xóa
        /// </summary>
        /// <param name="restoredBy">ID người khôi phục</param>
        public virtual void Restore(int restoredBy, string reason = null)
        {
            Status = ContentStatus.Active;
            base.Restore(restoredBy, reason);
        }
    }
}
