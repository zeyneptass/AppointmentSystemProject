using AppointmentSystem_Core.Mapper;
using AppointmentSystem_Core.Services.Abstract;
using AppointmentSystem_Core.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Core
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            #region DI AutoMapper Configuration
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            #endregion

            #region DI Mappings
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IAppointmentService, AppointmentService>();



            #endregion



            return services;
        }
    }
}
