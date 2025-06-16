using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Specifications;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CleanFoodVietAPI.Data.Repositories.Interfaces
{
    public interface IGenericRepository<T> : IDisposable where T : class
    {
        #region Get Methods
        Task<T> GetAsync(Expression<Func<T, bool>>? predicate = null,
             Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        Task<TResult> GetAsync<TResult>(Expression<Func<T, TResult>> selector,
             Expression<Func<T, bool>>? predicate = null,
             Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        Task<ICollection<T>> GetListAsync(ISpecification<T>? spec = null,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        Task<ICollection<TResult>> GetListAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            ISpecification<T>? spec = null);
        Task<IPaginate<T>> GetPagingListAsync(
            ISpecification<T>? spec = null,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            int page = 1, int size = 10);
        Task<IPaginate<TResult>> GetPagingListAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            ISpecification<T>? spec = null,
            int page = 1, int size = 10);
        #endregion

        #region Insert Methods
        Task InsertAsync(T entity);
        Task InsertRangeAsync(IEnumerable<T> entities);
        #endregion

        #region Update Methods
        void UpdateAsync(T entity);
        void UpdateRange(IEnumerable<T> entities);
        #endregion

        #region Delete Methods
        void DeleteAsync(T entity);
        void DeleteRangeAsync(IEnumerable<T> entities);
        #endregion        
    }
}
