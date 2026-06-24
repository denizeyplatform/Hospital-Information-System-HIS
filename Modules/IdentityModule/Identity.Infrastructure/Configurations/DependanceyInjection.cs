using Identity.Application.Contracts.Interface;
using Identity.Application.Contracts.Interface.Repository;
using Identity.Domain.Interface;
using Identity.Domain.Interface.Service;
using Identity.Infrastructure.Models;
using Identity.Infrastructure.Presistance.Data;
using Identity.Infrastructure.Repository;
using Identity.Infrastructure.Services;
using Identity.Infrastructure.Services.Email;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using System.Text;
using System.Text.Json.Serialization;


namespace Identity.Infrastructure.Configurations
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddJWTCongigurations(this IServiceCollection services, IConfiguration configuration)
        {
            

            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            if (jwtSettings == null)
            {
                throw new Exception("JWT settings are not configured properly.");
            }
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = secretKey
                };
                o.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            message = "You are not authorized to access this resource. Please authenticate."
                        });
                        return context.Response.WriteAsync(result);
                    },
                };
            });


            return services;
        }
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString, IConfiguration configuration)
        {

            services.AddDbContext<AppIdentityDBContext>(options =>
               options.UseSqlServer(connectionString));

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AppIdentityDBContext>()
                .AddDefaultTokenProviders();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AppIdentityDBContext).Assembly));


            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromMinutes(5);
            });

            services.Configure<EmailSettings>(
                configuration.GetSection("EmailSettings"));


            services.AddScoped<IDomainEventDispatcher,DomainEventDispatcher>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IMFARepository, MFARepository>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPasswordRepository, PasswordRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();

            return services;
        }
    }
}
