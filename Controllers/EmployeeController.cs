using EmployeeDashboard.Exceptions;
using EmployeeDashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDashboard.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                var employeeSummaries = await _employeeService.GetEmployeeSummariesAsync();
                return View(employeeSummaries);
            }
            catch (NoDataFoundException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("NoDataFound");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpGet("employee/employee-summaries")]
        public async Task<IActionResult> GetEmployeeSummaries()
        {
            try
            {
                var employeeSummaries = await _employeeService.GetEmployeeSummariesAsync();
                return Ok(employeeSummaries);
            }
            catch (NoDataFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
