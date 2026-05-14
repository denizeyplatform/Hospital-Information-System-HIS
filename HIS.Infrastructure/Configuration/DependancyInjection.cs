using HIS.Domain.Interfaces.Repository;
using HIS.Infrastructure.Presestance.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Infrastructure.Configuration
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IPatientRepository, PatientRepository>();
            return services;
        }
    }
}
