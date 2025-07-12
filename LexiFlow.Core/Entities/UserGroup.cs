using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LexiFlow.Core.Entities
{
    public class UserGroup
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }

        // Navigation properties
        public virtual User? User { get; set; }
        public virtual Group? Group { get; set; }
    }
}
