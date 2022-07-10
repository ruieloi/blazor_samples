using Sample.Web.Server.Services.Utils;
using Sample.Web.Shared;

namespace Sample.Web.Server.Services
{
    public class EmployeeService : IEmployeeService
    {
        public const string HTTP_CLIENT_NAME = "employee_api";

        private readonly IHttpClientFactory _httpClientFactory;

        public EmployeeService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<DurableHttpResponse> Create(Employee employee)
        {
            var response = await GetClient().PostAsJsonAsync("employee/create", employee);
            if (response.IsSuccessStatusCode)
            {
                var durableResponse = await response.Content.ReadFromJsonAsync<DurableHttpResponse>();
                if (durableResponse != null && !string.IsNullOrEmpty(durableResponse.id))
                {
                    return durableResponse;
                }
            }

            //improve handling
            throw new Exception("Something happen calling the service");
        }

        private HttpClient GetClient()
        {
            return _httpClientFactory.CreateClient(HTTP_CLIENT_NAME);
        }

        public async Task<IEnumerable<EmployeeSummary>> GetAll()
        {
            return await GetClient().GetFromJsonAsync<IEnumerable<EmployeeSummary>>("employee");
        }

        public async Task<Employee> GetById(Guid id)
        {
            return await GetClient().GetFromJsonAsync<Employee>($"employee/{id}");

        }
    }
}
