using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.AdminDashboard.Models
{
    public class MenuItemModel
    {
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string ToolTip { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
        public string ViewName { get; set; } = string.Empty;
        public bool IsExpanded { get; set; }
        public List<MenuItemModel> SubItems { get; set; } = new List<MenuItemModel>();
        public bool HasSubItems => SubItems.Count > 0;
        public bool RequiresAdminPermission { get; set; }
    }
}
