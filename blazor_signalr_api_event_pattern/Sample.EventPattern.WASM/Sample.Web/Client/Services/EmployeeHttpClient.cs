using Sample.Web.Shared;
using System.Net.Http.Json;

namespace Sample.Web.Client.Services
{
    // See https://docs.microsoft.com/en-us/aspnet/core/blazor/call-web-api?view=aspnetcore-5.0#typed-httpclient
    // TODO: consider error handling!
    // Possibly configure a library like Polly to centralize error handling policies: https://github.com/App-vNext/Polly/wiki/Polly-and-HttpClientFactory
    public class EmployeeHttpClient
    {
        private readonly HttpClient http;

        public EmployeeHttpClient(HttpClient http)
        {
            this.http = http;
        }

        public async Task<EmployeeSummary[]> GetEmployees()
        {
            return await this.http.GetFromJsonAsync<EmployeeSummary[]>("api/employee");
        }

        public async Task<Employee> GetEmployee(Guid employeeId)
        {
            return await this.http.GetFromJsonAsync<Employee>($"api/employee/{employeeId}");
        }

        public async Task<HttpResponseMessage> AddEmployee(AddEmployeeModel employee)
        {
            return await this.http.PutAsJsonAsync<AddEmployeeModel>("api/employee", employee);
        }

        public async Task<string> CheckEmployeeStatus(string statusId)
        {
            return await this.http.GetStringAsync($"api/employee/status?{statusId}");
        }
    }
}