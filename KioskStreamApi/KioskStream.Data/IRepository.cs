
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KioskStream.Data
{
    public interface IRepository<T>
    {
        IQueryable<T> Get(Expression<Func<T, bool>> expression);

        IQueryable<T> Get();

        Task<int> CountAsync(Expression<Func<T, bool>> expression);

        Task<int> CountAsync();

        void Insert(T entity);

        void Update(T entity);
        
        void Delete(T entity);

        Task<long> SaveChangesAsync();
    }
}
