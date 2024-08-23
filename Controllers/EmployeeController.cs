using EmployeeDashboard.Exceptions;
using EmployeeDashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDashboard.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IChartService _chartService;

        public EmployeeController(IEmployeeService employeeService, IChartService chartService)
        {
            _employeeService = employeeService;
            _chartService = chartService;
        }
        

        public async Task<IActionResult> Index()
        {
            try
            {
                var employeeSummaries = await _employeeService.GetEmployeeSummariesAsync();
                var pieChartImage = _chartService.GenerateEmployeeWorkPieChart(employeeSummaries);
                ViewBag.PieChartImage = pieChartImage;
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
        
        [HttpGet("employee/employee-summaries/piechart")]
        public async Task<IActionResult> GetEmployeeWorkPieChart()
        {
            try
            {
                var employeeSummaries = await _employeeService.GetEmployeeSummariesAsync();
                var image = _chartService.GenerateEmployeeWorkPieChart(employeeSummaries);
                return File(image, "image/png");
            }
            catch (NoDataFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while generating the pie chart.");
            }
        }
    }
}