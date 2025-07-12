using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Core.Entities
{
    public class RolePermission
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        // Navigation properties
        public virtual Role? Role { get; set; }
        public virtual Permission? Permission { get; set; }
    }
}
