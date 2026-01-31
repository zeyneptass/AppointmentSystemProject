using AppointmentSystem_Core.DataAccess.Abstract;
using AppointmentSystem_Core.DTOs.Auth;
using AppointmentSystem_Core.DTOs.Doctor;
using AppointmentSystem_Core.Services.Abstract;
using AppointmentSystem_Core.Utilities.Results.Abstract;
using AppointmentSystem_Core.Utilities.Results.Concrete;
using AppointmentSystem_Domain.Entities;
using AppointmentSystem_Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Core.Services.Concrete
{
    public class DoctorService : IDoctorService
    {
        private readonly IGenericRepository<Doctor> _doctorRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public DoctorService(IGenericRepository<Doctor> doctorRepository, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _doctorRepository = doctorRepository;
            _userManager = userManager;
        }

        public async Task<IDataResult<UserDTO>> AddDoctorAsync(AddDoctorByAdminDTO addDoctorByAdminDto)
        {
            try
            {
                var emaiCheck = await _userManager.FindByEmailAsync(addDoctorByAdminDto.Email);
                if (emaiCheck != null)
                {
                    return new ErrorDataResult<UserDTO>("Bu email adresiyle kayıtlı kullanıcı var");
                }

                var tcCheck = await _userManager.FindByNameAsync(addDoctorByAdminDto.TC);
                if (tcCheck != null)
                {
                    return new ErrorDataResult<UserDTO>("Bu TC Kimlik numarası ile kayıtlı kullanıcı zaten var");
                }

                var newDoctor = new ApplicationUser
                {
                    UserName = addDoctorByAdminDto.TC,
                    TC = addDoctorByAdminDto.TC,
                    Email = addDoctorByAdminDto.Email,
                    Name = addDoctorByAdminDto.FirstName,
                    Surname = addDoctorByAdminDto.LastName,
                    PhoneNumber = addDoctorByAdminDto.PhoneNumber
                };

                var result = await _userManager.CreateAsync(newDoctor, addDoctorByAdminDto.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return new ErrorDataResult<UserDTO>(errors);
                }

                await _userManager.AddToRoleAsync(newDoctor, "Doctor");

                var doctor = new Doctor
                {
                    AppUserId = newDoctor.Id,
                    DepartmentId = addDoctorByAdminDto.DepartmentId
                };

                await _doctorRepository.AddAsync(doctor);
                await _unitOfWork.SaveChangesAsync();

                //  UserDTO manuel mapping
                var userDTO = new UserDTO
                {
                    Id = newDoctor.Id.ToString(),
                    FirstName = newDoctor.Name,
                    LastName = newDoctor.Surname,
                    Email = newDoctor.Email,
                    TC = newDoctor.TC,
                    PhoneNumber = newDoctor.PhoneNumber,
                    Token = "", // Token yok
                    Expiration = DateTime.Now
                };

                return new SuccessDataResult<UserDTO>(userDTO, "Doktor başarılı bir şekilde eklendi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<UserDTO>($"Doktor eklenirken hata oluştu: {ex.Message}");
            }
        }
    }
}
