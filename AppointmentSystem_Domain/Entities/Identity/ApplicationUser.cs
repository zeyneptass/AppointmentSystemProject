using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Domain.Entities.Identity
{
    public class ApplicationUser:IdentityUser<Guid> //Id tipinin Guid olmasını sağlıyoruz
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string TC { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        [StringLength(13)]
        public override string PhoneNumber { get; set; }
    }
}
