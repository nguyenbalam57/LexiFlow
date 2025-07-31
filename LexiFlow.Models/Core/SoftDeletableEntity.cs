using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Core
{
    /// <summary>
    /// Lớp cơ sở với hỗ trợ xóa mềm
    /// </summary>
    public abstract class SoftDeletableEntity : BaseEntity, ISoftDeletable
    {
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }

        [ForeignKey("DeletedBy")]
        public virtual User.User DeletedByUser { get; set; }
    }
}
