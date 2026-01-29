using AppointmentSystem_Core.DTOs.Auth;
using AppointmentSystem_Core.Services.Abstract;
using AppointmentSystem_Core.Utilities.Results.Abstract;
using AppointmentSystem_Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Core.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
        public Task<IDataResult<UserDTO>> LoginAsync(LoginDTO loginDto)
        {
            throw new NotImplementedException();

        }

        public Task<IDataResult<UserDTO>> RegisterAsync(RegisterDTO registerDto)
        {
            throw new NotImplementedException();
        }
    }
}
