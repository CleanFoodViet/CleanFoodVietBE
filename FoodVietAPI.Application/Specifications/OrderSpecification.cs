using CleanFoodVietAPI.Application.Utils;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Specifications
{
    public class OrderSpecification : BaseSpecification<Order>
    {
        public OrderSpecification(
            string? filterField,
            string? filterValue,
            //string? search,
            string? sortField,
            string? sortOrder)
            : base(null)
        {
            Expression<Func<Order, bool>>? criteria = null;

            // Existing filtering based on filterField/filterValue:
            if (!string.IsNullOrEmpty(filterField) && !string.IsNullOrEmpty(filterValue))
            {
                var propertyInfo = typeof(Order).GetProperty(filterField);
                if (propertyInfo == null)
                {
                    throw new Exception("Data field unavailable");
                }

                ParameterExpression param = Expression.Parameter(typeof(Order), "x");
                Expression propertyExp = Expression.Property(param, propertyInfo);
                Expression lambdaBody;

                if (propertyInfo.PropertyType == typeof(string))
                {
                    var constant = Expression.Constant(filterValue);
                    lambdaBody = Expression.Call(propertyExp, "Equals", Type.EmptyTypes, constant);
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

                criteria = Expression.Lambda<Func<Order, bool>>(lambdaBody, param);
            }

            //// Additional search filtering on ServiceFeatureName:
            //if (!string.IsNullOrEmpty(search))
            //{
            //    Expression<Func<Order, bool>> searchCriteria =
            //        x => x.Title.Contains(search);
            //    criteria = criteria == null ? searchCriteria : criteria.AndAlso(searchCriteria);
            //}

            this.Criteria = criteria;

            // Sorting logic remains the same:
            if (!string.IsNullOrEmpty(sortField))
            {
                var propertyInfo = typeof(Order).GetProperty(sortField);
                if (propertyInfo == null)
                {
                    throw new Exception("Data field unavailable");
                }
                ParameterExpression param = Expression.Parameter(typeof(Order), "x");
                var propertyExp = Expression.Property(param, propertyInfo);
                var lambda = Expression.Lambda<Func<Order, object>>(
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
