using AppointmentSystem_Core.DTOs.Auth;
using AppointmentSystem_Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Core.Services.Abstract
{
    public interface IAuthService
    {
        Task<IDataResult<UserDTO>> LoginAsync(LoginDTO loginDto);
        Task<IDataResult<UserDTO>> RegisterAsync(RegisterDTO registerDto);
    }
}
