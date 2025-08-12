using CleanFoodVietAPI.Application.Utils;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Specifications;
using System.Linq.Expressions;

namespace CleanFoodVietAPI.Application.Specifications
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(
            string? filterField,
            string? filterValue,
            string? sortField,
            string? sortOrder,
            string? search) : base(null)
        {
            Expression<Func<Product, bool>>? criteria = null;

            // Existing filtering based on filterField/filterValue:
            if (!string.IsNullOrEmpty(filterField) && !string.IsNullOrEmpty(filterValue))
            {
                var propertyInfo = typeof(Product).GetProperty(filterField);
                if (propertyInfo == null)
                {
                    throw new Exception("Data field unavailable");
                }

                ParameterExpression param = Expression.Parameter(typeof(Product), "x");
                Expression propertyExp = Expression.Property(param, propertyInfo);
                Expression lambdaBody;

                if (propertyInfo.PropertyType == typeof(string))
                {
                    var constant = Expression.Constant(filterValue);
                    lambdaBody = Expression.Call(
                                     propertyExp,
                                     typeof(string).GetMethod("Equals", new[] { typeof(string) })!,
                                     Expression.Constant(filterValue));
                }
                else if (propertyInfo.PropertyType.IsEnum)
                {
                    object enumValue;
                    try
                    {
                        enumValue = Enum.Parse(propertyInfo.PropertyType, filterValue, true);
                    }
                    catch
                    {
                        throw new Exception("Invalid enum value for filter.");
                    }
                    var constant = Expression.Constant(enumValue);
                    lambdaBody = Expression.Equal(propertyExp, constant);
                }
                else
                {
                    throw new Exception("Filtering is not supported for the given field type.");
                }

                criteria = Expression.Lambda<Func<Product, bool>>(lambdaBody, param);
            }

            // Additional search filtering on ServiceFeatureName:
            if (!string.IsNullOrEmpty(search))
            {
                Expression<Func<Product, bool>> searchCriteria =
                    x => x.ProductName.Contains(search);
                criteria = criteria == null ? searchCriteria : criteria.AndAlso(searchCriteria);
            }

            this.Criteria = criteria;

            // Sorting logic remains the same:
            if (!string.IsNullOrEmpty(sortField))
            {
                var propertyInfo = typeof(Product).GetProperty(sortField);
                if (propertyInfo == null)
                {
                    throw new Exception("Data field unavailable");
                }
                ParameterExpression param = Expression.Parameter(typeof(Product), "x");
                var propertyExp = Expression.Property(param, propertyInfo);
                var lambda = Expression.Lambda<Func<Product, object>>(
                    Expression.Convert(propertyExp, typeof(object)), param);

                if (!string.IsNullOrEmpty(sortOrder) && sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    ApplyOrderByDescending(lambda);
                }
                else
                {
                    ApplyOrderBy(lambda);
                }
            }
        }
    }
}
