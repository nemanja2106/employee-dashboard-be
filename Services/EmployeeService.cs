using System.Text.Json;
using EmployeeDashboard.Exceptions;
using EmployeeDashboard.Models;

namespace EmployeeDashboard.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient _httpClient;
        private readonly string _employeeActivitiesUrl;


        public EmployeeService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _employeeActivitiesUrl = configuration["ApiUrl"];
        }

        private async Task<List<EmployeeActivities>> GetEmployeeActivitiesAsync(string url)
        {
            var response = await _httpClient.GetStringAsync(url);
            var activeEmployeeActivities = JsonSerializer.Deserialize<List<EmployeeActivities>>(response)?
                .Where(e => e.DeletedOn == null && !string.IsNullOrEmpty(e.EmployeeName) && e.StarTimeUtc < e.EndTimeUtc)
                .ToList();
            if (activeEmployeeActivities == null || activeEmployeeActivities.Count == 0)
            {
                throw new NoDataFoundException("No active employee activities found in the response.");
            }
            return activeEmployeeActivities;
        }
        
        private List<EmployeeSummary> CalculateEmployeeSummaries(List<EmployeeActivities> employeeActivities)
        {
            return employeeActivities
                .GroupBy(e => e.EmployeeName)
                .Select(g => new EmployeeSummary
                {
                    EmployeeName = g.Key,
                    TotalHoursWorked = Math.Round(g.Sum(e => (e.EndTimeUtc - e.StarTimeUtc).TotalHours), 2)
                })
                .OrderByDescending(summary => summary.TotalHoursWorked)
                .ToList();
        }
        
        public async Task<List<EmployeeSummary>> GetEmployeeSummariesAsync()
        {
            var employeeActivities = await GetEmployeeActivitiesAsync(_employeeActivitiesUrl);
            return CalculateEmployeeSummaries(employeeActivities);
        }
    }
}