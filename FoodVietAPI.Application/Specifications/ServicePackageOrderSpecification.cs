// SubscriptionOrderSpecification.cs
using CleanFoodVietAPI.Application.Utils;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Specifications;
using System;
using System.Linq.Expressions;

namespace CleanFoodVietAPI.Application.Specifications
{
    public class ServicePackageOrderSpecification
        : BaseSpecification<ServicePackageOrder>
    {
        public ServicePackageOrderSpecification(
            string? filterField,
            string? filterValue,
            string? sortField,
            string? sortOrder,
            string? search)
            : base(null)
        {
            Expression<Func<ServicePackageOrder, bool>>? criteria = null;

            // FILTER
            if (!string.IsNullOrWhiteSpace(filterField)
                && !string.IsNullOrWhiteSpace(filterValue))
            {
                var pi = typeof(ServicePackageOrder).GetProperty(filterField);
                if (pi == null)
                    throw new Exception($"Field '{filterField}' not found on SubscriptionOrder");

                var param = Expression.Parameter(typeof(ServicePackageOrder), "x");
                var prop = Expression.Property(param, pi);
                Expression body;

                if (pi.PropertyType == typeof(string))
                {
                    body = Expression.Call(
                        prop,
                        nameof(string.Contains),
                        Type.EmptyTypes,
                        Expression.Constant(filterValue));
                }
                else if (pi.PropertyType.IsEnum)
                {
                    var enumVal = Enum.Parse(pi.PropertyType, filterValue, true);
                    body = Expression.Equal(prop, Expression.Constant(enumVal));
                }
                else
                {
                    throw new Exception("Unsupported filter type");
                }

                criteria = Expression.Lambda<Func<ServicePackageOrder, bool>>(body, param);
            }

            // SEARCH across GardenerId or ServicePackageId
            // Currently support ulid, and Gardener name, email, phone number
            if (!string.IsNullOrWhiteSpace(search))
            {
                Expression<Func<ServicePackageOrder, bool>> searchExpr;

                if (Ulid.TryParse(search, out var parsedUlid))
                {
                    searchExpr = x =>
                        x.GardenerId == parsedUlid ||
                        x.ServicePackageId == parsedUlid;
                }
                else
                {
                    searchExpr = x =>
                        x.Status.Contains(search) ||
                        x.Gardener.Name.Contains(search) ||
                        x.Gardener.Email.Contains(search) ||
                        x.Gardener.PhoneNumber.Contains(search);
                }

                criteria = criteria == null
                    ? searchExpr
                    : criteria.AndAlso(searchExpr);
            }

            this.Criteria = criteria;


            // SORT
            if (!string.IsNullOrWhiteSpace(sortField))
            {
                var pi = typeof(ServicePackageOrder).GetProperty(sortField);
                if (pi == null)
                    throw new Exception($"Field '{sortField}' not found on SubscriptionOrder");

                var param = Expression.Parameter(typeof(ServicePackageOrder), "x");
                var prop = Expression.Property(param, pi);
                var lambda = Expression.Lambda<Func<ServicePackageOrder, object>>(
                                 Expression.Convert(prop, typeof(object)), param);

                if (string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase))
                    ApplyOrderByDescending(lambda);
                else
                    ApplyOrderBy(lambda);
            }

            // 4) INCLUDE payments
            AddInclude(x => x.ServicePackageOrderPayments);
        }
    }
}
