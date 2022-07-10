using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Employee.API
{
    public static class CreateFunctionOrch
    {
        [FunctionName("CreateFunctionOrch")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            // Replace "hello" with the name of your Durable Activity Function.
            outputs.Add(await context.CallActivityAsync<string>("SaveEmployee", "Tokyo"));
            outputs.Add(await context.CallActivityAsync<string>("SaveEmployee", "Seattle"));
            outputs.Add(await context.CallActivityAsync<string>("SaveEmployee", "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [FunctionName("SaveEmployee")]
        public static string SaveEmployee([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saving Employee");
            return $"Hello {name}!";
        }

        [FunctionName("CreateFunction_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "employee/create")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("CreateFunctionOrch", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}