using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace LexiFlow.AdminDashboard.Converters
{
    /// <summary>
    /// Converter to convert boolean values to colors for connection status
    /// </summary>
    public class BoolToConnectedColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isConnected)
            {
                return new SolidColorBrush(isConnected ? Colors.Green : Colors.Red);
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converter to convert count to visibility (visible if count > 0)
    /// </summary>
    public class CountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count)
            {
                return count > 0 ? Visibility.Collapsed : Visibility.Visible; // Show empty state when count is 0
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converter to convert string to boolean (true if not null or empty)
    /// </summary>
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !string.IsNullOrEmpty(value?.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converter to invert boolean values
    /// </summary>
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false;
        }
    }

    /// <summary>
    /// Converter to convert percentage to brush color
    /// </summary>
    public class PercentageToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double percentage)
            {
                if (percentage >= 80)
                    return new SolidColorBrush(Colors.Green);
                else if (percentage >= 60)
                    return new SolidColorBrush(Colors.Orange);
                else
                    return new SolidColorBrush(Colors.Red);
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converter to format time span as readable string
    /// </summary>
    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan timeSpan)
            {
                if (timeSpan.TotalDays >= 1)
                    return $"{(int)timeSpan.TotalDays} days, {timeSpan.Hours}h {timeSpan.Minutes}m";
                else if (timeSpan.TotalHours >= 1)
                    return $"{timeSpan.Hours}h {timeSpan.Minutes}m";
                else
                    return $"{timeSpan.Minutes}m {timeSpan.Seconds}s";
            }
            return "0m";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converter to format numbers with appropriate suffixes (K, M, B)
    /// </summary>
    public class NumberToFormattedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double number)
            {
                if (number >= 1000000000)
                    return $"{number / 1000000000:F1}B";
                else if (number >= 1000000)
                    return $"{number / 1000000:F1}M";
                else if (number >= 1000)
                    return $"{number / 1000:F1}K";
                else
                    return number.ToString("F0");
            }
            else if (value is int intNumber)
            {
                return Convert(intNumber, targetType, parameter, culture);
            }
            return value?.ToString() ?? "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Multi-value converter to format multiple values into a single string
    /// </summary>
    public class MultiValueStringFormatConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string format && values != null)
            {
                try
                {
                    return string.Format(format, values);
                }
                catch
                {
                    return string.Join(" ", values);
                }
            }
            return string.Join(" ", values ?? new object[0]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converter for analytics severity levels to colors
    /// </summary>
    public class SeverityToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value?.ToString()?.ToLower() is string severity)
            {
                return severity switch
                {
                    "critical" => new SolidColorBrush(Colors.DarkRed),
                    "high" => new SolidColorBrush(Colors.Red),
                    "medium" => new SolidColorBrush(Colors.Orange),
                    "low" => new SolidColorBrush(Colors.Yellow),
                    _ => new SolidColorBrush(Colors.Gray)
                };
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converter to convert analytics status to appropriate icon
    /// </summary>
    public class StatusToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value?.ToString()?.ToLower() is string status)
            {
                return status switch
                {
                    "success" or "completed" => "CheckCircle",
                    "warning" or "caution" => "Alert",
                    "error" or "failed" => "CloseCircle",
                    "info" or "information" => "Information",
                    "loading" or "processing" => "Loading",
                    "connected" => "Wifi",
                    "disconnected" => "WifiOff",
                    _ => "Circle"
                };
            }
            return "Circle";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}