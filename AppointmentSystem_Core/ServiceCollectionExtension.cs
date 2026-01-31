using AppointmentSystem_Core.DataAccess.Abstract;
using AppointmentSystem_Core.DTOs.Auth;
using AppointmentSystem_Core.Mapper;
using AppointmentSystem_Core.Services.Abstract;
using AppointmentSystem_Core.Services.Concrete;
using AppointmentSystem_Core.Validators;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentSystem_Domain.Entities;

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

            services.AddScoped<IAuthService, AuthService>();

            #endregion



            #region FluentValidation

            services.AddValidatorsFromAssembly(typeof(LoginDTOValidator).Assembly);
            services.AddValidatorsFromAssembly(typeof(RegisterDTOValidator).Assembly);

            #endregion

            return services;
        }
    }
}
