using CleanFoodVietAPI.Data.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CleanFoodVietAPI.Data.Specifications
{
    public class ServicePackageSpecification : BaseSpecification<ServicePackage>
    {
        public ServicePackageSpecification(
            string? filterField,
            string? filterValue,
            string? sortField,
            string? sortOrder,
            string? search)
            : base(BuildCriteria(filterField, filterValue, search))
        {
            // Apply sorting if a sortField is provided; otherwise, use a default sort.
            if (!string.IsNullOrEmpty(sortField))
            {
                if (sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    ApplyOrderBy(GetSortExpression(sortField));
                }
                else
                {
                    ApplyOrderByDescending(GetSortExpression(sortField));
                }
            }
            else
            {
                // Default sort by CreatedAt.
                ApplyOrderBy(x => x.CreatedAt);
            }
        }

        // Builds the filtering/search criteria based on incoming parameters.
        private static Expression<Func<ServicePackage, bool>> BuildCriteria(string? filterField, string? filterValue, string? search)
        {
            // Start with an "always true" predicate.
            Expression<Func<ServicePackage, bool>> criteria = sp => true;

            // Apply filtering using filterField and filterValue.
            if (!string.IsNullOrEmpty(filterField) && !string.IsNullOrEmpty(filterValue))
            {
                // For example, if filtering by PackageName.
                if (filterField.Equals("PackageName", StringComparison.OrdinalIgnoreCase))
                {
                    criteria = sp => sp.PackageName.Contains(filterValue);
                }
                else if (filterField.Equals("Status", StringComparison.OrdinalIgnoreCase))
                {
                    criteria = sp => sp.Status.Equals(filterValue, StringComparison.OrdinalIgnoreCase);
                }
                // Add additional filtering criteria if necessary.
            }

            // Apply a general search across PackageName and Description.
            if (!string.IsNullOrEmpty(search))
            {
                Expression<Func<ServicePackage, bool>> searchCriteria = sp =>
                    (sp.PackageName != null && sp.PackageName.Contains(search)) ||
                    (sp.Description != null && sp.Description.Contains(search));

                criteria = criteria.And(searchCriteria); // Combine the criteria.
            }

            return criteria;
        }

        // Returns a sorting expression based on the sortField value.
        private Expression<Func<ServicePackage, object>> GetSortExpression(string sortField)
        {
            if (sortField.Equals("PackageName", StringComparison.OrdinalIgnoreCase))
            {
                return sp => sp.PackageName;
            }
            else if (sortField.Equals("Price", StringComparison.OrdinalIgnoreCase))
            {
                return sp => sp.Price;
            }
            else if (sortField.Equals("CreatedAt", StringComparison.OrdinalIgnoreCase))
            {
                return sp => sp.CreatedAt;
            }
            // Default fallback — sort by CreatedAt.
            return sp => sp.CreatedAt;
        }
    }

    // Helper extension to combine expressions.
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left!, right!),
                parameter);
        }

        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression? Visit(Expression? node) =>
                node == _oldValue ? _newValue : base.Visit(node);
        }
    }
}
