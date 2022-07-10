using Sample.Web.Shared;
using System.Collections.Concurrent;

namespace Employee.API.Data
{
    public interface IEmployeeStore
    {
        ConcurrentBag<Sample.Web.Shared.Employee> Employees { get; }
    }
}