using Microsoft.AspNetCore.Mvc;
using Sample.Web.Shared;

namespace Sample.Web.Server.Services
{
    public interface IEmployeeService
    {
        Task<string> Create(Employee employee);
        Task<Employee> GetById(Guid id); 
        Task<IEnumerable<EmployeeSummary>> GetAll();
        Task<bool> CheckStatus(string statusId);
    }
}