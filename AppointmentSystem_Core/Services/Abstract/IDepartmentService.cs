using AppointmentSystem_Core.DTOs.Department;
using AppointmentSystem_Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Core.Services.Abstract
{
    public interface IDepartmentService
    {
        Task<IDataResult<Guid>> AddDepartmentAsync(CreateDepartmentDTO createDepartmentDTO);
        Task<IResult> UpdateDepartmentAsync(UpdateDepartmentDTO updateDepartmentDTO);
        Task<IResult> DeleteDepartmentAsync(Guid id); // bölüm silindiğinde bölüm bağlı doktorlar pasif halle getirilecek (soft delete)
        Task<IDataResult<IEnumerable<DepartmentDTO>>> GetAllDepartmentAsync();
        Task<IResult> GetDepartmentByIdAsync(Guid id);
    }
}
