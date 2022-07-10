using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Employee.API.Data;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Employee.API
{
    public class CreateFunctionOrch
    {
        private readonly IEmployeeStore _employeeStore;

        public CreateFunctionOrch(IEmployeeStore employeeStore)
        {
            _employeeStore = employeeStore;
        }

        [FunctionName("CreateFunctionOrch")]
        public async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context, 
            ILogger log)
        {
            var employee = context.GetInput<Sample.Web.Shared.Employee>();
            var outputs = new List<string>();

            log.LogInformation($"Saving Employee Orch started");

            outputs.Add(await context.CallActivityAsync<string>("SaveEmployee", employee));
            outputs.Add(await context.CallActivityAsync<string>("SendEmployeeSavedEvent", employee));

            //TODO call the SaveContract
            //TODO call the SaveHR

            return outputs;
        }

        [FunctionName("SaveEmployee")]
        public string SaveEmployee([ActivityTrigger] Sample.Web.Shared.Employee employee, ILogger log)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            log.LogInformation($"Saving Employee in db");
            _employeeStore.Employees.Add(employee);

            return $"Employee {employee.Id} saved!";
        }

        [FunctionName("SendEmployeeSavedEvent")]
        public async Task<string> SendEmployeeSavedEvent([ActivityTrigger] Sample.Web.Shared.Employee employee,
            [SignalR(ConnectionStringSetting = "AzureSignalRConnectionString", HubName = "employeeHub")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            log.LogInformation($"Sending Employee Added Event");

            await signalRMessages.AddAsync(
                                  new SignalRMessage
                                  {
                                      Target = "EmployeeAdded",
                                      Arguments = new[] { employee.ToSummary() }
                                  });

            return $"Employee Added Event {employee.Id} sent!";
        }

        [FunctionName("CreateFunction_HttpStart")]
        public async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "employee/create")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            var contentString = await req.Content.ReadAsStringAsync();
            var employee = JsonConvert.DeserializeObject<Sample.Web.Shared.Employee>(contentString);

            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("CreateFunctionOrch", employee);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}