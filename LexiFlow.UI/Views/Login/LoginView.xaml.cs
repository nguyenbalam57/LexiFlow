using LexiFlow.Application.ViewModels;
using LexiFlow.UI.Helpers;
using System.Windows;
using System.Windows.Controls;

namespace LexiFlow.UI.Views.Login
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        private readonly LoginViewModel _viewModel;

        public LoginView(LoginViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;

            // Set initial language
            LanguageHelper.CurrentLanguage = "VN";

            // Subscribe to login successful event
            _viewModel.LoginSuccessful += ViewModel_LoginSuccessful;

            // Setup converters if not available in XAML
            Resources.Add("BooleanToVisibilityConverter", new System.Windows.Controls.BooleanToVisibilityConverter());
            Resources.Add("StringToVisibilityConverter", new StringToVisibilityConverter());
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }

        private void ViewModel_LoginSuccessful(object? sender, EventArgs e)
        {
            // Open main window and close login window
            var mainWindow = App.Services.GetService(typeof(MainWindow)) as MainWindow;
            mainWindow?.Show();
            Close();
        }
    }

    // A simple converter to convert empty string to Collapsed and non-empty to Visible
    public class StringToVisibilityConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is Visibility visibility && visibility == Visibility.Visible ? "Visible" : string.Empty;
        }
    }
}
