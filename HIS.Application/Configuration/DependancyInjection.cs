using HIS.Application.Interface;
using HIS.Application.Service;
using HIS.Domain.Interfaces.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Configuration
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IPatientService, PatientService>();
            return services;
        }
    }
}
