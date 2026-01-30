using AppointmentSystem_Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Infrastructure.Persistence.Seed
{
    public class AdminSeeding
    {
        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
        {
            var adminUser = await userManager.FindByEmailAsync("admin@hospital.com");

            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = "99988877716",
                    Email = "admin@hospital.com",
                    EmailConfirmed = true,
                    Name = "Sistem",
                    Surname = "Admin",
                    TC = "99988877716",
                    PhoneNumber = "+905551234567",
                    DateOfBirth = new DateTime(1990, 01, 01)
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");  // Admin rolüne ata
                }
            }
        }
    }
}
