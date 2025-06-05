using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CleanFoodVietAPI.Data.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        protected BaseSpecification(Expression<Func<T, bool>>? criteria)
        {
            Criteria = criteria;
            Includes = new List<Expression<Func<T, object>>>();
        }

        public Expression<Func<T, bool>>? Criteria { get; protected set; }  // Modified to allow setting

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>>? OrderBy { get; private set; }

        public Expression<Func<T, object>>? OrderByDescending { get; private set; }

        public int? Take { get; private set; }

        public int? Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
    }
}

