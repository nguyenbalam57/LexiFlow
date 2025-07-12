using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Core.Entities
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<User>? Users { get; set; }

        public virtual ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
