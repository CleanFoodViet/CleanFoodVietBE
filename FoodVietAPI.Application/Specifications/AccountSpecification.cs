using CleanFoodVietAPI.Application.Utils;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Specifications;
using System.Linq.Expressions;

namespace CleanFoodVietAPI.Application.Specifications
{
    public class AccountSpecification : BaseSpecification<Account>
    {
        public AccountSpecification(
             string? filterField,
             string? filterValue,
             string? sortField,
             string? sortOrder,
             string? search
            ) : base(null)
        {
            Expression<Func<Account, bool>>? criteria = null;

            // Existing filtering based on filterField/filterValue:
            if (!string.IsNullOrEmpty(filterField) && !string.IsNullOrEmpty(filterValue))
            {
                var propertyInfo = typeof(Account).GetProperty(filterField);
                if (propertyInfo == null)
                {
                    throw new Exception("Data field unavailable");
                }

                ParameterExpression param = Expression.Parameter(typeof(Account), "x");
                Expression propertyExp = Expression.Property(param, propertyInfo);
                Expression lambdaBody;

                if (propertyInfo.PropertyType == typeof(string))
                {
                    var constant = Expression.Constant(filterValue);
                    lambdaBody = Expression.Call(propertyExp, "Contains", Type.EmptyTypes, constant);
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

                criteria = Expression.Lambda<Func<Account, bool>>(lambdaBody, param);
            }

            // Additional search filtering on ServiceFeatureName:
            if (!string.IsNullOrEmpty(search))
            {
                Expression<Func<Account, bool>> searchCriteria =
                    x => x.PhoneNumber.Contains(search);
                criteria = criteria == null ? searchCriteria : criteria.AndAlso(searchCriteria);
            }

            this.Criteria = criteria;

            // Sorting logic remains the same:
            if (!string.IsNullOrEmpty(sortField))
            {
                var propertyInfo = typeof(Account).GetProperty(sortField);
                if (propertyInfo == null)
                {
                    throw new Exception("Data field unavailable");
                }
                ParameterExpression param = Expression.Parameter(typeof(Account), "x");
                var propertyExp = Expression.Property(param, propertyInfo);
                var lambda = Expression.Lambda<Func<Account, object>>(
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
