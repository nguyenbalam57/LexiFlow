using LexiFlow.AdminDashboard.Views.Dialogs;
using LexiFlow.AdminDashboard.ViewModels;
using System.Windows;

namespace LexiFlow.AdminDashboard.Services
{
    /// <summary>
    /// Service for managing dialog operations
    /// </summary>
    public interface IDialogService
    {
        bool? ShowVocabularyEditDialog(VocabularyManagementViewModel viewModel);
        bool? ShowCategoryEditDialog(VocabularyManagementViewModel viewModel);
        bool ShowConfirmDialog(string message, string title = "Confirm");
        void ShowInfoDialog(string message, string title = "Information");
        void ShowErrorDialog(string message, string title = "Error");
        string ShowOpenFileDialog(string filter = "All files (*.*)|*.*", string defaultExt = "");
        string ShowSaveFileDialog(string filter = "All files (*.*)|*.*", string defaultExt = "", string defaultFileName = "");
    }

    /// <summary>
    /// Implementation of dialog service
    /// </summary>
    public class DialogService : IDialogService
    {
        public bool? ShowVocabularyEditDialog(VocabularyManagementViewModel viewModel)
        {
            var dialog = new VocabularyEditDialog
            {
                DataContext = viewModel,
                Owner = Application.Current.MainWindow
            };

            return dialog.ShowDialog();
        }

        public bool? ShowCategoryEditDialog(VocabularyManagementViewModel viewModel)
        {
            var dialog = new CategoryEditDialog
            {
                DataContext = viewModel,
                Owner = Application.Current.MainWindow
            };

            return dialog.ShowDialog();
        }

        public bool ShowConfirmDialog(string message, string title = "Confirm")
        {
            var result = MessageBox.Show(
                message, 
                title, 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Question,
                MessageBoxResult.No);

            return result == MessageBoxResult.Yes;
        }

        public void ShowInfoDialog(string message, string title = "Information")
        {
            MessageBox.Show(
                message, 
                title, 
                MessageBoxButton.OK, 
                MessageBoxImage.Information);
        }

        public void ShowErrorDialog(string message, string title = "Error")
        {
            MessageBox.Show(
                message, 
                title, 
                MessageBoxButton.OK, 
                MessageBoxImage.Error);
        }

        public string ShowOpenFileDialog(string filter = "All files (*.*)|*.*", string defaultExt = "")
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = filter,
                DefaultExt = defaultExt
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        public string ShowSaveFileDialog(string filter = "All files (*.*)|*.*", string defaultExt = "", string defaultFileName = "")
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = filter,
                DefaultExt = defaultExt,
                FileName = defaultFileName
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }
    }
}