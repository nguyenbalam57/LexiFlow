using System.Windows;
using LexiFlow.AdminDashboard.ViewModels;

namespace LexiFlow.AdminDashboard.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for VocabularyEditDialog.xaml
    /// </summary>
    public partial class VocabularyEditDialog : Window
    {
        public VocabularyEditDialog()
        {
            InitializeComponent();
        }

        public VocabularyEditDialog(VocabularyManagementViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}