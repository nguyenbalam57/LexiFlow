using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Legends;
using System;
using System.Collections.Generic;
using System.Linq;
using LexiFlow.AdminDashboard.Services;

namespace LexiFlow.AdminDashboard.Models.Charts
{
    /// <summary>
    /// Factory class for creating professional analytics charts with enhanced features
    /// </summary>
    public static class AnalyticsChartFactory
    {
        // Enhanced color palettes for professional look
        private static readonly string[] ModernColors = {
            "#3F51B5", "#009688", "#4CAF50", "#FF9800", "#F44336", 
            "#9C27B0", "#2196F3", "#FF5722", "#607D8B", "#795548"
        };

        private static readonly string[] GradientColors = {
            "#1976D2", "#388E3C", "#F57C00", "#D32F2F", "#7B1FA2",
            "#0288D1", "#689F38", "#E64A19", "#455A64", "#5D4037"
        };

        // Chart styling constants
        private const double DefaultFontSize = 12;
        private const double TitleFontSize = 16;
        private const double AxisFontSize = 10;

        #region Enhanced Existing Charts

        /// <summary>
        /// Create enhanced study time trend chart with predictions
        /// </summary>
        public static PlotModel CreateStudyTimeTrendChart(List<ChartDataPoint> data, string title = "Study Time Trend")
        {
            var plotModel = CreateBasePlotModel(title, "Date", "Minutes");

            // Historical data series
            var historicalSeries = new LineSeries
            {
                Title = "Historical Data",
                Color = OxyColor.Parse(ModernColors[0]),
                StrokeThickness = 2,
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerFill = OxyColor.Parse(ModernColors[0])
            };

            foreach (var point in data.OrderBy(d => d.Date))
            {
                historicalSeries.Points.Add(DateTimeAxis.CreateDataPoint(point.Date, point.Value));
            }

            plotModel.Series.Add(historicalSeries);

            // Add trend line
            if (data.Count > 1)
            {
                var trendSeries = CreateTrendLine(data, "Trend");
                plotModel.Series.Add(trendSeries);
            }

            // Add moving average
            if (data.Count > 7)
            {
                var movingAvgSeries = CreateMovingAverage(data, 7, "7-Day Average");
                plotModel.Series.Add(movingAvgSeries);
            }

            AddInteractiveFeatures(plotModel);
            return plotModel;
        }

        /// <summary>
        /// Create predictive chart combining historical and forecast data
        /// </summary>
        public static PlotModel CreatePredictiveChart(List<ChartDataPoint> historicalData, 
            List<PredictedDataPoint> predictedData, string title = "Learning Progress Prediction")
        {
            var plotModel = CreateBasePlotModel(title, "Date", "Progress Value");

            // Historical data series
            var historicalSeries = new LineSeries
            {
                Title = "Historical Data",
                Color = OxyColor.Parse(ModernColors[0]),
                StrokeThickness = 2,
                MarkerType = MarkerType.Circle,
                MarkerSize = 4
            };

            foreach (var point in historicalData.OrderBy(d => d.Date))
            {
                historicalSeries.Points.Add(DateTimeAxis.CreateDataPoint(point.Date, point.Value));
            }

            plotModel.Series.Add(historicalSeries);

            // Predicted data series
            var predictedSeries = new LineSeries
            {
                Title = "Predicted Data",
                Color = OxyColor.Parse(ModernColors[1]),
                StrokeThickness = 2,
                LineStyle = LineStyle.Dash,
                MarkerType = MarkerType.Triangle,
                MarkerSize = 4
            };

            foreach (var point in predictedData.OrderBy(d => d.Date))
            {
                predictedSeries.Points.Add(DateTimeAxis.CreateDataPoint(point.Date, point.VocabularyCount));
            }

            plotModel.Series.Add(predictedSeries);

            // Confidence interval
            var confidenceArea = new AreaSeries
            {
                Title = "Confidence Interval",
                Color = OxyColor.FromArgb(50, 76, 175, 80),
                Color2 = OxyColors.Transparent,
                Fill = OxyColor.FromArgb(30, 76, 175, 80)
            };

            foreach (var point in predictedData.OrderBy(d => d.Date))
            {
                var baseValue = point.VocabularyCount;
                var margin = baseValue * (1 - point.ConfidenceLevel) * 0.2; // 20% margin based on confidence
                
                confidenceArea.Points.Add(DateTimeAxis.CreateDataPoint(point.Date, baseValue + margin));
                confidenceArea.Points2.Add(DateTimeAxis.CreateDataPoint(point.Date, baseValue - margin));
            }

            plotModel.Series.Add(confidenceArea);

            AddPredictiveLegend(plotModel);
            AddInteractiveFeatures(plotModel);
            return plotModel;
        }

        /// <summary>
        /// Create efficiency analysis radar chart
        /// </summary>
        public static PlotModel CreateEfficiencyChart(StudyEfficiencyAnalysis analysis, string title = "Study Efficiency Analysis")
        {
            var plotModel = CreateBasePlotModel(title, "Efficiency Metrics", "Score");

            // Efficiency metrics bar chart
            var series = new ColumnSeries
            {
                Title = "Efficiency Metrics",
                FillColor = OxyColor.Parse(ModernColors[0]),
                StrokeColor = OxyColor.Parse(GradientColors[0]),
                StrokeThickness = 1
            };

            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Metrics",
                FontSize = AxisFontSize
            };

            // Add efficiency metrics
            categoryAxis.Labels.Add("Time Efficiency");
            series.Items.Add(new ColumnItem(analysis.TimeEfficiency * 100));

            categoryAxis.Labels.Add("Learning Velocity");
            series.Items.Add(new ColumnItem(analysis.LearningVelocity * 10)); // Scale for visualization

            categoryAxis.Labels.Add("Retention Rate");
            series.Items.Add(new ColumnItem(analysis.RetentionRate * 100));

            categoryAxis.Labels.Add("Overall Score");
            series.Items.Add(new ColumnItem(analysis.EfficiencyScore * 10));

            plotModel.Axes.Clear();
            plotModel.Axes.Add(categoryAxis);
            plotModel.Axes.Add(new LinearAxis 
            { 
                Position = AxisPosition.Left, 
                Title = "Score (%)",
                FontSize = AxisFontSize,
                MinimumPadding = 0.1,
                MaximumPadding = 0.1
            });

            plotModel.Series.Add(series);

            AddInteractiveFeatures(plotModel);
            return plotModel;
        }

        #endregion

        #region Enhanced Performance Charts

        public static PlotModel CreatePerformanceComparisonChart(List<ChartDataPoint> data, string title = "Performance Trends by Skill")
        {
            var plotModel = CreateBasePlotModel(title, "Date", "Accuracy (%)");

            var skillGroups = data.GroupBy(d => d.Category).ToList();
            
            for (int i = 0; i < skillGroups.Count && i < ModernColors.Length; i++)
            {
                var group = skillGroups[i];
                var series = new LineSeries
                {
                    Title = group.Key,
                    Color = OxyColor.Parse(ModernColors[i]),
                    StrokeThickness = 2,
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 3,
                    MarkerFill = OxyColor.Parse(ModernColors[i])
                };

                foreach (var point in group.OrderBy(d => d.Date))
                {
                    series.Points.Add(DateTimeAxis.CreateDataPoint(point.Date, point.Value));
                }

                // Add trend annotation for each skill
                if (group.Count() > 1)
                {
                    var trend = CalculateTrend(group.ToList());
                    series.Title += $" (Trend: {trend:+0.0;-0.0}%)";
                }

                plotModel.Series.Add(series);
            }

            // Add benchmark line at 80%
            var benchmarkSeries = new LineSeries
            {
                Title = "Target (80%)",
                Color = OxyColors.Gray,
                StrokeThickness = 1,
                LineStyle = LineStyle.Dash
            };

            if (data.Any())
            {
                var minDate = data.Min(d => d.Date);
                var maxDate = data.Max(d => d.Date);
                benchmarkSeries.Points.Add(DateTimeAxis.CreateDataPoint(minDate, 80));
                benchmarkSeries.Points.Add(DateTimeAxis.CreateDataPoint(maxDate, 80));
            }

            plotModel.Series.Add(benchmarkSeries);

            AddInteractiveFeatures(plotModel);
            return plotModel;
        }

        #endregion

        #region Enhanced Skill Breakdown Charts

        public static PlotModel CreateSkillBreakdownPieChart(List<SkillBreakdownData> data, string title = "Skill Accuracy Breakdown")
        {
            var plotModel = CreateBasePlotModel(title);

            var pieSeries = new PieSeries
            {
                StrokeThickness = 2,
                AngleSpan = 360,
                StartAngle = 0,
                InnerDiameter = 0.2, // Create donut chart
                FontSize = 12,
                FontWeight = FontWeights.Bold
            };

            for (int i = 0; i < data.Count && i < ModernColors.Length; i++)
            {
                var skill = data[i];
                var slice = new PieSlice(skill.SkillName, skill.AccuracyRate)
                {
                    IsExploded = skill.AccuracyRate == data.Max(s => s.AccuracyRate), // Explode best skill
                    Fill = OxyColor.Parse(ModernColors[i])
                };

                pieSeries.Slices.Add(slice);
            }

            plotModel.Series.Add(pieSeries);

            // Add center text for donut chart
            plotModel.Annotations.Add(new OxyPlot.Annotations.TextAnnotation
            {
                Text = $"Avg\n{data.Average(s => s.AccuracyRate):F1}%",
                Position = new DataPoint(0.5, 0.5),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Middle,
                FontSize = 14,
                FontWeight = FontWeights.Bold
            });

            return plotModel;
        }

        #endregion

        #region Study Pattern Charts

        public static PlotModel CreateStudyPatternColumnChart(List<StudyPatternData> data, string title = "Daily Study Pattern")
        {
            var plotModel = CreateBasePlotModel(title, "Hour of Day", "Study Minutes");

            var columnSeries = new ColumnSeries
            {
                Title = "Study Time",
                FillColor = OxyColor.Parse(ModernColors[0]),
                StrokeColor = OxyColor.Parse(GradientColors[0]),
                StrokeThickness = 1
            };

            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Hour of Day",
                FontSize = AxisFontSize
            };

            foreach (var hourData in data.OrderBy(d => d.Hour))
            {
                categoryAxis.Labels.Add($"{hourData.Hour:00}:00");
                
                var color = hourData.StudyMinutes > 30 ? 
                    OxyColor.Parse(ModernColors[2]) : // Green for peak hours
                    OxyColor.Parse(ModernColors[0]);  // Blue for normal hours
                
                columnSeries.Items.Add(new ColumnItem(hourData.StudyMinutes) { Color = color });
            }

            plotModel.Axes.Clear();
            plotModel.Axes.Add(categoryAxis);
            plotModel.Axes.Add(new LinearAxis 
            { 
                Position = AxisPosition.Left, 
                Title = "Minutes",
                FontSize = AxisFontSize,
                MinimumPadding = 0.1
            });

            plotModel.Series.Add(columnSeries);

            // Add peak hour annotations
            var peakHours = data.Where(d => d.StudyMinutes == data.Max(x => x.StudyMinutes)).ToList();
            foreach (var peak in peakHours)
            {
                plotModel.Annotations.Add(new OxyPlot.Annotations.TextAnnotation
                {
                    Text = "Peak",
                    Position = new DataPoint(peak.Hour, peak.StudyMinutes + 5),
                    FontSize = 10,
                    TextColor = OxyColors.Red
                });
            }

            AddInteractiveFeatures(plotModel);
            return plotModel;
        }

        #endregion

        #region Progress Comparison Area Charts

        public static PlotModel CreateProgressComparisonAreaChart(StudyTimeSeriesData data, string title = "Learning Progress Comparison")
        {
            var plotModel = CreateBasePlotModel(title, "Date", "Cumulative Count");

            // Vocabulary progress area
            var vocabSeries = new AreaSeries
            {
                Title = "Vocabulary",
                Color = OxyColor.Parse(ModernColors[0]),
                Color2 = OxyColors.Transparent,
                Fill = OxyColor.FromArgb(80, OxyColor.Parse(ModernColors[0]).R, OxyColor.Parse(ModernColors[0]).G, OxyColor.Parse(ModernColors[0]).B),
                StrokeThickness = 2
            };

            foreach (var point in data.VocabularyProgress.OrderBy(d => d.Date))
            {
                vocabSeries.Points.Add(DateTimeAxis.CreateDataPoint(point.Date, point.Value));
            }

            // Kanji progress area
            var kanjiSeries = new AreaSeries
            {
                Title = "Kanji",
                Color = OxyColor.Parse(ModernColors[1]),
                Color2 = OxyColors.Transparent,
                Fill = OxyColor.FromArgb(80, OxyColor.Parse(ModernColors[1]).R, OxyColor.Parse(ModernColors[1]).G, OxyColor.Parse(ModernColors[1]).B),
                StrokeThickness = 2
            };

            foreach (var point in data.KanjiProgress.OrderBy(d => d.Date))
            {
                kanjiSeries.Points.Add(DateTimeAxis.CreateDataPoint(point.Date, point.Value));
            }

            // Grammar progress area
            var grammarSeries = new AreaSeries
            {
                Title = "Grammar",
                Color = OxyColor.Parse(ModernColors[2]),
                Color2 = OxyColors.Transparent,
                Fill = OxyColor.FromArgb(80, OxyColor.Parse(ModernColors[2]).R, OxyColor.Parse(ModernColors[2]).G, OxyColor.Parse(ModernColors[2]).B),
                StrokeThickness = 2
            };

            foreach (var point in data.GrammarProgress.OrderBy(d => d.Date))
            {
                grammarSeries.Points.Add(DateTimeAxis.CreateDataPoint(point.Date, point.Value));
            }

            plotModel.Series.Add(vocabSeries);
            plotModel.Series.Add(kanjiSeries);
            plotModel.Series.Add(grammarSeries);

            AddInteractiveFeatures(plotModel);
            return plotModel;
        }

        #endregion

        #region Heatmap Charts

        /// <summary>
        /// Create study heatmap showing activity intensity
        /// </summary>
        public static PlotModel CreateStudyHeatmap(List<StudyPatternData> data, string title = "Study Activity Heatmap")
        {
            var plotModel = CreateBasePlotModel(title, "Hour", "Day of Week");

            var heatmapSeries = new HeatMapSeries
            {
                X0 = 0,
                X1 = 23,
                Y0 = 0,
                Y1 = 6,
                Interpolate = false,
                RenderMethod = HeatMapRenderMethod.Rectangles
            };

            // Create data array for heatmap
            var heatmapData = new double[7, 24]; // 7 days, 24 hours

            foreach (var point in data)
            {
                if (point.Hour >= 0 && point.Hour < 24 && point.DayOfWeek >= 0 && point.DayOfWeek < 7)
                {
                    heatmapData[point.DayOfWeek, point.Hour] = point.StudyMinutes;
                }
            }

            heatmapSeries.Data = heatmapData;

            plotModel.Series.Add(heatmapSeries);

            // Configure axes
            var hourAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Hour of Day",
                FontSize = AxisFontSize,
                Minimum = 0,
                Maximum = 23
            };

            var dayAxis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                Title = "Day of Week",
                FontSize = AxisFontSize
            };

            dayAxis.Labels.AddRange(new[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" });

            plotModel.Axes.Clear();
            plotModel.Axes.Add(hourAxis);
            plotModel.Axes.Add(dayAxis);

            return plotModel;
        }

        #endregion

        #region Helper Methods

        private static PlotModel CreateBasePlotModel(string title, string xAxisTitle = "", string yAxisTitle = "")
        {
            var plotModel = new PlotModel
            {
                Title = title,
                TitleFontSize = TitleFontSize,
                TitleFontWeight = FontWeights.Bold,
                Background = OxyColors.White,
                PlotAreaBorderColor = OxyColor.Parse("#E0E0E0"),
                PlotAreaBorderThickness = new OxyThickness(1),
                TextColor = OxyColor.Parse("#333333"),
                DefaultFontSize = DefaultFontSize
            };

            // Add legend
            plotModel.Legends.Add(new Legend
            {
                LegendPosition = LegendPosition.RightTop,
                LegendPlacement = LegendPlacement.Outside,
                LegendOrientation = LegendOrientation.Vertical,
                LegendFontSize = 11,
                LegendMargin = 10
            });

            // Configure axes if titles provided
            if (!string.IsNullOrEmpty(xAxisTitle))
            {
                plotModel.Axes.Add(new DateTimeAxis
                {
                    Position = AxisPosition.Bottom,
                    Title = xAxisTitle,
                    FontSize = AxisFontSize,
                    StringFormat = "MM/dd",
                    IntervalType = DateTimeIntervalType.Days,
                    MinorIntervalType = DateTimeIntervalType.Days,
                    IntervalLength = 50
                });
            }

            if (!string.IsNullOrEmpty(yAxisTitle))
            {
                plotModel.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Title = yAxisTitle,
                    FontSize = AxisFontSize,
                    MinimumPadding = 0.1,
                    MaximumPadding = 0.1
                });
            }

            return plotModel;
        }

        private static LineSeries CreateTrendLine(List<ChartDataPoint> data, string title)
        {
            var trendSeries = new LineSeries
            {
                Title = title,
                Color = OxyColors.Red,
                StrokeThickness = 1,
                LineStyle = LineStyle.Dot
            };

            if (data.Count >= 2)
            {
                var orderedData = data.OrderBy(d => d.Date).ToList();
                var firstPoint = orderedData.First();
                var lastPoint = orderedData.Last();

                var slope = (lastPoint.Value - firstPoint.Value) / (lastPoint.Date - firstPoint.Date).TotalDays;
                var intercept = firstPoint.Value - slope * firstPoint.Date.ToOADate();

                foreach (var point in orderedData)
                {
                    var trendValue = slope * point.Date.ToOADate() + intercept;
                    trendSeries.Points.Add(DateTimeAxis.CreateDataPoint(point.Date, trendValue));
                }
            }

            return trendSeries;
        }

        private static LineSeries CreateMovingAverage(List<ChartDataPoint> data, int windowSize, string title)
        {
            var movingAvgSeries = new LineSeries
            {
                Title = title,
                Color = OxyColors.Orange,
                StrokeThickness = 2,
                LineStyle = LineStyle.Dash
            };

            var orderedData = data.OrderBy(d => d.Date).ToList();
            
            for (int i = windowSize - 1; i < orderedData.Count; i++)
            {
                var window = orderedData.Skip(i - windowSize + 1).Take(windowSize);
                var average = window.Average(p => p.Value);
                movingAvgSeries.Points.Add(DateTimeAxis.CreateDataPoint(orderedData[i].Date, average));
            }

            return movingAvgSeries;
        }

        private static void AddInteractiveFeatures(PlotModel plotModel)
        {
            // Add tracker for interactive tooltips
            plotModel.MouseDown += (s, e) =>
            {
                if (e.HitTestResult?.Item is DataPoint dataPoint)
                {
                    // Custom tooltip logic can be added here
                }
            };

            // Enable zooming and panning
            plotModel.IsLegendVisible = true;
        }

        private static void AddPredictiveLegend(PlotModel plotModel)
        {
            // Customize legend for predictive charts
            if (plotModel.Legends.Any())
            {
                var legend = plotModel.Legends.First();
                legend.LegendTitle = "Data Types";
                legend.LegendTitleFontSize = 12;
                legend.LegendTitleFontWeight = FontWeights.Bold;
            }
        }

        private static double CalculateTrend(List<ChartDataPoint> data)
        {
            if (data.Count < 2) return 0;

            var orderedData = data.OrderBy(d => d.Date).ToList();
            var firstValue = orderedData.Take(orderedData.Count / 3).Average(p => p.Value);
            var lastValue = orderedData.Skip(orderedData.Count * 2 / 3).Average(p => p.Value);

            return firstValue > 0 ? ((lastValue - firstValue) / firstValue) * 100 : 0;
        }

        #endregion

        #region Chart Export Enhancement Methods

        /// <summary>
        /// Prepare chart for high-quality export
        /// </summary>
        public static PlotModel PrepareForExport(PlotModel originalModel, ExportSettings settings = null)
        {
            settings = settings ?? new ExportSettings();

            var exportModel = new PlotModel();
            originalModel.CopyTo(exportModel);

            // Enhance for export
            exportModel.Background = OxyColors.White;
            exportModel.TextColor = OxyColors.Black;
            exportModel.DefaultFontSize = settings.FontSize;
            exportModel.TitleFontSize = settings.TitleFontSize;

            // Add export watermark if specified
            if (!string.IsNullOrEmpty(settings.Watermark))
            {
                exportModel.Annotations.Add(new OxyPlot.Annotations.TextAnnotation
                {
                    Text = settings.Watermark,
                    Position = new DataPoint(0.98, 0.02),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    FontSize = 8,
                    TextColor = OxyColors.LightGray
                });
            }

            // Add timestamp
            exportModel.Annotations.Add(new OxyPlot.Annotations.TextAnnotation
            {
                Text = $"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                Position = new DataPoint(0.02, 0.02),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                FontSize = 8,
                TextColor = OxyColors.Gray
            });

            return exportModel;
        }

        #endregion
    }

    #region Data Models

    public class ChartDataPoint
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public string Label { get; set; }
        public string Category { get; set; }
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    public class StudyTimeSeriesData
    {
        public List<ChartDataPoint> DailyStudyTime { get; set; } = new();
        public List<ChartDataPoint> PerformanceTrend { get; set; } = new();
        public List<ChartDataPoint> VocabularyProgress { get; set; } = new();
        public List<ChartDataPoint> KanjiProgress { get; set; } = new();
        public List<ChartDataPoint> GrammarProgress { get; set; } = new();
    }

    public class SkillBreakdownData
    {
        public string SkillName { get; set; }
        public double AccuracyRate { get; set; }
        public int TotalAttempts { get; set; }
        public double ImprovementRate { get; set; }
        public string Level { get; set; }
        public OxyColor Color { get; set; } = OxyColors.Blue;
    }

    public class StudyPatternData
    {
        public int Hour { get; set; }
        public int DayOfWeek { get; set; }
        public double StudyMinutes { get; set; }
        public int Sessions { get; set; }
    }

    public class ExportSettings
    {
        public double FontSize { get; set; } = 12;
        public double TitleFontSize { get; set; } = 16;
        public string Watermark { get; set; } = "LexiFlow Analytics";
        public bool IncludeTimestamp { get; set; } = true;
        public bool HighQuality { get; set; } = true;
    }

    #endregion
}