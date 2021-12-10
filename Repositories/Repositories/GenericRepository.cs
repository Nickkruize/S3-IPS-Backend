using DAL;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected WebshopContext RepositoryContext { get; set; }

        public GenericRepository(WebshopContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }

        public async Task<IEnumerable<T>> FindAll()
        {
            return await this.RepositoryContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return await this.RepositoryContext.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await this.RepositoryContext.Set<T>().FindAsync(id);
        }

        public async Task<T> Create(T entity)
        {
            var result = await this.RepositoryContext.Set<T>().AddAsync(entity);
            return result.Entity;
            
        }

        public void Update(T entity)
        {
            this.RepositoryContext.Set<T>().Update(entity);
        }

        public async Task<T> Delete(T entity)
        {
            this.RepositoryContext.Set<T>().Remove(entity);
            return entity;
        }

        public async Task Save()
        {
            await this.RepositoryContext.SaveChangesAsync();
        }
    }
}
