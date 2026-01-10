using AppointmentSystem_Domain.Abstract;
using AppointmentSystem_Domain.EF;
using AppointmentSystem_Domain.Entities;
using AppointmentSystem_Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Infrastructure.UoW
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly AppointmentDbContext _context;

        public UnitOfWork(AppointmentDbContext context)
        {
            _context = context;
            Departments = new GenericRepository<Department>(_context, _context.Departments);
            Doctors = new GenericRepository<Doctor>(_context, _context.Doctors);
            Patients = new GenericRepository<Patient>(_context, _context.Patients);
            Appointments = new GenericRepository<Appointment>(_context, _context.Appointments);
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
