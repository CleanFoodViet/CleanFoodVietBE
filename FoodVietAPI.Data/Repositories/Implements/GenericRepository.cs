using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using CleanFoodVietAPI.Data.Specifications; // Make sure this namespace is correct
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Data.Repositories.Implements
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<T>();
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        #region Get Methods

        public virtual async Task<T> GetAsync
            (Expression<Func<T, bool>>? predicate = null,
             Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
             Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = _dbSet;
            if (include != null)
                query = include(query);
            if (predicate != null)
                query = query.Where(predicate);
            if (orderBy != null)
                return await orderBy(query).AsNoTracking().FirstOrDefaultAsync();
            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public virtual async Task<TResult> GetAsync<TResult>
            (Expression<Func<T, TResult>> selector,
             Expression<Func<T, bool>>? predicate = null,
             Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
             Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = _dbSet;
            if (include != null)
                query = include(query);
            if (predicate != null)
                query = query.Where(predicate);
            if (orderBy != null)
                return await orderBy(query).AsNoTracking().Select(selector).FirstOrDefaultAsync();
            return await query.AsNoTracking().Select(selector).FirstOrDefaultAsync();
        }

        public virtual async Task<ICollection<T>> GetListAsync
            (Expression<Func<T, bool>>? predicate = null,
             Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
             Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = _dbSet;
            if (include != null)
                query = include(query);
            if (predicate != null)
                query = query.Where(predicate);
            if (orderBy != null)
                return await orderBy(query).AsNoTracking().ToListAsync();
            return await query.AsNoTracking().ToListAsync();
        }

        public virtual async Task<ICollection<TResult>> GetListAsync<TResult>
            (Expression<Func<T, TResult>> selector,
             Expression<Func<T, bool>>? predicate = null,
             Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
             Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = _dbSet;
            if (include != null)
                query = include(query);
            if (predicate != null)
                query = query.Where(predicate);
            if (orderBy != null)
                return await orderBy(query).AsNoTracking().Select(selector).ToListAsync();
            return await query.Select(selector).ToListAsync();
        }

        public Task<IPaginate<T>> GetPagingListAsync
            (Expression<Func<T, bool>>? predicate = null,
             Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
             Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
             int page = 1,
             int size = 10)
        {
            IQueryable<T> query = _dbSet;
            if (include != null)
                query = include(query);
            if (predicate != null)
                query = query.Where(predicate);
            if (orderBy != null)
                return orderBy(query).ToPaginateAsync(page, size, 1);
            return query.AsNoTracking().ToPaginateAsync(page, size, 1);
        }

        public Task<IPaginate<TResult>> GetPagingListAsync<TResult>
            (Expression<Func<T, TResult>> selector,
             Expression<Func<T, bool>>? predicate = null,
             Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
             Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
             int page = 1,
             int size = 10)
        {
            IQueryable<T> query = _dbSet;
            if (include != null)
                query = include(query);
            if (predicate != null)
                query = query.Where(predicate);
            if (orderBy != null)
                return orderBy(query).Select(selector).ToPaginateAsync(page, size, 1);
            return query.AsNoTracking().Select(selector).ToPaginateAsync(page, size, 1);
        }

        #endregion

        #region Specification Methods

        /// <summary>
        /// Applies the specification to the IQueryable.
        /// </summary>
        /// <param name="spec">The specification containing filter, includes, ordering and paging criteria.</param>
        /// <returns>An IQueryable with the specification applied.</returns>
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            IQueryable<T> query = _dbSet.AsQueryable();

            // Apply filtering (criteria)
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            // Apply includes for navigation properties.
            if (spec.Includes != null && spec.Includes.Any())
            {
                query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            }

            // Apply sorting
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            // Apply paging if enabled.
            if (spec.IsPagingEnabled)
            {
                if (spec.Skip.HasValue)
                {
                    query = query.Skip(spec.Skip.Value);
                }
                if (spec.Take.HasValue)
                {
                    query = query.Take(spec.Take.Value);
                }
            }

            return query;
        }

        /// <summary>
        /// Retrieves a list of entities based on the given specification.
        /// </summary>
        public async Task<ICollection<T>> GetListAsync(ISpecification<T> spec)
        {
            IQueryable<T> query = ApplySpecification(spec);
            return await query.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Retrieves a paged list of entities based on the given specification.
        /// </summary>
        public Task<IPaginate<T>> GetPagingListAsync(ISpecification<T> spec, int page = 1, int size = 10)
        {
            IQueryable<T> query = ApplySpecification(spec);
            return query.AsNoTracking().ToPaginateAsync(page, size, 1);
        }

        #endregion

        #region Insert Methods

        public async Task InsertAsync(T entity)
        {
            if (entity == null) return;
            await _dbSet.AddAsync(entity);
        }

        public async Task InsertRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        #endregion

        #region Update Methods

        public void UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        #endregion

        #region Delete Methods

        public void DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        #endregion
    }
}
