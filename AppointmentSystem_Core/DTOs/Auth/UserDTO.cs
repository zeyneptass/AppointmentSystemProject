using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Core.DTOs.Auth
{
    public class UserDTO
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; }
        public string TC { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; } = DateTime.Now;
    }
}
    