using Sample.Web.Shared;
using System.Collections.Concurrent;

namespace Sample.Web.Server.Data
{
    public interface IEmployeeStore
    {
        ConcurrentBag<Employee> Employees { get; }
    }
}