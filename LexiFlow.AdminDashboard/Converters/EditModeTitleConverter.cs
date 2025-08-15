using System;
using System.Globalization;
using System.Windows.Data;

namespace LexiFlow.AdminDashboard.Converters
{
    /// <summary>
    /// Converter for handling edit mode titles and text
    /// </summary>
    public class EditModeTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isEditMode && parameter is string parameterString)
            {
                var parts = parameterString.Split(';');
                if (parts.Length == 2)
                {
                    return isEditMode ? parts[0] : parts[1];
                }
            }

            return isEditMode ? "Edit" : "Create";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}