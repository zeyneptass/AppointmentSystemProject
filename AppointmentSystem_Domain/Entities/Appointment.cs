using AppointmentSystem_Domain.Entities.Base;
using AppointmentSystem_Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Domain.Entities
{
    public class Appointment : BaseEntity
    {       
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;  // default beklemede
        public DateTime AppointmentDate { get; set; }

        // Navigation Properties
        // many to one
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
    }
}
