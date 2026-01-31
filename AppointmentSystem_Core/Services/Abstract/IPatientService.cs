using AppointmentSystem_Core.DTOs.Auth;
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
    }
}
