using AppointmentSystem_Core.DataAccess.Abstract;
using AppointmentSystem_Infrastructure.Persistence.Context;
using AppointmentSystem_Infrastructure.Repository;
using AppointmentSystem_Infrastructure.UoW;
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

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));

            return services;

        }
    }
}
