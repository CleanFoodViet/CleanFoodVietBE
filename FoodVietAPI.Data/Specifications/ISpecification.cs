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
    }
}
