using LexiFlow.Models.Cores;
using LexiFlow.Models.Users;
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
        public string Status { get; set; } = "Review"; // Active, Draft, Review, Deprecated

        /// <summary>
        /// Đánh dấu đã xác minh
        /// </summary>
        [ForeignKey("VerifiedBy")]
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
            Status = "Active";
            UpdateTimestamp();
        }

        /// <summary>
        /// Hủy xác thực
        /// </summary>
        public virtual void Unverify(int unverifiedBy)
        {
            IsVerified = false;
            VerifiedAt = DateTime.UtcNow;
            VerifiedBy = unverifiedBy;
            Status = "Pending";
            UpdateModification(unverifiedBy, "Hủy xác thực Kanji");
        }

        /// <summary>
        /// Soft delete Kanji
        /// </summary>
        /// <param name="deletedBy">ID người xóa</param>
        /// <param name="reason">Lý do xóa</param>
        public virtual void SoftDelete(int deletedBy, string reason = null)
        {
            Status = "Deprecated";
            base.SoftDelete(deletedBy, $"Xóa Kanji: {reason}");
        }

        /// <summary>
        /// Khôi phục Kanji đã xóa
        /// </summary>
        /// <param name="restoredBy">ID người khôi phục</param>
        public virtual void Restore(int restoredBy, string reason = null)
        {
            Status = "Active";
            base.Restore(restoredBy, $"Khôi phục Kanji: {reason}");
        }
    }
}
