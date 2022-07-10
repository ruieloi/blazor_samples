using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Sample.Web.Client;
using Sample.Web.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var baseAddress = new Uri(builder.HostEnvironment.BaseAddress);
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = baseAddress });

// Configure a typed HttpClient with all the survey API endpoints
// so the razor pages/components dont need to use the raw HttpClient
builder.Services.AddHttpClient<EmployeeHttpClient>(client => client.BaseAddress = baseAddress);

// Register a preconfigure SignalR hub connection.
builder.Services.AddSingleton<HubConnection>(sp => {
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    return new HubConnectionBuilder()
      .WithUrl(navigationManager.ToAbsoluteUri("/employeehub"))
      .WithAutomaticReconnect()
      .Build();
});

await builder.Build().RunAsync();
