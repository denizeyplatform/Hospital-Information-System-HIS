using HIS.API.Configuration;
using HIS.API.Endpoints.Patient;
using HIS.Application.Configuration;
using HIS.Infrastructure.Configuration;
using HIS.Infrastructure.Presestance;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApi();
builder.Services.AddApplication();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddInfrastructure(connectionString);



builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();


//app.MapPatientEndpoints();

//await CreatePatientEndPoint.MapEndPoint(app);
//await GetPatientByIdEndPoint.MapEndPoint(app);
//await UpdatePatientEndPoint.MapEndPoint(app);

app.MapControllers();

app.Run();
