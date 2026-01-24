using AppointmentSystem_Core.DTOs.Doctor;
using AppointmentSystem_Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Core.Services.Abstract
{
    public interface IDoctorService
    {
        Task<IDataResult<Guid>> AddDoctorAsync(AddDoctorByAdminDTO addDoctorByAdminDto);
    }
}
