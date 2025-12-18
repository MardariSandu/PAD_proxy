using SyncNode;

var builder = WebApplication.CreateBuilder(args);

// Manually attach Startup.cs
var startup = new Startup(builder.Configuration);

// Call ConfigureServices()
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Call Configure()
startup.Configure(app, app.Environment);

app.Run();
