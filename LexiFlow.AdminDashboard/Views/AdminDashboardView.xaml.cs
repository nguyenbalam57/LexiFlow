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
using LexiFlow.AdminDashboard.Models;
using LexiFlow.AdminDashboard.ViewModels;

namespace LexiFlow.AdminDashboard.Views
{
    /// <summary>
    /// Interaction logic for AdminDashboardView.xaml
    /// </summary>
    public partial class AdminDashboardView : Window
    {
        private readonly AdminDashboardViewModel _viewModel;
        public AdminDashboardView()
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;

            this.Loaded += AdminDashboardView_Loaded;
            this.Closing += AdminDashboardView_Closing;
        }
        private async void AdminDashboardView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Set current user
                _viewModel.CurrentUser = App.Current.Properties["CurrentUser"]?.ToString() ?? "Admin User";

                // Load dashboard data
                await _viewModel.LoadDashboardDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AdminDashboardView_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            // Save any settings if needed
        }

        private void NavigationTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is MenuItemModel menuItem)
            {
                // Navigate to selected view
                switch (menuItem.ViewName)
                {
                    case "UserManagement":
                        OpenUserManagementView();
                        break;
                    case "VocabularyManagement":
                        OpenVocabularyManagementView();
                        break;
                    case "SystemConfig":
                        OpenSystemConfigView();
                        break;
                }
            }
        }

        private void OpenUserManagementView()
        {
            try
            {
                var userManagementView = App.Current.Services.GetService(typeof(UserManagementView)) as UserManagementView;
                if (userManagementView != null)
                {
                    userManagementView.Owner = this;
                    userManagementView.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Could not create User Management view.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening User Management: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenVocabularyManagementView()
        {
            // To be implemented
            MessageBox.Show("Vocabulary Management is not implemented yet.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OpenSystemConfigView()
        {
            try
            {
                var systemConfigView = App.Current.Services.GetService(typeof(SystemConfigView)) as SystemConfigView;
                if (systemConfigView != null)
                {
                    systemConfigView.Owner = this;
                    systemConfigView.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Could not create System Config view.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening System Config: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Helper method to support App.Current.Services in design-time
        private static object? GetService(Type serviceType)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return null;

            return App.Current.Services.GetService(serviceType);
        }
    }

    // Converter to get user initials from username
    public class UserInitialsConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string username && !string.IsNullOrEmpty(username))
            {
                var parts = username.Split(new[] { ' ', '.', '_', '-' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 0)
                {
                    if (parts.Length == 1)
                    {
                        // Take first two characters of single name
                        return username.Length > 1 ? username.Substring(0, 2).ToUpper() : username.ToUpper();
                    }
                    else
                    {
                        // Take first character of first and last name
                        return $"{parts[0][0]}{parts[parts.Length - 1][0]}".ToUpper();
                    }
                }
            }

            return "?";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
