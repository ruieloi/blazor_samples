using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Sample.Web.Server.Hubs;
using Sample.Web.Server.Services.Utils;
using Sample.Web.Shared;

namespace Sample.Web.Server.Services
{
    public class EmployeeService : IEmployeeService
    {
        public const string HTTP_CLIENT_NAME = "employee_api";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        private readonly IHubContext<EmployeeHub, IEmployeeHub> _hubContext;

        public EmployeeService(IHttpClientFactory httpClientFactory, IMemoryCache cache, IHubContext<EmployeeHub, IEmployeeHub> employeeHub)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
            _hubContext = employeeHub;
        }

        public async Task<Guid> Create(Employee employee)
        {
            var response = await GetClient().PostAsJsonAsync("employee/create", employee);
            if (response.IsSuccessStatusCode)
            {
                //we are not going to use the status Id pool checking
                //but it can be a good feature in case of error handling (ex: function didn't sent the signal, or client didn't get the event

                //var durableResponse = await response.Content.ReadFromJsonAsync<DurableHttpResponse>();
                //if (durableResponse != null && !string.IsNullOrEmpty(durableResponse.id))
                //{
                //    //don't send internal links to the outside
                //    //store the links in API and send only the Id
                //    _cache.Set(durableResponse.id, durableResponse);

                //    return durableResponse.id;
                //}

                return employee.Id;
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

        public async Task<bool> CheckStatus(string statusId)
        {
            //get status info
            if(_cache.TryGetValue(statusId, out DurableHttpResponse durableReponse))
            {
                //check status
                //var result = await GetClient().GetFromJsonAsync<DurableStatusResponse>(durableReponse.StatusQueryGetUri);
                //for some reason the call above doesn't map correctly the field "input"
                var result = JsonConvert.DeserializeObject<DurableStatusResponse>(await GetClient().GetStringAsync(durableReponse.StatusQueryGetUri));
                if (result != null && result.runtimeStatus == "Completed")
                {
                    //TODO return other status in a Enum instead of bool

                    var employeeId = Guid.Parse(result.input.Id.Value);

                    var employee = await GetById(employeeId);

                    await _hubContext.Clients.All.EmployeeAdded(employee.ToSummary());

                    return true;
                }
            }
            return false;
        }
    }
}
