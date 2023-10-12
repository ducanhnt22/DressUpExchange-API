using DressUpExchange.Data.Entity;
using DressUpExchange.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DressupExchanceContext _context;
        public UnitOfWork(DressupExchanceContext context)
        {
            _context = context;
        }
        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();
        public int Commit()
        {
            return _context.SaveChanges();
        }
        public Task<int> CommitAsync() => _context.SaveChangesAsync();

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            Type type = typeof(T);
            if (!repositories.TryGetValue(type, out object value))
            {
                var genericRepos = new GenericRepository<T>(_context);
                repositories.Add(type, genericRepos);
                return genericRepos;
            }
            return value as GenericRepository<T>;
        }
    }
}
