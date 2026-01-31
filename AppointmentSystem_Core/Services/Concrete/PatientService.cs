using AppointmentSystem_Core.DataAccess.Abstract;
using AppointmentSystem_Core.DTOs.Auth;
using AppointmentSystem_Core.Services.Abstract;
using AppointmentSystem_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Core.Services.Concrete
{
    public class PatientService : IPatientService
    {
        private readonly IGenericRepository<Patient> _patientRepository;
        private readonly IUnitOfWork _unitOfWork;
        public PatientService(IGenericRepository<Patient> patientRepository,IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
            _patientRepository = patientRepository;
        }
        public async Task AddPatientAsync(Guid userId, RegisterDTO registerDto)
        {
            var patient = new Patient
            {
                AppUserId = userId,
                TC = registerDto.TC,
                DateOfBirth = registerDto.DateOfBirth
            };
            await _patientRepository.AddAsync(patient);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
