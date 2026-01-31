using AppointmentSystem_Core.DataAccess.Abstract;
using AppointmentSystem_Core.DTOs.Auth;
using AppointmentSystem_Core.DTOs.Department;
using AppointmentSystem_Core.DTOs.Doctor;
using AppointmentSystem_Core.Services.Abstract;
using AppointmentSystem_Core.Utilities.Results.Abstract;
using AppointmentSystem_Core.Utilities.Results.Concrete;
using AppointmentSystem_Domain.Entities;
using AppointmentSystem_Domain.Entities.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Core.Services.Concrete
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public DoctorService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IDataResult<UserDTO>> AddDoctorAsync(AddDoctorByAdminDTO dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                return new ErrorDataResult<UserDTO>("Bu email adresiyle kayıtlı kullanıcı var");

            if (await _userManager.FindByNameAsync(dto.TC) != null)
                return new ErrorDataResult<UserDTO>("Bu TC ile kayıtlı kullanıcı var");

            var department = await _unitOfWork.Departments.GetByIdAsync(dto.DepartmentId);
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
                EmailConfirmed = true 
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

                await _unitOfWork.Doctors.AddAsync(doctor);

                await _unitOfWork.SaveChangesAsync();
                var userDto = _mapper.Map<UserDTO>(newDoctorUser);
                // Başarılı Dönüş
                return new SuccessDataResult<UserDTO>(userDto, "Doktor başarıyla eklendi.");
            }
            catch (Exception ex)
            {
                // HATA OLURSA: Identity User'ı sil (Rollback)
                // Böylece "User var ama Doctor yok" durumu engellenir.
                await _userManager.DeleteAsync(newDoctorUser);

                return new ErrorDataResult<UserDTO>($"Doktor profili oluşturulamadı, işlem geri alındı. Hata: {ex.Message}");
            }
        }

        public async Task<IResult> DeleteDoctorAsync(Guid doctorId)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(doctorId);
            if (doctor == null)
            {
                return new ErrorResult("Doktor bulunamadı");
            }
            doctor.isActive = false;
            doctor.IsDeleted = true;
            doctor.UpdatedDate = DateTime.Now;
            _unitOfWork.Doctors.Update(doctor);
            await _unitOfWork.SaveChangesAsync();
            return new SuccessResult("Doktor kaydı başarılı bir şekilde silindi");
        }

        public async Task<IDataResult<IEnumerable<DoctorDetailDTO>>> GetAllDoctorsAsync()
        {
            //include ile db'de join işlemi yapmış oluruz N+1 problemine karşı çözüm
            var doctors = await _unitOfWork.Doctors.GetAsync(
                filter: d => !d.IsDeleted,  // silinmeyen doctorları getirek için filter
                includes: new Expression<Func<Doctor, object>>[]{d => d.Department,d => d.ApplicationUser});
            var dtos = _mapper.Map<IEnumerable<DoctorDetailDTO>>(doctors);
            return new SuccessDataResult<IEnumerable<DoctorDetailDTO>>(dtos, "Doktorlar listelendi");
        }

        public async Task<IDataResult<DoctorDetailDTO>> GetDoctorByIdAsync(Guid doctorId)
        {
            var doctors = await _unitOfWork.Doctors.GetAsync(
                filter: d => d.Id == doctorId && !d.IsDeleted,
                includes: new Expression<Func<Doctor, object>>[] { d => d.Department, d => d.ApplicationUser });

            var doctor = doctors.FirstOrDefault();
            if (doctor == null)
            {
                return new ErrorDataResult<DoctorDetailDTO>("Doktor bulunamadı");
            }
            var dto = _mapper.Map<DoctorDetailDTO>(doctor);
            return new SuccessDataResult<DoctorDetailDTO>(dto);
        }

        public async Task<IResult> UpdateDoctorAsync(UpdateDoctorDTO updateDoctorDTO)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(updateDoctorDTO.Id, asNoTracking: false);
            if (doctor == null) return new ErrorResult("Doktor bulunamadı.");

            var user = await _userManager.FindByIdAsync(doctor.AppUserId.ToString());
            if (user == null) return new ErrorResult("Doktorun kullanıcı bilgileri bulunamadı.");
            // Kullanıcı bilgilerini güncelle
            user.Name = updateDoctorDTO.FirstName;
            user.Surname = updateDoctorDTO.LastName;
            user.PhoneNumber = updateDoctorDTO.PhoneNumber;
            user.Email = updateDoctorDTO.Email;
            user.UserName = updateDoctorDTO.Email;

            var identityResult = await _userManager.UpdateAsync(user);
            if (!identityResult.Succeeded)
            {
                return new ErrorResult("Kullanıcı bilgileri güncellenirken hata oluştu: "+string.Join(", ", identityResult.Errors.Select(e => e.Description)));
            }
            // Doctor entity'sini güncelle
            doctor.DepartmentId = updateDoctorDTO.DepartmentId;
            doctor.UpdatedDate = DateTime.Now;
            _unitOfWork.Doctors.Update(doctor);
            await _unitOfWork.SaveChangesAsync();

            return new SuccessResult("Doktor bilgileri başarıyla güncellendi.");
        }
    }
}