using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DressUpExchange.Data.Entity;

namespace DressUpExchange.Data.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetWhere(Expression<Func<T, bool>>? filter = null);
        Task<T> GetAsync(Expression<Func<T, bool>>? filter = null);
        DbSet<T> GetAll();
        Task<List<T>> GetAllAsync();
        Task CreateAsync(T entity);
        EntityEntry<T> Delete(T entity);
        IQueryable<T> FindAll(Func<T, bool> predicate);
        T Find(Func<T, bool> predicate);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetById(int id);
        Task Update(T entity, int Id);
        Task<User> GetUserByPhoneAndPassword(string phone, string password);
    }
}
