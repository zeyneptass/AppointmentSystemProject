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
        private readonly IGenericRepository<Department> _departmentRepository;
        public DoctorService(IGenericRepository<Doctor> doctorRepository, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IGenericRepository<Department> departmentRepository)
        {
            _unitOfWork = unitOfWork;
            _doctorRepository = doctorRepository;
            _userManager = userManager;
            _departmentRepository = departmentRepository;
        }

        public async Task<IDataResult<UserDTO>> AddDoctorAsync(AddDoctorByAdminDTO dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                return new ErrorDataResult<UserDTO>("Bu email adresiyle kayıtlı kullanıcı var");

            if (await _userManager.FindByNameAsync(dto.TC) != null)
                return new ErrorDataResult<UserDTO>("Bu TC ile kayıtlı kullanıcı var");

            var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
            if(department == null)
            {
                return new ErrorDataResult<UserDTO>("Deparman sistemde bulunamadı");
            }
                      

            // 3. Identity User Oluşturma
            var newDoctorUser = new ApplicationUser
            {
                UserName = dto.TC,
                TC = dto.TC,
                Email = dto.Email,
                Name = dto.FirstName,
                Surname = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                EmailConfirmed = true // Admin eklediği için onaylı sayalım
            };

            var identityResult = await _userManager.CreateAsync(newDoctorUser, dto.Password);
            if (!identityResult.Succeeded)
            {
                return new ErrorDataResult<UserDTO>(string.Join(", ", identityResult.Errors.Select(e => e.Description)));
            }

            await _userManager.AddToRoleAsync(newDoctorUser, "Doctor");

            // 4. Doctor Tablosuna Kayıt (MANUEL ROLLBACK EKLENDİ)
            try
            {
                var doctor = new Doctor
                {
                    AppUserId = newDoctorUser.Id,
                    DepartmentId = dto.DepartmentId,
                    isActive = true
                };

                await _doctorRepository.AddAsync(doctor);

                await _unitOfWork.SaveChangesAsync();

                // Başarılı Dönüş
                return new SuccessDataResult<UserDTO>(new UserDTO
                {
                    Id = newDoctorUser.Id.ToString(),
                    FirstName = newDoctorUser.Name,
                    LastName = newDoctorUser.Surname,
                    Email = newDoctorUser.Email,
                    TC = newDoctorUser.TC,
                    PhoneNumber = newDoctorUser.PhoneNumber
                }, "Doktor başarıyla eklendi.");
            }
            catch (Exception ex)
            {
                // HATA OLURSA: Identity User'ı sil (Rollback)
                // Böylece "User var ama Doctor yok" durumu engellenir.
                await _userManager.DeleteAsync(newDoctorUser);

                return new ErrorDataResult<UserDTO>($"Doktor profili oluşturulamadı, işlem geri alındı. Hata: {ex.Message}");
            }
        }

    }
}