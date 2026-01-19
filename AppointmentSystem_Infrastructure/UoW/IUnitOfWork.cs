using AppointmentSystem_Core.DataAccess.Abstract;
using AppointmentSystem_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Infrastructure.UoW
{
    public interface IUnitOfWork
    {
        IGenericRepository<Department> Departments { get; }
        IGenericRepository<Doctor> Doctors { get; }
        IGenericRepository<Patient> Patients { get; }
        IGenericRepository<Appointment> Appointments { get; }
        Task<int> SaveChangesAsync();
    }
}
