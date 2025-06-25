using System.Linq.Expressions;

namespace CleanFoodVietAPI.Data.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        protected BaseSpecification(Expression<Func<T, bool>>? criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>>? Criteria { get; protected set; }  // Modified to allow setting

        public Expression<Func<T, object>>? OrderBy { get; private set; }

        public Expression<Func<T, object>>? OrderByDescending { get; private set; }

        private readonly List<Expression<Func<T, object>>> _includes = new();
        public IReadOnlyList<Expression<Func<T, object>>> Includes => _includes;

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
          => _includes.Add(includeExpression);

        protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }
    }
}

