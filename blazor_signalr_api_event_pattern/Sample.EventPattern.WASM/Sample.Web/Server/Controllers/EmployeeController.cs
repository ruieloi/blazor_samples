using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Sample.Web.Server.Data;
using Sample.Web.Server.Hubs;
using Sample.Web.Shared;

namespace Sample.Web.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IHubContext<EmployeeHub, IEmployeeHub> _hubContext;
        private readonly IEmployeeStore _employeeStore;

        public EmployeeController(IHubContext<EmployeeHub, IEmployeeHub> employeeHub, IEmployeeStore employeeStore)
        {
            _hubContext = employeeHub;
            _employeeStore = employeeStore;
        }

        [HttpGet()]
        public IEnumerable<EmployeeSummary> GetEmployees()
        {
            return _employeeStore.Employees.Select(s => s.ToSummary());
        }

        [HttpGet("{id}")]
        public ActionResult GetEmployee(Guid id)
        {
            var employee = _employeeStore.Employees.SingleOrDefault(t => t.Id == id);
            if (employee == null) return NotFound();

            return new JsonResult(employee);
        }

        // Note an [ApiController] will automatically return a 400 response if any
        // of the data annotation valiadations defined in AddSurveyModel fails
        [HttpPut()]
        public async Task<Employee> AddEmployee([FromBody] AddEmployeeModel addEmployeeModel)
        {
            var employee = new Employee
            {
                FirstName = addEmployeeModel.FirstName,
                FamilyName = addEmployeeModel.FamilyName,
                Gender = addEmployeeModel.Gender.HasValue ? addEmployeeModel.Gender.Value : Gender.Unknown,
                Address = addEmployeeModel.Address,
                IsTermContract = addEmployeeModel.IsTermContract
            };

            _employeeStore.Employees.Add(employee);

            await _hubContext.Clients.All.EmployeeAdded(employee.ToSummary());
            return employee;
        }
    }
}