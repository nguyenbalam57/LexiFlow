using LexiFlow.UI.Helpers;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LexiFlow.UI.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for DatabaseConfigDialog.xaml
    /// </summary>
    public partial class DatabaseConfigDialog : Window
    {
        public string ConnectionString { get; private set; } = string.Empty;
        public bool ConnectionTested { get; private set; } = false;
        public DatabaseConfigDialog()
        {
            InitializeComponent();
            InitializeUI();
        }
        private async void InitializeUI()
        {
            // Subscribe to radio button events
            WindowsAuthRadio.Checked += AuthRadio_Changed;
            SqlAuthRadio.Checked += AuthRadio_Changed;

            // Load available SQL servers
            try
            {
                var servers = await DatabaseConnectionHelper.GetAvailableSqlServersAsync();
                foreach (var server in servers)
                {
                    if (!ServerComboBox.Items.Cast<object>().Any(item =>
                        (item as ComboBoxItem)?.Content?.ToString() == server))
                    {
                        ServerComboBox.Items.Add(new ComboBoxItem { Content = server });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading servers: {ex.Message}");
            }

            // Set default selection
            ServerComboBox.SelectedIndex = 0;
        }

        private void AuthRadio_Changed(object sender, RoutedEventArgs e)
        {
            if (SqlAuthPanel != null)
            {
                SqlAuthPanel.Visibility = SqlAuthRadio.IsChecked == true
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        private async void TestConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            // Disable button during test
            TestConnectionButton.IsEnabled = false;
            TestResultBorder.Visibility = Visibility.Collapsed;

            try
            {
                // Build connection string
                string serverName = GetServerName();
                string databaseName = DatabaseTextBox.Text.Trim();

                if (string.IsNullOrEmpty(serverName) || string.IsNullOrEmpty(databaseName))
                {
                    ShowTestResult(false, "Vui lòng nhập đầy đủ thông tin server và database");
                    return;
                }

                ConnectionString = DatabaseConnectionHelper.GetConnectionString(
                    serverName,
                    databaseName,
                    WindowsAuthRadio.IsChecked == true,
                    UsernameTextBox.Text,
                    PasswordBox.Password
                );

                // Test connection
                var (success, message) = await DatabaseConnectionHelper.TestConnectionAsync(ConnectionString);

                ShowTestResult(success, message);

                if (success)
                {
                    ConnectionTested = true;
                    SaveButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                ShowTestResult(false, $"Lỗi: {ex.Message}");
            }
            finally
            {
                TestConnectionButton.IsEnabled = true;
            }
        }

        private string GetServerName()
        {
            if (ServerComboBox.SelectedItem is ComboBoxItem item)
            {
                return item.Content?.ToString() ?? "";
            }
            return ServerComboBox.Text.Trim();
        }

        private void ShowTestResult(bool success, string message)
        {
            TestResultBorder.Visibility = Visibility.Visible;

            if (success)
            {
                TestResultBorder.Background = new SolidColorBrush(Color.FromRgb(220, 252, 231));
                TestResultBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(134, 239, 172));
                TestResultIcon.Kind = PackIconKind.CheckCircle;
                TestResultIcon.Foreground = new SolidColorBrush(Color.FromRgb(22, 163, 74));
                TestResultText.Foreground = new SolidColorBrush(Color.FromRgb(22, 163, 74));
            }
            else
            {
                TestResultBorder.Background = new SolidColorBrush(Color.FromRgb(254, 226, 226));
                TestResultBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(252, 165, 165));
                TestResultIcon.Kind = PackIconKind.AlertCircle;
                TestResultIcon.Foreground = new SolidColorBrush(Color.FromRgb(220, 38, 38));
                TestResultText.Foreground = new SolidColorBrush(Color.FromRgb(220, 38, 38));
            }

            TestResultText.Text = message;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ConnectionTested)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show(
                    "Vui lòng test kết nối trước khi lưu.",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

    }
}
