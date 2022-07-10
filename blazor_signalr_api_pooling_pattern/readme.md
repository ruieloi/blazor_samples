# Smaple.PoolingPattern.WASM

A code example to show how to use a Blazor WASM with a backend API using Pooling Pattern


## How to setup in your machine

* pull code
* 

## How to create the project

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

* https://docs.microsoft.com/en-us/aspnet/core/blazor/tutorials/signalr-blazor?view=aspnetcore-6.0&tabs=visual-studio&pivots=server



## Links

* https://github.com/DaniJG/blazor-surveys - one good example of Blazor SignalR usage
* https://github.com/bchavez/Bogus - great tool to create dummy data
* https://visualstudiomagazine.com/articles/2021/01/06/blazor-lists.aspx
* https://www.rahulpnath.com/blog/dependency-injection-in-azure-functions/
* https://www.c-sharpcorner.com/article/routing-in-azure-function/

