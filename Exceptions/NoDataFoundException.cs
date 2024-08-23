namespace EmployeeDashboard.Exceptions
{
    public class NoDataFoundException : Exception
    {
        public NoDataFoundException(string message = "No data found in the response.") : base(message) { }
    }
}