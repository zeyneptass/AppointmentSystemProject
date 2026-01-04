using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Domain.Entities
{
    public class Department : Base.BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        // Navigation Properties
        // one to many
        public ICollection<Doctor> Doctors { get; set; }
    }
}
