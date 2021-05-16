using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace UniversitySystem.DataAccess.Repositories
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        TEntity GetById(int id);

        IEnumerable<TEntity> List();

        IEnumerable<TEntity> List(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);

        void Delete(TEntity entity);

        void Update(TEntity entity);
    }

    public abstract class EntityBase
    {
        public int Id { get; protected set; }
    }
}
