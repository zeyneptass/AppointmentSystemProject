using AppointmentSystem_Core.DTOs.Auth;
using AppointmentSystem_Core.Services.Abstract;
using AppointmentSystem_Core.Utilities.Results.Abstract;
using AppointmentSystem_Core.Utilities.Results.Concrete;
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

        public async Task<IDataResult<UserDTO>> RegisterAsync(RegisterDTO registerDto)
        {
            var emaiCheck = await _userManager.FindByEmailAsync(registerDto.Email);
            if (emaiCheck != null)
            {
                return new ErrorDataResult<UserDTO>("Bu email adresiyle kayıtlı kullanıcı var");
            }

            var tcCheck = await _userManager.FindByNameAsync(registerDto.TC);
            if (tcCheck != null)
            {
                return new ErrorDataResult<UserDTO>("Bu TC Kimlik numarası ile kayıtlı kullanıcı zaten var");
            }

            var newUser = new ApplicationUser // manual mapping
            {
                UserName = registerDto.TC,  // Kullanıcı adı TC kimlik numarası
                TC = registerDto.TC,
                Email = registerDto.Email,
                Name = registerDto.FirstName,
                Surname = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
                DateOfBirth = registerDto.DateOfBirth
            };
            var result = await _userManager.CreateAsync(newUser, registerDto.Password); // Identity veilen şifreyi hashleyip saklar passwordhash olarak

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ErrorDataResult<UserDTO>(errors);
            }
            // kullnıcı kayıt olunca patient olarak rol atanır
            await _userManager.AddToRoleAsync(newUser, "Patient");

            // otomatik token oluşturma
            var roles = await _userManager.GetRolesAsync(newUser);
            var token = _tokenService.CreateToken(newUser, roles);

            var userDTO = new UserDTO
            {
                Id = newUser.Id.ToString(),
                FirstName = newUser.Name,
                LastName = newUser.Surname,
                Email = newUser.Email,
                TC = newUser.TC,
                PhoneNumber = newUser.PhoneNumber,
                Token = token,
                Expiration = DateTime.Now.AddHours(3) // tokenın geçerlilik süresi 3 saat
            };
            return new SuccessDataResult<UserDTO>(userDTO, "Başarılı bir şekilde kayıt oldunuz.");
        }
    }
}
