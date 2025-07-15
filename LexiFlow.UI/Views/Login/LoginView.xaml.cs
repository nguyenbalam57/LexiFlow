using LexiFlow.Application.ViewModels;
using LexiFlow.UI.Helpers;
using Microsoft.Extensions.DependencyInjection;
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
        private bool _isClosing = false;

        public LoginView(LoginViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;

            // Load window settings
            LoadWindowSettings();

            // Set initial language
            LanguageHelper.CurrentLanguage = Properties.Settings.Default.PreferredLanguage ?? "VN";

            // Subscribe to events
            _viewModel.LoginSuccessful += ViewModel_LoginSuccessful;

            // Setup animations
            SetupAnimations();

            // Handle window events
            this.Loaded += LoginView_Loaded;
            this.Closing += LoginView_Closing;
            this.KeyDown += LoginView_KeyDown;

            // Focus management
            this.Activated += LoginView_Activated;
        }

        private void SetupAnimations()
        {
            // Add entrance animation
            if (Properties.Settings.Default.EnableAnimations)
            {
                var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(500));
                var slideIn = new ThicknessAnimation(
                    new Thickness(0, 50, 0, 0),
                    new Thickness(0, 0, 0, 0),
                    TimeSpan.FromMilliseconds(400));

                this.BeginAnimation(OpacityProperty, fadeIn);
            }
        }

        private void LoadWindowSettings()
        {
            try
            {
                var settings = Properties.Settings.Default;

                // Restore window size and position
                if (settings.WindowWidth > 0 && settings.WindowHeight > 0)
                {
                    this.Width = settings.WindowWidth;
                    this.Height = settings.WindowHeight;
                }

                if (settings.WindowLeft >= 0 && settings.WindowTop >= 0)
                {
                    this.Left = settings.WindowLeft;
                    this.Top = settings.WindowTop;
                }

                if (settings.IsMaximized)
                {
                    this.WindowState = WindowState.Maximized;
                }

                // Restore theme
                if (!string.IsNullOrEmpty(settings.ThemeMode))
                {
                    ApplyTheme(settings.ThemeMode);
                }

                // Restore font size
                if (settings.FontSize > 0)
                {
                    this.FontSize = settings.FontSize;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading window settings: {ex.Message}");
            }
        }

        private void SaveWindowSettings()
        {
            try
            {
                var settings = Properties.Settings.Default;

                settings.WindowWidth = this.Width;
                settings.WindowHeight = this.Height;
                settings.WindowLeft = this.Left;
                settings.WindowTop = this.Top;
                settings.IsMaximized = this.WindowState == WindowState.Maximized;

                settings.Save();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving window settings: {ex.Message}");
            }
        }

        private void ApplyTheme(string theme)
        {
            try
            {
                var resourceDictionaries = System.Windows.Application.Current.Resources.MergedDictionaries;

                // Remove existing theme dictionary
                var existingTheme = resourceDictionaries
                    .FirstOrDefault(rd => rd.Source?.OriginalString?.Contains("MaterialDesignTheme") == true);

                if (existingTheme != null)
                {
                    resourceDictionaries.Remove(existingTheme);
                }

                // Add new theme dictionary
                string themeUri = theme == "Dark"
                    ? "pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml"
                    : "pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml";

                var newTheme = new ResourceDictionary
                {
                    Source = new Uri(themeUri, UriKind.RelativeOrAbsolute)
                };

                resourceDictionaries.Insert(0, newTheme);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error applying theme: {ex.Message}");
            }
        }

        private void LoginView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Set focus to username field if empty, otherwise password field
                if (string.IsNullOrEmpty(_viewModel.Username))
                {
                    var usernameTextBox = FindVisualChild<TextBox>(this);
                    usernameTextBox?.Focus();
                }
                else
                {
                    PasswordBox?.Focus();
                }

                // Start periodic security check
                StartSecurityMonitoring();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in LoginView_Loaded: {ex.Message}");
            }
        }

        private void LoginView_Activated(object? sender, EventArgs e)
        {
            // Re-focus the appropriate field when window is activated
            if (string.IsNullOrEmpty(_viewModel.Username))
            {
                var usernameTextBox = FindVisualChild<TextBox>(this);
                usernameTextBox?.Focus();
            }
            else if (string.IsNullOrEmpty(_viewModel.Password))
            {
                PasswordBox?.Focus();
            }
        }

        private void LoginView_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                // Handle keyboard shortcuts
                switch (e.Key)
                {
                    case Key.Escape:
                        if (!_viewModel.IsLoading)
                        {
                            var result = MessageBox.Show(
                                LanguageHelper.GetLocalizedString("Login_ConfirmExit"),
                                LanguageHelper.GetLocalizedString("Login_ExitTitle"),
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question);

                            if (result == MessageBoxResult.Yes)
                            {
                                System.Windows.Application.Current.Shutdown();
                            }
                        }
                        e.Handled = true;
                        break;

                    case Key.F1:
                        ShowHelp();
                        e.Handled = true;
                        break;

                    case Key.F5:
                        RefreshLogin();
                        e.Handled = true;
                        break;

                    case Key.Tab:
                        // Enhanced tab navigation
                        HandleTabNavigation(e);
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error handling key down: {ex.Message}");
            }
        }

        private void HandleTabNavigation(KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
            {
                // Shift+Tab for reverse navigation
                MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
            }
            else
            {
                // Tab for forward navigation
                MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
            e.Handled = true;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox && DataContext is LoginViewModel viewModel)
            {
                viewModel.Password = passwordBox.Password;

                // Add visual feedback for password strength
                UpdatePasswordStrengthIndicator(passwordBox.Password);
            }
        }

        private void UpdatePasswordStrengthIndicator(string password)
        {
            // This could be expanded to show password strength
            // For now, just basic validation feedback
            try
            {
                if (!string.IsNullOrEmpty(password) && password.Length >= 6)
                {
                    // Password meets minimum requirements
                    PasswordBox.BorderBrush = Brushes.Green;
                }
                else if (!string.IsNullOrEmpty(password))
                {
                    // Password too short
                    PasswordBox.BorderBrush = Brushes.Orange;
                }
                else
                {
                    // No password
                    PasswordBox.ClearValue(Border.BorderBrushProperty);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating password indicator: {ex.Message}");
            }
        }

        private void ViewModel_LoginSuccessful(object? sender, EventArgs e)
        {
            try
            {
                _isClosing = true;

                // Save current settings
                SaveWindowSettings();

                // Show success animation if enabled
                if (Properties.Settings.Default.EnableAnimations)
                {
                    ShowSuccessAnimation(() => OpenMainWindow());
                }
                else
                {
                    OpenMainWindow();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error handling login success: {ex.Message}");
                OpenMainWindow(); // Fallback
            }
        }

        private void ShowSuccessAnimation(Action onComplete)
        {
            try
            {
                var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(300));
                fadeOut.Completed += (s, e) => onComplete();
                this.BeginAnimation(OpacityProperty, fadeOut);
            }
            catch
            {
                onComplete(); // Fallback if animation fails
            }
        }

        private void OpenMainWindow()
        {
            try
            {
                var mainWindow = App.Services.GetService<MainWindow>();
                if (mainWindow != null)
                {
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error opening main window.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening main window: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoginView_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_isClosing && _viewModel.IsLoading)
            {
                e.Cancel = true;
                MessageBox.Show(
                    LanguageHelper.GetLocalizedString("Login_CannotCloseWhileLoading"),
                    LanguageHelper.GetLocalizedString("Common_Warning"),
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            try
            {
                SaveWindowSettings();
                StopSecurityMonitoring();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during window closing: {ex.Message}");
            }
        }

        private void StartSecurityMonitoring()
        {
            // Monitor for suspicious activity
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(1)
            };
            timer.Tick += SecurityTimer_Tick;
            timer.Start();
        }

        private void SecurityTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                // Check for too many failed login attempts
                var settings = Properties.Settings.Default;
                if (settings.LoginAttempts >= 5)
                {
                    var timeSinceLastFailed = DateTime.Now - settings.LastFailedLogin;
                    if (timeSinceLastFailed.TotalMinutes < 15)
                    {
                        // Temporarily disable login
                        _viewModel.ErrorMessage = LanguageHelper.GetLocalizedString("Login_TooManyAttempts");
                    }
                    else
                    {
                        // Reset attempts after cooldown
                        settings.LoginAttempts = 0;
                        settings.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Security monitoring error: {ex.Message}");
            }
        }

        private void StopSecurityMonitoring()
        {
            // Clean up timers and monitoring
        }

        private void ShowHelp()
        {
            try
            {
                var helpMessage = LanguageHelper.GetLocalizedString("Login_HelpMessage");
                var helpTitle = LanguageHelper.GetLocalizedString("Login_HelpTitle");
                MessageBox.Show(helpMessage, helpTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("For help, please contact your system administrator.",
                    "Help", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RefreshLogin()
        {
            try
            {
                if (!_viewModel.IsLoading)
                {
                    _viewModel.ErrorMessage = string.Empty;
                    _viewModel.SuccessMessage = string.Empty;
                    PasswordBox.Clear();

                    var usernameTextBox = FindVisualChild<TextBox>(this);
                    usernameTextBox?.Focus();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error refreshing login: {ex.Message}");
            }
        }

        // Helper method to find visual children
        private static T? FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child is T typedChild)
                    return typedChild;

                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                    return childOfChild;
            }
            return null;
        }
    }

    // Custom converters
    //public class StringToVisibilityConverter : System.Windows.Data.IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        return string.IsNullOrEmpty(value as string) ? Visibility.Collapsed : Visibility.Visible;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        return value is Visibility visibility && visibility == Visibility.Visible ? "Visible" : string.Empty;
    //    }
    //}

    //public class InverseBooleanConverter : System.Windows.Data.IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        return !(bool)value;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        return !(bool)value;
    //    }
    //}
}
