using AppointmentSystem_Domain.Entities;
using AppointmentSystem_Domain.Entities.Base; // BaseEntity için
using AppointmentSystem_Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AppointmentSystem_Infrastructure.Persistence.Context
{
    public class AppointmentDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Patient> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Identity tablolarının oluşmasını sağlar :
            base.OnModelCreating(modelBuilder);

            // 2. KRİTİK:  Configuration dosyalarını (PatientConfiguration vb.) yükler
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // 3. KRİTİK: Global Query Filter (Soft Delete - Case 1)
            modelBuilder.Entity<Department>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Doctor>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Patient>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Appointment>().HasQueryFilter(x => !x.IsDeleted);
            //Böylece silinen veriler otomatik olarak sorgulamalardan filtrelenir.
        }

        // SaveChangesAsync Amacı: Her kaydın ne zaman oluşturulduğunu ve son güncellemesinin ne zaman yapıldığını otomatik olarak kaydetmek.
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.IsDeleted = false;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedDate = DateTime.Now;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}