using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UniversitySystem.DataAccess.Repositories
{
    public interface IAsyncRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetById(int id);

        Task<IEnumerable<TEntity>> List();

        Task<IEnumerable<TEntity>> List(Expression<Func<TEntity, bool>> predicate);

        Task Add(TEntity entity);

        Task Delete(TEntity entity);

        Task Update(TEntity entity);
    }
}
