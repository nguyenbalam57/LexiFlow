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
using Microsoft.Extensions.Logging;

namespace LexiFlow.AdminDashboard.Views
{
    /// <summary>
    /// Interaction logic for VocabularyManagementView.xaml with Dependency Injection
    /// </summary>
    public partial class VocabularyManagementView : UserControl
    {
        private readonly VocabularyManagementViewModel _viewModel;
        private readonly ILogger<VocabularyManagementView> _logger;

        public VocabularyManagementView(VocabularyManagementViewModel viewModel, ILogger<VocabularyManagementView> logger)
        {
            _viewModel = viewModel;
            _logger = logger;
            
            InitializeComponent();
            
            _logger.LogInformation("VocabularyManagementView initializing...");
            
            // Set DataContext
            DataContext = _viewModel;
            
            _logger.LogInformation("VocabularyManagementView initialized successfully");
        }
        
        /// <summary>
        /// Refresh data in the view
        /// </summary>
        public void RefreshData()
        {
            try
            {
                _logger.LogInformation("Refreshing vocabulary management data");
                _viewModel.RefreshCommand?.Execute(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing vocabulary data");
            }
        }

        /// <summary>
        /// Async refresh method for MainWindow
        /// </summary>
        public async Task RefreshAsync()
        {
            try
            {
                _logger.LogInformation("Async refreshing vocabulary management data");
                
                if (_viewModel.RefreshCommand?.CanExecute(null) == true)
                {
                    _viewModel.RefreshCommand.Execute(null);
                }
                
                // Allow UI to update
                await Task.Delay(100);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during async refresh");
            }
        }
    }
}
