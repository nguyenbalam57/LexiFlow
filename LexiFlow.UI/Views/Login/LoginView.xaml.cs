using LexiFlow.App.ViewModels;
using LexiFlow.UI.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LexiFlow.UI.Views.Login
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        private readonly LoginViewModel _viewModel;

        public LoginView()
        {
            // Set application culture
            SetDefaultCulture("en-US");

            InitializeComponent();

            // Create and set the view model
            _viewModel = new LoginViewModel();
            DataContext = _viewModel;

            // Set password changed event
            PasswordBox.PasswordChanged += PasswordBox_PasswordChanged;

            // Check for saved credentials
            LoadSavedCredentials();
        }

        private void SetDefaultCulture(string cultureName)
        {
            // Set current thread culture
            var culture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // Set application culture
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        private void LoadSavedCredentials()
        {
            // Load saved username if remember me is checked
            var settings = Properties.Settings.Default;
            if (settings.RememberMe)
            {
                _viewModel.Username = settings.SavedUsername;
                _viewModel.RememberMe = true;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Update the password in view model when the password box content changes
            if (sender is PasswordBox passwordBox)
            {
                _viewModel.Password = passwordBox.Password;
            }
        }

        private void LanguageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                string cultureName = "en-US"; // Default

                switch (comboBox.SelectedIndex)
                {
                    case 0: // English
                        cultureName = "en-US";
                        break;
                    case 1: // Vietnamese
                        cultureName = "vi-VN";
                        break;
                    case 2: // Japanese
                        cultureName = "ja-JP";
                        break;
                }

                ChangeLanguage(cultureName);
            }
        }

        private void ChangeLanguage(string cultureName)
        {
            // Change current thread culture
            var culture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // Change application resources
            ResourceDictionary dict = new ResourceDictionary();
            switch (cultureName)
            {
                case "vi-VN":
                    dict.Source = new Uri("pack://application:,,,/Resources/Languages/Vietnamese.xaml", UriKind.Absolute);
                    break;
                case "ja-JP":
                    dict.Source = new Uri("pack://application:,,,/Resources/Languages/Japanese.xaml", UriKind.Absolute);
                    break;
                default:
                    dict.Source = new Uri("pack://application:,,,/Resources/Languages/English.xaml", UriKind.Absolute);
                    break;
            }

            // Replace the current language dictionary
            var oldDict = Application.Current.Resources.MergedDictionaries.FirstOrDefault(
                d => d.Source != null && (
                    d.Source.OriginalString.Contains("English.xaml") ||
                    d.Source.OriginalString.Contains("Vietnamese.xaml") ||
                    d.Source.OriginalString.Contains("Japanese.xaml")
                ));

            if (oldDict != null)
            {
                int index = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                Application.Current.Resources.MergedDictionaries.Insert(index, dict);
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }

            // Save selected language preference
            Properties.Settings.Default.SelectedLanguage = cultureName;
            Properties.Settings.Default.Save();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Allow dragging the window
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }
    }
}
