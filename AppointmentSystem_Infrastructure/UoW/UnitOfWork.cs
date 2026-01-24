using AppointmentSystem_Core.DataAccess.Abstract;
using AppointmentSystem_Domain.Entities;
using AppointmentSystem_Infrastructure.Persistence.Context;
using AppointmentSystem_Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AppointmentSystem_Infrastructure.UoW.IUnitOfWork;

namespace AppointmentSystem_Infrastructure.UoW
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly AppointmentDbContext _context;

        public UnitOfWork(AppointmentDbContext context)
        {
            _context = context;
            Departments = new GenericRepository<Department>(_context);
            Doctors = new GenericRepository<Doctor>(_context);
            Patients = new GenericRepository<Patient>(_context);
            Appointments = new GenericRepository<Appointment>(_context);
        }
        public IGenericRepository<Department> Departments { get; }

        public IGenericRepository<Doctor> Doctors { get; }

        public IGenericRepository<Patient> Patients { get; }

        public IGenericRepository<Appointment> Appointments { get; }


        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
