using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Employee.API.Data;
using System.Linq;

namespace Employee.API
{
    public class QueriesFunctions
    {
        private readonly IEmployeeStore _employeeStore;

        public QueriesFunctions(IEmployeeStore employeeStore)
        {
            _employeeStore = employeeStore;
        }

        [FunctionName("GetEmployees")]
        public async Task<IActionResult> GetEmployees(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "employee")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Starting GetEmployee request");

            var result = _employeeStore.Employees.Select(s => s.ToSummary());

            return new OkObjectResult(result);
        }

        [FunctionName("GetEmployee")]
        public async Task<IActionResult> GetEmployee(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "employee/{id}")] HttpRequest req,
            string id,
            ILogger log)
        {
            log.LogInformation("Starting GetEmployees request");

            //TODO validate before cast
            var employeeId = Guid.Parse(id);
            var employee = _employeeStore.Employees.SingleOrDefault(t => t.Id == employeeId);
            if (employee == null) return new NotFoundResult();

            return new JsonResult(employee);
        }
    }
}
