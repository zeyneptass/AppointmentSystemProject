using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Infrastructure.Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
        {
            // Sabit GUID'ler verdim ki, roller her zaman aynı ID'ye sahip olsun.
            builder.HasData(
                new IdentityRole<Guid>
                {
                    Id = Guid.Parse("a1b2c3d4-e5f6-4789-9012-34567890abcd"),
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole<Guid>
                {
                    Id = Guid.Parse("b2c3d4e5-f678-4890-1234-567890abcdef"),
                    Name = "Doctor",
                    NormalizedName = "DOCTOR"
                },
                new IdentityRole<Guid>
                {
                    Id = Guid.Parse("c3d4e5f6-7890-4901-2345-67890abcdef1"),
                    Name = "Patient",
                    NormalizedName = "PATIENT"
                }
            );
        }
    }
}
