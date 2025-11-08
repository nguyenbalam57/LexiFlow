using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class EnumHelper
    {
        /// <summary>
        /// Lấy giá trị Tên hiển thị từ [Display(Name = "...")]
        /// </summary>
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetAttribute<DisplayAttribute>()?.Name ?? enumValue.ToString();
        }

        /// <summary>
        /// Lấy giá trị Tên ngắn gọn từ [Display(ShortName = "...")]
        /// Nếu không có, sẽ lấy Tên hiển thị (Name)
        /// </summary>
        public static string GetShortName(this Enum enumValue)
        {
            var displayAttr = enumValue.GetAttribute<DisplayAttribute>();
            return displayAttr?.ShortName ?? displayAttr?.Name ?? enumValue.ToString();
        }

        /// <summary>
        /// Lấy giá trị Mô tả từ [Description("...")]
        /// </summary>
        public static string GetDescription(this Enum enumValue)
        {
            return enumValue.GetAttribute<DescriptionAttribute>()?.Description;
        }

        /// <summary>
        /// Lấy giá trị Icon từ [Icon("...")]
        /// </summary>
        public static string GetIcon(this Enum enumValue)
        {
            return enumValue.GetAttribute<IconAttribute>()?.IconText;
        }

        // --- Phương thức helper private ---

        /// <summary>
        /// Phương thức private chung để lấy một Attribute bất kỳ từ thành viên enum
        /// </summary>
        private static T GetAttribute<T>(this Enum enumValue) where T : Attribute
        {
            if (enumValue == null) return null;

            FieldInfo fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            if (fieldInfo == null) return null;

            return fieldInfo.GetCustomAttribute<T>(false);
        }
    }
}
