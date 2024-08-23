using EmployeeDashboard.Models;

namespace EmployeeDashboard.Services
{
    public interface IChartService
    {
        byte[] GenerateEmployeeWorkPieChart(List<EmployeeSummary> employeeSummaries);
    }
}