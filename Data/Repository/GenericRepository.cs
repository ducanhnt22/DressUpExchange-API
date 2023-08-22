using DressUpExchange.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private static DressUpExchanceContext Context;
        private static DbSet<T> Table { get; set; }
        public GenericRepository(DressUpExchanceContext context)
        {
            Context = context;
            Table = Context.Set<T>();
        }
        public Task CreateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public EntityEntry<T> Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public T Find(Func<T, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> FindAll(Func<T, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public DbSet<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(Expression<Func<T, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetWhere(Expression<Func<T, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public Task Update(T entity, int Id)
        {
            throw new NotImplementedException();
        }
    }
}
