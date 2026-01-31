using AppointmentSystem_Core.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Core.DTOs.Doctor
{
    public class DoctorDetailDTO : UserDTO
    {
        public string DepartmentName { get; set; }
        public Guid DepartmentId { get; set; }
        public bool IsActive { get; set; }

    }
}
