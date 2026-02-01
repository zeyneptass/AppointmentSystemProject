using AppointmentSystem_Core.DTOs.Auth;
using AppointmentSystem_Core.DTOs.Patient;
using AppointmentSystem_Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Core.Services.Abstract
{
    public interface IPatientService
    {
        Task AddPatientAsync(Guid userId, RegisterDTO registerDto);

        Task<IDataResult<IEnumerable<PatientDetailDTO>>> GetAllPatientsAsync();
        Task<IDataResult<PatientDetailDTO>> GetPatientByIdAsync(Guid id);
        Task<IResult> UpdatePatientAsync(PatientUpdateDTO dto);
        Task<IResult> DeletePatientAsync(Guid id);

    }
}
