using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KioskStream.Data
{
    /// <summary>
    /// Generic Repository implementation for CRUD operations
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _entities;

        public Repository(KioskStreamDbContext context)
        {
            _dbContext = context;
            _entities = context.Set<T>();
        }

        /// <summary>
        /// Get query with by filter expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>Filter and return query</returns>
        public IQueryable<T> Get(Expression<Func<T, bool>> expression)
        {
            return _entities.Where(expression);
        }

        /// <summary>
        /// Get current entity query
        /// </summary>
        /// <returns>Return query</returns>
        public IQueryable<T> Get()
        {
            return _entities.AsQueryable();
        }

        /// <summary>
        /// Count of entity
        /// </summary>
        /// <param name="expression">expression</param>
        /// <returns>Return count after filter</returns>
        public async Task<int> CountAsync(Expression<Func<T, bool>> expression)
        {
            return await _entities.CountAsync(expression);
        }

        /// <summary>
        /// Get total count of entity
        /// </summary>
        /// <returns>Return total count</returns>
        public async Task<int> CountAsync()
        {
            return await _entities.CountAsync();
        }

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">entity</param>
        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _entities.Add(entity);
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">entity</param>
        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            //Entities.Update(entity);
            _entities.Update(entity);
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">entity</param>
        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _entities.Remove(entity);
        }

        /// <summary>
        ///     <para>
        ///         Saves all changes made in this context to the database.
        ///     </para>
        ///     <para>
        ///         Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        ///         that any asynchronous operations have completed before calling another method on this context.
        ///     </para>
        /// </summary>
        /// <returns>
        ///     A task that represents the asynchronous save operation. The task result contains the
        ///     number of state entries written to the database.
        /// </returns>
        /// <exception cref="DbUpdateException">
        ///     An error is encountered while saving to the database.
        /// </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        ///     A concurrency violation is encountered while saving to the database.
        ///     A concurrency violation occurs when an unexpected number of rows are affected during save.
        ///     This is usually because the data in the database has been modified since it was loaded into memory.
        /// </exception>
        public async Task<long> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }

}
