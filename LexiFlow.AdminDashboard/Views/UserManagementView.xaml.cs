using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LexiFlow.AdminDashboard.ViewModels;

namespace LexiFlow.AdminDashboard.Views
{
    /// <summary>
    /// Interaction logic for UserManagementView.xaml
    /// </summary>
    public partial class UserManagementView : Window
    {
        private readonly UserManagementViewModel _viewModel;
        public UserManagementView()
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;

            this.Loaded += UserManagementView_Loaded;
        }
        private async void UserManagementView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await _viewModel.InitializeAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing User Management: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox && DataContext is UserManagementViewModel viewModel)
            {
                viewModel.NewPassword = passwordBox.Password;
            }
        }
    }

    // Converter for active status to color
    public class ActiveStatusToBrushConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool isActive)
            {
                return isActive
                    ? new SolidColorBrush(Color.FromRgb(22, 163, 74)) // Green for active
                    : new SolidColorBrush(Color.FromRgb(220, 38, 38)); // Red for inactive
            }

            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Converter for boolean to string
    public class BooleanToStringConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool boolValue && parameter is string options)
            {
                string[] parts = options.Split(':');
                return boolValue ? parts[0] : (parts.Length > 1 ? parts[1] : string.Empty);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Converter for boolean to value
    public class BooleanToValueConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool boolValue && parameter is string options)
            {
                string[] parts = options.Split(':');
                return boolValue ? parts[0] : (parts.Length > 1 ? parts[1] : string.Empty);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
