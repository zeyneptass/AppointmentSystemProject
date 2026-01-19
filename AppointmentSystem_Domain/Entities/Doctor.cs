using AppointmentSystem_Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Domain.Entities
{
    public class Doctor : BaseEntity
    {
        public Guid DepartmentId { get; set; }
        public bool isActive { get; set; } = true;
        // Navigation Properties
        public Department Department { get; set; }
        // one to many
        public ICollection<Appointment> Appointments { get; set; }
        // Identity User Id ile ilişki
        public Guid AppUserId { get; set; }
    }
}
