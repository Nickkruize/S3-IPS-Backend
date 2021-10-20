using DAL;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repositories.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected WebshopContext RepositoryContext { get; set; }

        public GenericRepository(WebshopContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }

        public IEnumerable<T> FindAll()
        {
            return this.RepositoryContext.Set<T>().ToList();
        }

        public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.RepositoryContext.Set<T>().Where(expression).ToList();
        }

        public T GetById(int id)
        {
            return this.RepositoryContext.Set<T>().Find(id);
        }

        public T Create(T entity)
        {
            this.RepositoryContext.Set<T>().Add(entity);
            return entity;
        }

        public void Update(T entity)
        {
            this.RepositoryContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.RepositoryContext.Set<T>().Remove(entity);
        }

        public void Delete(int id)
        {
            this.RepositoryContext.Remove(RepositoryContext.Set<T>().Find(id));
        }

        public void Save()
        {
            this.RepositoryContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.RepositoryContext.DisposeAsync();
        }
    }
}
