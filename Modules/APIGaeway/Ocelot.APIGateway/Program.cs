using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
      .SetBasePath(builder.Environment.ContentRootPath)
      .AddOcelot(); // single ocelot.json file in read-only mode

builder.Services
       .AddOcelot(builder.Configuration)
       .AddPolly();

builder.Configuration
       .AddJsonFile("ocelot.json",
                    optional: false,
                    reloadOnChange: true);

//builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerForOcelot(builder.Configuration);


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseSwagger();

app.UseSwaggerUI();


app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
}).UseOcelot().Wait();

await app.UseOcelot();

app.Run();
