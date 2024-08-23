using EmployeeDashboard.Models;
using OxyPlot;
using OxyPlot.ImageSharp;
using OxyPlot.Series;

namespace EmployeeDashboard.Services
{
    public class ChartService : IChartService
    {
        private const double PieStrokeThickness = 2.0;
        private const double PieInsideLabelPosition = 0.8;
        private const int ChartWidth = 600;
        private const int ChartHeight = 400;
        private const int Resolution = 96;

        public byte[] GenerateEmployeeWorkPieChart(List<EmployeeSummary> employeeSummaries)
        {
            if (employeeSummaries == null || !employeeSummaries.Any())
            {
                throw new ArgumentException("Employee summaries cannot be null or empty.", nameof(employeeSummaries));
            }

            var plotModel = CreatePlotModel();
            var pieSeries = CreatePieSeries(employeeSummaries);

            plotModel.Series.Add(pieSeries);

            return ExportPlotToPng(plotModel);
        }

        private static PlotModel CreatePlotModel()
        {
            return new PlotModel { Title = "Employee Work Distribution" };
        }

        private PieSeries CreatePieSeries(List<EmployeeSummary> employeeSummaries)
        {
            var pieSeries = new PieSeries
            {
                StrokeThickness = PieStrokeThickness,
                AngleSpan = 360,
                StartAngle = 0,
                InsideLabelPosition = PieInsideLabelPosition
            };

            var totalHours = employeeSummaries.Sum(e => e.TotalHoursWorked);

            foreach (var employee in employeeSummaries)
            {
                var percentage = CalculatePercentage(employee.TotalHoursWorked, totalHours);
                pieSeries.Slices.Add(CreatePieSlice(employee.EmployeeName, percentage, employee.TotalHoursWorked < 100));
            }

            return pieSeries;
        }

        private static double CalculatePercentage(double hoursWorked, double totalHours)
        {
            return (hoursWorked / totalHours) * 100;
        }

        private static PieSlice CreatePieSlice(string employeeName, double percentage, bool isExploded)
        {
            return new PieSlice(employeeName, percentage)
            {
                IsExploded = isExploded
            };
        }

        private static byte[] ExportPlotToPng(PlotModel plotModel)
        {
            using var ms = new MemoryStream();
            try
            {
                var pngExporter = new PngExporter(ChartWidth, ChartHeight, Resolution);
                pngExporter.Export(plotModel, ms);
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to generate PNG.", ex);
            }
        }
    }
}