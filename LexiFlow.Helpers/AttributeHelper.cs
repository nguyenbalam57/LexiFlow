using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Helpers
{
    /// <summary>
    /// Attribute tùy chỉnh để lưu trữ một chuỗi (ví dụ: tên class icon, URL, hoặc key)
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class IconAttribute : Attribute
    {
        public string IconText { get; }

        /// <summary>
        /// Gán một chuỗi (tên class icon, v.v.) vào thành viên enum.
        /// </summary>
        /// <param name="iconText">Chuỗi đại diện cho icon (ví dụ: "fa-book", "icon-user")</param>
        public IconAttribute(string iconText)
        {
            IconText = iconText;
        }
    }
}
