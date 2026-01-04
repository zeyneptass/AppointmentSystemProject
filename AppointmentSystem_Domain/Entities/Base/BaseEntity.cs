using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Domain.Entities.Base
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
        }

        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; } = false; // Soft Delete için (Case 1)
        // Soft Delete (Yumuşak Silme), bir veriyi veritabanından gerçekten silmek yerine, sadece "silinmiş gibi" işaretleme yöntemidir.
    }
}
