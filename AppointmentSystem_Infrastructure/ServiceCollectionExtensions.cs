using AppointmentSystem_Core.DataAccess.Abstract;
using AppointmentSystem_Core.Services.Abstract;
using AppointmentSystem_Domain.Entities.Identity;
using AppointmentSystem_Infrastructure.Persistence.Context;
using AppointmentSystem_Infrastructure.Repository;
using AppointmentSystem_Infrastructure.Serialization;
using AppointmentSystem_Infrastructure.Services.TokenExtensions;
using AppointmentSystem_Infrastructure.UoW;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
        {

            #region DB Confguration
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppointmentDbContext>(options =>
               options.UseSqlServer(connectionString,
               b => b.MigrationsAssembly(typeof(AppointmentDbContext).Assembly.FullName)));
            #endregion

            #region Identity Configuration
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<AppointmentDbContext>()
            .AddDefaultTokenProviders();
            #endregion

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));

            services.AddScoped<ITokenService, TokenService>();

            #region DateConverter
            services.Configure<JsonOptions>(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
            });
            #endregion

            return services;

        }
    }
}
