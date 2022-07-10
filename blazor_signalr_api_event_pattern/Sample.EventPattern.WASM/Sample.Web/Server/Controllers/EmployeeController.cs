using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Sample.Web.Server.Hubs;
using Sample.Web.Server.Services;
using Sample.Web.Shared;

namespace Sample.Web.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IHubContext<EmployeeHub, IEmployeeHub> _hubContext;
        private readonly IMemoryCache _cache;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IHubContext<EmployeeHub, IEmployeeHub> employeeHub,
                                    IMemoryCache cache,
                                    IEmployeeService employeeService)
        {
            _hubContext = employeeHub;
            _cache = cache;
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

        // Note an [ApiController] will automatically return a 400 response if any
        // of the data annotation valiadations defined in AddSurveyModel fails
        [HttpPut()]
        public async Task<string> AddEmployee([FromBody] AddEmployeeModel addEmployeeModel)
        {
            var employee = new Employee
            {
                FirstName = addEmployeeModel.FirstName,
                FamilyName = addEmployeeModel.FamilyName,
                Gender = addEmployeeModel.Gender.HasValue ? addEmployeeModel.Gender.Value : Gender.Unknown,
                Address = addEmployeeModel.Address,
                IsTermContract = addEmployeeModel.IsTermContract
            };

            var durableResponse = await _employeeService.Create(employee);

            //don't send internal links to the outside
            //store the links in API and send only the Id
            _cache.Set(durableResponse.id, durableResponse);
                       
           // await _hubContext.Clients.All.EmployeeAdded(employee.ToSummary());
            return durableResponse.id;
        }
    }
}