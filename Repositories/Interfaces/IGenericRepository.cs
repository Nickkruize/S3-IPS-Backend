using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Repositories.Interfaces
{
    public interface IGenericRepository<T>
    {
        Task<IEnumerable<T>> FindAll();
        Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression);
        Task<T> GetById(int id);
        Task<T> Create(T entity);
        void Update(T entity);
        T Delete(T entity);
        EntityEntry Delete2(T entity);
        IEnumerable<T> DeleteRange(List<T> entities);
        Task Save();
    }
}
