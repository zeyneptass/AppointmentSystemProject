using AppointmentSystem_Core.DTOs.Appointment;
using AppointmentSystem_Core.DTOs.Department;
using AppointmentSystem_Core.DTOs.Doctor;
using AppointmentSystem_Core.DTOs.Patient;
using AppointmentSystem_Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Core.Mapper
{
    // DTO'ları veritbanaın kaydetme işlemi yaparken entity'lere dönüştürmek ve entity'leri DTO'lara dönüştürmek için kullanılır.
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<AppointmentDTO, Appointment>().ReverseMap();
            CreateMap<DoctorDTO, Doctor>().ReverseMap();
            CreateMap<DepartmentDTO, Department>().ReverseMap();
            CreateMap<PatientDTO, Patient>().ReverseMap();
        }        
    }
}
