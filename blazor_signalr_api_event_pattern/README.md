# Sample.EventPattern.WASM

A code example to show how to use a Blazor WASM with a backend API and Azure function using Event Pattern


# How it works

*TODO Add diagrams

In this sample we have 4 major components:
* A Blazor Web app (WASM) - UI application
* A .NET Core Api - Backend of the UI (we could directly have the backend to be the Azure function but that is not recommended)
* SignalR - responsable for the communication between UI and Backend
* Azure Function App

We have functional logic
* A list of Employees
* It's possible to add a new employee

###Flows###

**TODO**



## How to setup in your machine

**TODO**
* pull code
* Change project configuration to start the "Employee.API" and then "Server" project

## How to create the project

**TODO**

```
"Azure": {
  "SignalR": {
    "Enabled": true,
    "ConnectionString": <your-connection-string>       
  }
}

´´´

* Create a new project in Visual Studio
	* Choose Blazor WebAssembly template
	* Choose AspNet Core API Host

* Add SignalR
	* Add package Microsoft.AspNetCore.SignalR.Client in Client project
		`dotnet add package Microsoft.AspNetCore.SignalR.Client`
	* Add package Microsoft.Extensions.Http in Client project	
	* Add package Microsoft.AspNetCore.ResponseCompression in Server project
	* Create folder "Hub" in the Server project and create Hub class
	* In Program.cs in Server project
		Add before var app = builder.Build();
		```builder.Services.AddResponseCompression(opts =>
			{
				opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
					new[] { "application/octet-stream" });
			});```
		Add before app.MapFallbackToFile("index.html");
		`app.MapHub<EmployeeHub>("/employeeHub"); `
		

## Documentation

**TODO**




## Links
* https://github.com/Azure/azure-signalr/blob/dev/docs/emulator.md - signalr emulator
* https://docs.microsoft.com/en-us/aspnet/core/blazor/tutorials/signalr-blazor?view=aspnetcore-6.0&tabs=visual-studio&pivots=server
* https://github.com/DaniJG/blazor-surveys - one good example of Blazor SignalR usage
* https://github.com/bchavez/Bogus - great tool to create dummy data
* https://visualstudiomagazine.com/articles/2021/01/06/blazor-lists.aspx - use virtualize component in blazor for lists
* https://www.rahulpnath.com/blog/dependency-injection-in-azure-functions/ - how to setup azure function DI
* https://www.c-sharpcorner.com/article/routing-in-azure-function/ - how to configure routing in azure functions

