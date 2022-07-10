using Sample.Web.Shared;

namespace Sample.Web.Server.Hubs
{
    public interface IEmployeeHub
    {
        Task EmployeeAdded(EmployeeSummary survey);
        Task EmployeeUpdated(Employee survey);
        Task TestNotify(string text);
    }
}
