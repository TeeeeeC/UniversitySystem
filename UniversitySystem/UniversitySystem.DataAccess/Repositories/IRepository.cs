using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UniversitySystem.DataAccess.Repositories
{
    public interface IAsyncRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);

        Task<int> AddAsync(TEntity entity);

        Task<int> DeleteAsync(TEntity entity);

        Task<int> UpdateAsync(TEntity entity);
    }
}
