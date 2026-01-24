
using AppointmentSystem_Core.DataAccess.Abstract;
using AppointmentSystem_Domain.Entities;
using AppointmentSystem_Domain.Entities.Base;
using AppointmentSystem_Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        AppointmentDbContext _context;
        DbSet<T> _dbSet;
        private AppointmentDbContext context;
        private DbSet<Doctor> doctors;
        private DbSet<Patient> patients;
        private DbSet<Appointment> appointments;

        public GenericRepository(AppointmentDbContext context, DbSet<AppointmentSystem_Domain.Entities.Department> departments)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public GenericRepository(AppointmentDbContext context, DbSet<Doctor> doctors)
        {
            this.context = context;
            this.doctors = doctors;
        }

        public GenericRepository(AppointmentDbContext context, DbSet<Patient> patients)
        {
            this.context = context;
            this.patients = patients;
        }

        public GenericRepository(AppointmentDbContext context, DbSet<Appointment> appointments)
        {
            this.context = context;
            this.appointments = appointments;
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public Task<bool> ExistsAsync(Expression<Func<T, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool asNoTracking = true, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter, bool asNoTracking = true, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            if(filter != null)
            {
                query = query.Where(filter);              
            }
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }
            if (includes != null )
                {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.ToListAsync();

        }

        public Task<T> GetByIdAsync(Guid id, bool asNoTracking = true)
        {
            var query = _dbSet.AsQueryable();
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }
            return query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async void SoftDelete(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
           if (entity != null)
            {
                entity.IsDeleted = true;
                _dbSet.Update(entity);
            }
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
