using EmployeeDashboard.Models;

namespace EmployeeDashboard.Services
{
    public interface IEmployeeService
    {
        Task<List<EmployeeSummary>> GetEmployeeSummariesAsync();
    }
}