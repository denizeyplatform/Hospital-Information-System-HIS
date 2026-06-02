using Ocelot.APIGateway.Endpoints;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
      .SetBasePath(builder.Environment.ContentRootPath)
      .AddOcelot(); // single ocelot.json file in read-only mode
builder.Services
    .AddOcelot(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

//app.loginEndpointFunc();

await app.UseOcelot();

app.Run();
