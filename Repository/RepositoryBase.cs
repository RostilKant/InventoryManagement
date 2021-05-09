using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ApplicationContext ApplicationContext { get; set; }

        public RepositoryBase(ApplicationContext applicationContext)
        {
            ApplicationContext = applicationContext;
        }

        public IQueryable<T> FindAll() =>
            ApplicationContext.Set<T>()
                .AsNoTracking();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false) =>
            !trackChanges
                ? ApplicationContext.Set<T>()
                    .Where(expression)
                    .AsNoTracking()
                : ApplicationContext.Set<T>()
                    .Where(expression);

        public void Create(T entity) => ApplicationContext.Set<T>().Add(entity);

        public void CreateRange(IEnumerable<T> entities) => ApplicationContext.Set<T>().AddRange(entities);
        
        public void Update(T entity) => ApplicationContext.Set<T>().Update(entity);

        public void Delete(T entity) => ApplicationContext.Set<T>().Remove(entity);

        public void DeleteRange(IEnumerable<T> entities) => ApplicationContext.Set<T>().RemoveRange(entities);
    }
}