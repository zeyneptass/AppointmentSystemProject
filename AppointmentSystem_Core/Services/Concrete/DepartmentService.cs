using AppointmentSystem_Core.DataAccess.Abstract;
using AppointmentSystem_Core.DTOs.Department;
using AppointmentSystem_Core.Services.Abstract;
using AppointmentSystem_Core.Utilities.Results.Abstract;
using AppointmentSystem_Core.Utilities.Results.Concrete;
using AppointmentSystem_Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Core.Services.Concrete
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IDataResult<Guid>> AddDepartmentAsync(CreateDepartmentDTO createDepartmentDTO)
        {
            bool isExist = await _unitOfWork.Departments.ExistsAsync(e => e.Name.ToLower() == createDepartmentDTO.Name.ToLower() && !e.IsDeleted);
            if (isExist)
            {
                return new ErrorDataResult<Guid>($"{createDepartmentDTO.Name} bölümü zaten mevcut");
            }

            // DTO'yu Entity'ye dönüştür
            var department = _mapper.Map<Department>(createDepartmentDTO);
            await _unitOfWork.Departments.AddAsync(department);
            await _unitOfWork.SaveChangesAsync();

            return new SuccessDataResult<Guid>(department.Id, "Bölüm başrılı bir şekilde eklendi");

        }
        public async Task<IResult> DeleteDepartmentAsync(Guid id)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id, asNoTracking:false);
            if(department == null) return new ErrorResult("Aradığınız bölüm bulunamadı");

            department.IsDeleted = true;
            department.UpdatedDate = DateTime.Now;
            _unitOfWork.Departments.Update(department);

            // bölümdeki doktoralrı bul pasif hale getir
            var doctors = await _unitOfWork.Doctors.GetAsync(d => d.DepartmentId == id, asNoTracking: false);
            foreach (var doctor in doctors)
            {
                doctor.isActive = false;
                doctor.UpdatedDate = DateTime.Now;
                _unitOfWork.Doctors.Update(doctor);
            }
            await _unitOfWork.SaveChangesAsync();
            return new SuccessResult("Bölüm başarıyla silindi");    
        }

        public async Task<IDataResult<IEnumerable<DepartmentDTO>>> GetAllDepartmentAsync()
        {
            // isdelted filtesi ile soft deleted olanları getirme
            var departments = await _unitOfWork.Departments.GetAsync(d => !d.IsDeleted);
            // DTO'ya dönüştür
            var dtos = _mapper.Map<IEnumerable<DepartmentDTO>>(departments);
            return new SuccessDataResult<IEnumerable<DepartmentDTO>>(dtos);
        }

        public async Task<IResult> GetDepartmentByIdAsync(Guid id)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if(department == null || department.IsDeleted)
            {
                return new ErrorResult("Aradığınız bölüm bulunamadı");
            }
            var dto = _mapper.Map<DepartmentDTO>(department);
            return new SuccessDataResult<DepartmentDTO>(dto);
        }

        public async Task<IResult> UpdateDepartmentAsync(UpdateDepartmentDTO updateDepartmentDTO)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(updateDepartmentDTO.Id);
            if (department == null) return new ErrorResult("Aradığınız bölüm bulunamadı");

            bool isExist = await _unitOfWork.Departments.ExistsAsync(e => e.Name.ToLower() == updateDepartmentDTO.Name.ToLower() && e.Id != updateDepartmentDTO.Id && !e.IsDeleted);
            if (isExist)
            {
                return new ErrorResult($"{updateDepartmentDTO.Name} bölümü zaten mevcut");
            }

            _mapper.Map(updateDepartmentDTO, department);
            department.UpdatedDate = DateTime.Now;
            _unitOfWork.Departments.Update(department);
            await _unitOfWork.SaveChangesAsync();
            return new SuccessResult("Bölüm başarıyla güncellendi");
        }
    }
}

//Repository'nde asNoTracking = true varsayılan olarak atanmış. Bu, listeleme işlemleri için (Read-only) performansı artırır, harikadır. Ancak bir nesneyi çekip üzerinde değişiklik yapacaksan (Delete veya Update metodlarındaki gibi), asNoTracking: false parametresini geçmen EF Core'un nesneyi takip etmesini sağlar ve daha sağlıklı olur.