
using AppointmentSystem_Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Core.DataAccess.Abstract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task AddAsync(T entity);
        void Update(T entity);
        void SoftDelete(Guid id);
        Task<T> GetByIdAsync(Guid id, bool asNoTracking=true);
        Task<IEnumerable<T>> GetAllAsync(bool asNoTracking = true, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter, bool asNoTracking = true, params Expression<Func<T, object>>[] includes);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> filter); // belirli bir koşulu sağlayan kayıt var mı, yok mu?
    }
}
