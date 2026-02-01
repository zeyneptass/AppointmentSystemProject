using AppointmentSystem_Core.DataAccess.Abstract;
using AppointmentSystem_Core.DTOs.Auth;
using AppointmentSystem_Core.DTOs.Patient;
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
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public PatientService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task AddPatientAsync(Guid userId, RegisterDTO registerDto)
        {
            var patient = new Patient
            {
                AppUserId = userId,
                TC = registerDto.TC,
                DateOfBirth = registerDto.DateOfBirth
            };
            await _unitOfWork.Patients.AddAsync(patient);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IResult> DeletePatientAsync(Guid id)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(id);
            if (patient == null) return new ErrorResult("Kayıt bulunamadı.");

            patient.IsDeleted = true;
            patient.UpdatedDate = DateTime.Now;

            _unitOfWork.Patients.Update(patient);
            await _unitOfWork.SaveChangesAsync();

            return new SuccessResult("Hasta kaydı silindi.");
        }

        public async Task<IDataResult<IEnumerable<PatientDetailDTO>>> GetAllPatientsAsync()
        {
            var patients = await _unitOfWork.Patients.GetAsync(filter: p => !p.IsDeleted, includes: new Expression<Func<Patient, object>>[] { p => p.ApplicationUser });
            var patientDtos = _mapper.Map<IEnumerable<PatientDetailDTO>>(patients);
            return new SuccessDataResult<IEnumerable<PatientDetailDTO>>(patientDtos);
        }

        public async Task<IDataResult<PatientDetailDTO>> GetPatientByIdAsync(Guid id)
        {
            var patients = await _unitOfWork.Patients.GetAsync(
                filter: p => p.Id == id && !p.IsDeleted,
                includes: new Expression<Func<Patient, object>>[] { p => p.ApplicationUser }
                );
            var patient = patients.FirstOrDefault();
            if (patient == null)
                return new ErrorDataResult<PatientDetailDTO>("Hasta bulunamadı");
            return new SuccessDataResult<PatientDetailDTO>(_mapper.Map<PatientDetailDTO>(patient));
        }

        public async Task<IResult> UpdatePatientAsync(PatientUpdateDTO dto)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(dto.Id, asNoTracking:false);
            if(patient == null || patient.IsDeleted)
                return new ErrorResult("Hasta bulunamadı");
            var user = await _userManager.FindByIdAsync(patient.AppUserId.ToString());
            user.Name = dto.FirstName;
            user.Surname = dto.LastName;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;

            await _userManager.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return new SuccessResult("Hasta bilgileri güncellendi");
        }
    }
}
