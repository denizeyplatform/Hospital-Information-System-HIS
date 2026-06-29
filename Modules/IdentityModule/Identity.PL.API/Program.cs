using Identity.Application.Configuration;
using Identity.Infrastructure.Configurations;
using Identity.Infrastructure.Presistance.Data;
using Identity.Infrastructure.Presistance.Seeding;
using Identity.Infrastructure.Services.Identity;
using Identity.PL.API.Configurations;
using Identity.PL.API.Middleware;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DefaultConnection"), builder.Configuration);
builder.Services.AddJWTCongigurations(builder.Configuration);
builder.Services.AddAPI(builder.Configuration);


builder.Services.AddAuthorization();

builder.Services.AddSingleton<IAuthorizationPolicyProvider,PermissionPolicyProvider>();

builder.Services.AddScoped<IAuthorizationHandler,PermissionAuthorizationHandler>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}
using (var scope = app.Services.CreateScope())
{
    var dbContext =
        scope.ServiceProvider
            .GetRequiredService<AppIdentityDBContext>();

    await PermissionSeeder.SeedAsync(dbContext);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ApiKeyMiddleware>();
app.MapControllers();
app.Run();
