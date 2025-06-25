using System.Linq.Expressions;

namespace CleanFoodVietAPI.Data.Specifications
{
    public interface ISpecification<T>
    {
        // Filter Criteria: e.g., x => x.SomeProperty.Contains("abc")
        Expression<Func<T, bool>>? Criteria { get; }

        // Sorting
        Expression<Func<T, object>>? OrderBy { get; }
        Expression<Func<T, object>>? OrderByDescending { get; }

        // INCLUDES: e.g., x => x.RelatedEntity
        IReadOnlyList<Expression<Func<T, object>>> Includes { get; }
    }
}
