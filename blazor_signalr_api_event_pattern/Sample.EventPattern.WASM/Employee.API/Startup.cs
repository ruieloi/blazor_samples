using Employee.API.Data;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Employee.API.Startup))]
namespace Employee.API
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IEmployeeStore, EmployeeStore>();
        }
    }
}
