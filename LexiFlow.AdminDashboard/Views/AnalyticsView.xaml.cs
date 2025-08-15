using System;
using System.Windows;
using System.Windows.Controls;

namespace LexiFlow.AdminDashboard.Views
{
    /// <summary>
    /// Interaction logic for AnalyticsView.xaml
    /// </summary>
    public partial class AnalyticsView : UserControl
    {
        public AnalyticsView()
        {
            InitializeComponent();
        }

        public void RefreshData()
        {
            // Placeholder for refresh functionality
        }

        // Keep legacy click handlers for backward compatibility
        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Generate Report functionality will be implemented through ViewModel binding.", 
                "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportData_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Export Data functionality will be implemented through ViewModel binding.", 
                "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}