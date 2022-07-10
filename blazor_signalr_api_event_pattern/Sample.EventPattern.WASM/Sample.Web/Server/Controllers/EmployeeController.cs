using Microsoft.AspNetCore.Mvc;
using Sample.Web.Server.Services;
using Sample.Web.Shared;

namespace Sample.Web.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet()]
        public async Task<IEnumerable<EmployeeSummary>> GetEmployees()
        {
            return await _employeeService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetEmployee(Guid id)
        {
            var employee = await _employeeService.GetById(id);
            if (employee == null) return NotFound();

            return new JsonResult(employee);
        }

        [HttpGet("status/{statusId}")]
        public async Task<bool> CheckStatus(string statusId)
        {
            return await _employeeService.CheckStatus(statusId);
        }


        // Note an [ApiController] will automatically return a 400 response if any
        // of the data annotation valiadations defined in AddSurveyModel fails
        [HttpPut()]
        public async Task<Guid> AddEmployee([FromBody] AddEmployeeModel addEmployeeModel)
        {
            var employee = new Employee
            {
                FirstName = addEmployeeModel.FirstName,
                FamilyName = addEmployeeModel.FamilyName,
                Gender = addEmployeeModel.Gender.HasValue ? addEmployeeModel.Gender.Value : Gender.Unknown,
                Address = addEmployeeModel.Address,
                IsTermContract = addEmployeeModel.IsTermContract
            };
 
            return await _employeeService.Create(employee);
        }
    }
}