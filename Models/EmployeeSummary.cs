namespace EmployeeDashboard.Models
{
    public class EmployeeSummary
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public string? EmployeeName { get; set; }
        public double TotalHoursWorked { get; set; }
    }
}
