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
using Microsoft.WindowsAPICodePack.Dialogs;


namespace LexiFlow.AdminDashboard.Views
{
    /// <summary>
    /// Interaction logic for SystemConfigView.xaml
    /// </summary>
    public partial class SystemConfigView : Window
    {
        private readonly SystemConfigViewModel _viewModel;
        public SystemConfigView()
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;

            this.Loaded += SystemConfigView_Loaded;
        }
        private async void SystemConfigView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await _viewModel.InitializeAsync();

                // Set password box from settings
                if (!string.IsNullOrEmpty(_viewModel.Settings.Notifications.SmtpPassword))
                {
                    SmtpPasswordBox.Password = _viewModel.Settings.Notifications.SmtpPassword;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error initializing System Configuration: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SmtpPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.PasswordBox passwordBox && DataContext is SystemConfigViewModel viewModel)
            {
                viewModel.Settings.Notifications.SmtpPassword = passwordBox.Password;
            }
        }

        private void BackupPathBrowse_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                _viewModel.Settings.Backup.BackupPath = dialog.FileName;
            }
        }
    }
}
