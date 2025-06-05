using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CleanFoodVietAPI.Data.Specifications
{
    public interface ISpecification<T>
    {
        // Filter Criteria: e.g., x => x.SomeProperty.Contains("abc")
        Expression<Func<T, bool>>? Criteria { get; }

        // Includes for navigation properties
        List<Expression<Func<T, object>>> Includes { get; }

        // Sorting
        Expression<Func<T, object>>? OrderBy { get; }
        Expression<Func<T, object>>? OrderByDescending { get; }

        // Paging parameters
        int? Take { get; }
        int? Skip { get; }
        bool IsPagingEnabled { get; }
    }
}
