using System.Windows;
using LexiFlow.AdminDashboard.ViewModels;

namespace LexiFlow.AdminDashboard.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for CategoryEditDialog.xaml
    /// </summary>
    public partial class CategoryEditDialog : Window
    {
        public CategoryEditDialog()
        {
            InitializeComponent();
        }

        public CategoryEditDialog(VocabularyManagementViewModel viewModel) : this()
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