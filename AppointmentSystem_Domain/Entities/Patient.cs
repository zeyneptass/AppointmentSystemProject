using AppointmentSystem_Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Domain.Entities
{
    public class Patient : BaseEntity
    {
        public string TC { get; set; }
        public DateTime DateOfBirth { get; set; }

        // Navigation Properties
        // one to many 
        public ICollection<Appointment> Appointments { get; set; }
        // Identity User Id ile ilişki
        public Guid AppUserId { get; set; }

    }
}
