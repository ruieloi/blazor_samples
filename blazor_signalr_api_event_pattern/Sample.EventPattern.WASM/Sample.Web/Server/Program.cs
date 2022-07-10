using Microsoft.AspNetCore.ResponseCompression;
using Sample.Web.Server.Hubs;
using Sample.Web.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMemoryCache();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR(options => { options.EnableDetailedErrors = true; }).AddAzureSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddHttpClient(EmployeeService.HTTP_CLIENT_NAME, c =>
{
    c.BaseAddress = new Uri("http://localhost:7138/api/");
    c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
    //add authorization etc
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapHub<EmployeeHub>("/employeeHub");
app.MapFallbackToFile("index.html");

app.Run();
