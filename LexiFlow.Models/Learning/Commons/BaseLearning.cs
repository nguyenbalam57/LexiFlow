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
        

        // Cải tiến: Trạng thái
        public bool IsVerified { get; set; } = false; // Đã được xác minh

        public int? VerifiedBy { get; set; } // Người xác minh

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
        public string Status { get; set; } = "Review";

        [ForeignKey("VerifiedBy")]
        public virtual User VerifiedByUser { get; set; }

        /// <summary>
        /// Xác thực Kanji
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
        public virtual void Unverify()
        {
            IsVerified = false;
            VerifiedAt = null;
            VerifiedBy = null;
            Status = "Pending";
            UpdateTimestamp();
        }

        /// <summary>
        /// Soft delete Kanji
        /// </summary>
        /// <param name="deletedBy">ID người xóa</param>
        /// <param name="reason">Lý do xóa</param>
        public virtual void SoftDelete(int deletedBy, string reason = null)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = deletedBy;
            Status = "Deleted";

            UpdateModification(deletedBy, $"Xóa Kanji: {reason}");
        }

        /// <summary>
        /// Khôi phục Kanji đã xóa
        /// </summary>
        /// <param name="restoredBy">ID người khôi phục</param>
        public virtual void Restore(int restoredBy)
        {
            IsDeleted = false;
            DeletedAt = null;
            DeletedBy = null;
            Status = "Active";
            UpdateModification(restoredBy, "Khôi phục Kanji");
        }
    }
}
