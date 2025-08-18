// SubscriptionContractSpecification.cs
using CleanFoodVietAPI.Application.Utils;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Specifications;
using System.Linq.Expressions;

namespace CleanFoodVietAPI.Application.Specifications
{
    public class SubscriptionContractSpecification
        : BaseSpecification<SubscriptionContract>
    {
        public SubscriptionContractSpecification(
            string? filterField,
            string? filterValue,
            string? sortField,
            string? sortOrder,
            string? search)
            : base(null)
        {
            Expression<Func<SubscriptionContract, bool>>? criteria = null;

            // 1. FILTER by any field (string or enum)
            if (!string.IsNullOrWhiteSpace(filterField)
                && !string.IsNullOrWhiteSpace(filterValue))
            {
                var pi = typeof(SubscriptionContract).GetProperty(filterField);
                if (pi == null)
                    throw new Exception($"Field '{filterField}' not found on SubscriptionContract");

                var param = Expression.Parameter(typeof(SubscriptionContract), "x");
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

                criteria = Expression.Lambda<Func<SubscriptionContract, bool>>(body, param);
            }

            // 2. SEARCH across GardenerId or ServicePackageId
            // Currently support ulid, and Gardener name, email, phone number
            if (!string.IsNullOrWhiteSpace(search))
            {
                Expression<Func<SubscriptionContract, bool>> searchExpr;

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
                        x.SubscriptionType.Contains(search) ||
                        x.Account.Name.Contains(search) ||
                        x.Account.Email.Contains(search) ||
                        x.Account.PhoneNumber.Contains(search);
                }

                criteria = criteria == null
                    ? searchExpr
                    : criteria.AndAlso(searchExpr);
            }

            this.Criteria = criteria;


            // 3. SORTING
            if (!string.IsNullOrWhiteSpace(sortField))
            {
                var pi = typeof(SubscriptionContract).GetProperty(sortField);
                if (pi == null)
                    throw new Exception($"Field '{sortField}' not found on SubscriptionContract");

                var param = Expression.Parameter(typeof(SubscriptionContract), "x");
                var prop = Expression.Property(param, pi);
                var lambda = Expression.Lambda<Func<SubscriptionContract, object>>(
                                 Expression.Convert(prop, typeof(object)), param);

                if (string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase))
                    ApplyOrderByDescending(lambda);
                else
                    ApplyOrderBy(lambda);
            }

            // 4) INCLUDE benefits
            AddInclude(x => x.SubscriptionContractBenefits);
        }
    }
}
