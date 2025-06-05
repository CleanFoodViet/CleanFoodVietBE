using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Specifications;
using System;
using System.Linq.Expressions;

namespace CleanFoodVietAPI.Application.Specifications
{
    public class ServiceFeatureSpecification : BaseSpecification<ServiceFeature>
    {
        public ServiceFeatureSpecification(
                string? filterField,
                string? filterValue,
                string? sortField,
                string? sortOrder)
            : base(null)
        {
            // Filtering: if filter parameters are provided.
            if (!string.IsNullOrEmpty(filterField) && !string.IsNullOrEmpty(filterValue))
            {
                // Get the property info from ServiceFeature.
                var propertyInfo = typeof(ServiceFeature).GetProperty(filterField);
                if (propertyInfo == null)
                {
                    throw new Exception("Data field unavailable");
                }

                ParameterExpression param = Expression.Parameter(typeof(ServiceFeature), "x");
                Expression propertyExp = Expression.Property(param, propertyInfo);
                Expression lambdaBody;

                // If the property is a string, use Contains.
                if (propertyInfo.PropertyType == typeof(string))
                {
                    var constant = Expression.Constant(filterValue);
                    lambdaBody = Expression.Call(propertyExp, "Contains", Type.EmptyTypes, constant);
                }
                // If the property is an enum, parse the value and check for equality.
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

                this.Criteria = Expression.Lambda<Func<ServiceFeature, bool>>(lambdaBody, param);
            }

            // Sorting:
            if (!string.IsNullOrEmpty(sortField))
            {
                var propertyInfo = typeof(ServiceFeature).GetProperty(sortField);
                if (propertyInfo == null)
                {
                    throw new Exception("Data field unavailable");
                }
                ParameterExpression param = Expression.Parameter(typeof(ServiceFeature), "x");
                var propertyExp = Expression.Property(param, propertyInfo);
                // Convert to object for a unified lambda.
                var lambda = Expression.Lambda<Func<ServiceFeature, object>>(
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
