using Microsoft.AspNetCore.SignalR;

namespace Sample.Web.Server.Hubs
{
    public class EmployeeHub : Hub<IEmployeeHub>
    {
        // These 2 methods will be called from the client
        public async Task JoinEmployeeGroup(Guid employeeId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, employeeId.ToString());
        }
        public async Task LeaveEmployeeGroup(Guid employeeId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, employeeId.ToString());
        }
    }
}
