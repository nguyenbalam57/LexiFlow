using Microsoft.Extensions.Logging;
using OxyPlot;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace LexiFlow.AdminDashboard.Services
{
    /// <summary>
    /// Service for exporting charts to various formats (PNG, SVG)
    /// Note: PDF functionality requires PdfSharp NuGet package
    /// </summary>
    public interface IChartExportService
    {
        Task<bool> ExportChartToPngAsync(PlotModel plotModel, string filePath, int width = 800, int height = 600);
        Task<bool> ExportChartToPdfAsync(PlotModel plotModel, string filePath, int width = 800, int height = 600);
        Task<bool> ExportChartToSvgAsync(PlotModel plotModel, string filePath, int width = 800, int height = 600);
        Task<bool> ExportMultipleChartsToPdfAsync(Dictionary<string, PlotModel> charts, string filePath);
        Task<string> ShowSaveFileDialogAsync(string defaultFileName, string filter = "PNG Files|*.png|SVG Files|*.svg|All Files|*.*");
        Task<bool> PrintChartAsync(PlotModel plotModel);
        byte[] ExportChartToBytes(PlotModel plotModel, string format, int width = 800, int height = 600);
        Task<bool> ExportDashboardAsync(Dictionary<string, PlotModel> charts, string filePath, string title = "Analytics Dashboard");
    }

    public class ChartExportService : IChartExportService
    {
        private readonly ILogger<ChartExportService> _logger;

        public ChartExportService(ILogger<ChartExportService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> ExportChartToPngAsync(PlotModel plotModel, string filePath, int width = 800, int height = 600)
        {
            try
            {
                await Task.Run(() =>
                {
                    var pngExporter = new PngExporter
                    {
                        Width = width,
                        Height = height,
                        Resolution = 96
                    };

                    using (var stream = File.Create(filePath))
                    {
                        pngExporter.Export(plotModel, stream);
                    }
                });

                _logger.LogInformation("Chart exported to PNG: {FilePath}", filePath);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting chart to PNG: {FilePath}", filePath);
                return false;
            }
        }

        public async Task<bool> ExportChartToPdfAsync(PlotModel plotModel, string filePath, int width = 800, int height = 600)
        {
            try
            {
                // For now, we'll export as PNG and show a message about PDF capability
                _logger.LogWarning("PDF export requires PdfSharp NuGet package. Exporting as PNG instead.");
                
                var pngPath = Path.ChangeExtension(filePath, ".png");
                var result = await ExportChartToPngAsync(plotModel, pngPath, width, height);
                
                if (result)
                {
                    MessageBox.Show($"PDF export is not available in this version. Chart exported as PNG to:\n{pngPath}", 
                        "Export Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting chart to PDF: {FilePath}", filePath);
                return false;
            }
        }

        public async Task<bool> ExportChartToSvgAsync(PlotModel plotModel, string filePath, int width = 800, int height = 600)
        {
            try
            {
                await Task.Run(() =>
                {
                    var svgExporter = new OxyPlot.SvgExporter
                    {
                        Width = width,
                        Height = height,
                        UseVerticalTextAlignmentWorkaround = true
                    };

                    using (var stream = File.Create(filePath))
                    {
                        svgExporter.Export(plotModel, stream);
                    }
                });

                _logger.LogInformation("Chart exported to SVG: {FilePath}", filePath);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting chart to SVG: {FilePath}", filePath);
                return false;
            }
        }

        public async Task<bool> ExportMultipleChartsToPdfAsync(Dictionary<string, PlotModel> charts, string filePath)
        {
            try
            {
                // For now, export each chart as individual PNG files
                _logger.LogWarning("PDF multi-chart export requires PdfSharp NuGet package. Exporting as individual PNG files.");
                
                var directory = Path.GetDirectoryName(filePath);
                var baseFileName = Path.GetFileNameWithoutExtension(filePath);
                
                var exportedFiles = new List<string>();
                
                foreach (var chartPair in charts)
                {
                    var chartFileName = Path.Combine(directory, $"{baseFileName}_{chartPair.Key.Replace(" ", "_")}.png");
                    var success = await ExportChartToPngAsync(chartPair.Value, chartFileName, 800, 600);
                    
                    if (success)
                    {
                        exportedFiles.Add(chartFileName);
                    }
                }

                if (exportedFiles.Count > 0)
                {
                    MessageBox.Show($"Multi-chart PDF export is not available in this version. Charts exported as {exportedFiles.Count} PNG files in:\n{directory}", 
                        "Export Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting multiple charts: {FilePath}", filePath);
                return false;
            }
        }

        public async Task<bool> ExportDashboardAsync(Dictionary<string, PlotModel> charts, string filePath, string title = "Analytics Dashboard")
        {
            return await ExportMultipleChartsToPdfAsync(charts, filePath);
        }

        public async Task<string> ShowSaveFileDialogAsync(string defaultFileName, string filter = "PNG Files|*.png|SVG Files|*.svg|All Files|*.*")
        {
            return await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var saveFileDialog = new SaveFileDialog
                {
                    FileName = defaultFileName,
                    Filter = filter,
                    DefaultExt = ".png",
                    Title = "Export Chart"
                };

                bool? result = saveFileDialog.ShowDialog();
                return result == true ? saveFileDialog.FileName : null;
            });
        }

        public async Task<bool> PrintChartAsync(PlotModel plotModel)
        {
            try
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    var printDialog = new System.Windows.Controls.PrintDialog();
                    
                    if (printDialog.ShowDialog() == true)
                    {
                        // Create a visual representation of the chart
                        var plotView = new PlotView
                        {
                            Model = plotModel,
                            Width = printDialog.PrintableAreaWidth,
                            Height = printDialog.PrintableAreaHeight
                        };

                        // Measure and arrange the visual
                        plotView.Measure(new System.Windows.Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight));
                        plotView.Arrange(new Rect(new System.Windows.Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight)));

                        // Print the visual
                        printDialog.PrintVisual(plotView, "LexiFlow Analytics Chart");
                        return true;
                    }
                    
                    return false;
                });

                _logger.LogInformation("Chart sent to printer");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error printing chart");
                return false;
            }
        }

        public byte[] ExportChartToBytes(PlotModel plotModel, string format, int width = 800, int height = 600)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    switch (format.ToLowerInvariant())
                    {
                        case "png":
                            var pngExporter = new PngExporter
                            {
                                Width = width,
                                Height = height,
                                Resolution = 96
                            };
                            pngExporter.Export(plotModel, stream);
                            break;

                        case "svg":
                            var svgExporter = new OxyPlot.SvgExporter
                            {
                                Width = width,
                                Height = height,
                                UseVerticalTextAlignmentWorkaround = true
                            };
                            svgExporter.Export(plotModel, stream);
                            break;

                        default:
                            throw new ArgumentException($"Unsupported format: {format}");
                    }

                    return stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting chart to bytes");
                throw;
            }
        }

        /// <summary>
        /// Export chart with custom styling and branding
        /// </summary>
        public async Task<bool> ExportChartWithBrandingAsync(PlotModel plotModel, string filePath, 
            string format = "png", string title = null, bool includeLogo = true)
        {
            try
            {
                await Task.Run(() =>
                {
                    // Clone the plot model to avoid modifying the original
                    var exportModel = ClonePlotModel(plotModel);

                    // Add branding elements
                    if (!string.IsNullOrEmpty(title))
                    {
                        exportModel.Title = title;
                        exportModel.TitleFontSize = 16;
                        exportModel.TitleFontWeight = OxyPlot.FontWeights.Bold;
                    }

                    // Add subtitle with timestamp
                    exportModel.Subtitle = $"Generated by LexiFlow Analytics - {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                    exportModel.SubtitleFontSize = 12;
                    exportModel.SubtitleColor = OxyColors.Gray;

                    // Set professional styling
                    exportModel.Background = OxyColors.White;
                    exportModel.PlotAreaBorderColor = OxyColors.Gray;
                    exportModel.PlotAreaBorderThickness = new OxyThickness(1);

                    // Export based on format
                    switch (format.ToLowerInvariant())
                    {
                        case "png":
                            var pngExporter = new PngExporter
                            {
                                Width = 1200,
                                Height = 800,
                                Resolution = 150
                            };
                            using (var stream = File.Create(filePath))
                            {
                                pngExporter.Export(exportModel, stream);
                            }
                            break;

                        case "svg":
                            var svgExporter = new OxyPlot.SvgExporter
                            {
                                Width = 1200,
                                Height = 800
                            };
                            using (var stream = File.Create(filePath))
                            {
                                svgExporter.Export(exportModel, stream);
                            }
                            break;

                        default:
                            throw new ArgumentException($"Unsupported format: {format}");
                    }
                });

                _logger.LogInformation("Chart with branding exported: {FilePath}", filePath);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting chart with branding: {FilePath}", filePath);
                return false;
            }
        }

        /// <summary>
        /// Simple clone method for PlotModel
        /// </summary>
        private PlotModel ClonePlotModel(PlotModel original)
        {
            var clone = new PlotModel
            {
                Title = original.Title,
                Subtitle = original.Subtitle,
                TitleFontSize = original.TitleFontSize,
                TitleFontWeight = original.TitleFontWeight,
                SubtitleFontSize = original.SubtitleFontSize,
                SubtitleColor = original.SubtitleColor,
                Background = original.Background,
                PlotAreaBorderColor = original.PlotAreaBorderColor,
                PlotAreaBorderThickness = original.PlotAreaBorderThickness,
                TextColor = original.TextColor,
                DefaultFontSize = original.DefaultFontSize
            };

            // Copy series
            foreach (var series in original.Series)
            {
                clone.Series.Add(series);
            }

            // Copy axes
            foreach (var axis in original.Axes)
            {
                clone.Axes.Add(axis);
            }

            // Copy legends
            foreach (var legend in original.Legends)
            {
                clone.Legends.Add(legend);
            }

            // Copy annotations
            foreach (var annotation in original.Annotations)
            {
                clone.Annotations.Add(annotation);
            }

            return clone;
        }
    }
}