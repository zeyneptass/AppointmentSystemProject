using AppointmentSystem_Core.DTOs.Appointment;
using AppointmentSystem_Core.DTOs.Auth;
using AppointmentSystem_Core.DTOs.Department;
using AppointmentSystem_Core.DTOs.Doctor;
using AppointmentSystem_Core.DTOs.Patient;
using AppointmentSystem_Domain.Entities;
using AppointmentSystem_Domain.Entities.Identity;
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
            CreateMap<DoctorDetailDTO, Doctor>().ReverseMap();
            CreateMap<DepartmentDTO, Department>().ReverseMap();

            // department
            CreateMap<CreateDepartmentDTO, Department>().ReverseMap();
            CreateMap<UpdateDepartmentDTO, Department>().ReverseMap();

            CreateMap<ApplicationUser, UserDTO>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Surname))
            .ReverseMap();

            // _mapper.Map satırı çalıştığında, AutoMapper DoctorDetailDTO içindeki FirstName alanını nereden dolduracağını bilemez. Çünkü Doctor tablosunda FirstName yok, o ApplicationUser tablosunda. Bu yüzden ForMember ile bu alanların nasıl doldurulacağını belirtmemiz gerekir.


            CreateMap<Doctor, DoctorDetailDTO>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.ApplicationUser.Name))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.ApplicationUser.Surname))
            .ForMember(dest => dest.TC, opt => opt.MapFrom(src => src.ApplicationUser.TC))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.ApplicationUser.PhoneNumber))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));

            CreateMap<UpdateDepartmentDTO,Doctor>().ReverseMap();

            CreateMap<PatientUpdateDTO, Patient>().ReverseMap();
            CreateMap<Patient, PatientDetailDTO>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.ApplicationUser.Name))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.ApplicationUser.Surname))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.ApplicationUser.PhoneNumber))
            .ForMember(dest => dest.TC, opt => opt.MapFrom(src => src.ApplicationUser.TC)) 
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }        
    }
}
