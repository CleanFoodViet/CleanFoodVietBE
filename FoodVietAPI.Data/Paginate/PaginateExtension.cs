using Microsoft.EntityFrameworkCore;

namespace CleanFoodVietAPI.Data.Paginate
{
    public static class PaginateExtension
    {
        public static async Task<IPaginate<T>> ToPaginateAsync<T>(this IQueryable<T> queryable, int page, int size, int firstPage = 1)
        {
            if (firstPage > page)
                throw new ArgumentException($"Page ({page}) must greater or equal than firstPage ({firstPage})");

            
            var total = await queryable.CountAsync();
            var items = await queryable.Skip((page - firstPage) * size).Take(size).ToListAsync();
            var totalPages = (int)Math.Ceiling(total / (double)size);

            if(size <= 0)
            {
                total = 0;
                totalPages = 0;
            }

            return new Paginate<T>
            {
                Page = page,
                Size = size,
                Total = total,
                Items = items,
                TotalPages = totalPages
            };
        }
    }
}
