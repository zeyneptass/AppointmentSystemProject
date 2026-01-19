
using AppointmentSystem_Core.DataAccess.Abstract;
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
        public GenericRepository(AppointmentDbContext context, DbSet<T> dbSet)
        {
            _context = context;
            _dbSet = dbSet;
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }


        public async Task<T> GetById(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity;
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }


        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).ToListAsync();
            
        }
    }
}
