using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UniversitySystem.DataAccess.Repositories
{
    public class Repository<TEntity> : IAsyncRepository<TEntity> where TEntity : class
    {
        private readonly IUniversitySystemDbContext _dbContext;

        public Repository(IUniversitySystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async virtual Task<TEntity> GetById(int id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async virtual Task<IEnumerable<TEntity>> List()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public async virtual Task<IEnumerable<TEntity>> List(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbContext.Set<TEntity>()
                            .Where(predicate)
                            .ToListAsync();
        }

        public async Task Add(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(TEntity entity)
        {
            // In case AsNoTracking is used
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
