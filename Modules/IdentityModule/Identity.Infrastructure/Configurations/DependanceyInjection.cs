using Identity.Application.Contracts.Interface.Repository;
using Identity.Domain.Interface.Service;
using Identity.Infrastructure.Models;
using Identity.Infrastructure.Presistance.Data;
using Identity.Infrastructure.Repository;
using Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using System.Text;


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
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {

            services.AddDbContext<AppIdentityDBContext>(options =>
               options.UseSqlServer(connectionString));

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AppIdentityDBContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IAuthRepository, AuthRepository>();

            return services;
        }
    }
}
