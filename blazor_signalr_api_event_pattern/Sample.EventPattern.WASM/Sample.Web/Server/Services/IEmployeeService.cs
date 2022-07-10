using Sample.Web.Server.Services.Utils;
using Sample.Web.Shared;

namespace Sample.Web.Server.Services
{
    public interface IEmployeeService
    {
        Task<DurableHttpResponse> Create(Employee employee);
        Task<Employee> GetById(Guid id); 
        Task<IEnumerable<EmployeeSummary>> GetAll();
    }
}