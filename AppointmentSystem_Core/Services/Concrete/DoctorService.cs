using AppointmentSystem_Core.DTOs.Doctor;
using AppointmentSystem_Core.Services.Abstract;
using AppointmentSystem_Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Core.Services.Concrete
{
    public class DoctorService : IDoctorService
    {
        public Task<IDataResult<Guid>> AddDoctorAsync(AddDoctorByAdminDTO addDoctorByAdminDto)
        {
            throw new NotImplementedException();
        }
    }
}
