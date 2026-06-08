using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
       .AddJsonFile("ocelot.json",
                    optional: false,
                    reloadOnChange: true);
builder.Services
       .AddOcelot(builder.Configuration)
       .AddPolly();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services
    .AddSwaggerForOcelot(builder.Configuration);

var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

app.UseSwagger();

app.UseSwaggerUI();


app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
});

await app.UseOcelot();

app.Run();
