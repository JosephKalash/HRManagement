using HRManagement.Core.Entities;
using HRManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Core.Extensions
{
    public static class IQueryableExtensions
    {
        public static async Task<PagedResult<BaseEntity>> ToPagedResultAsync(
            this IQueryable<BaseEntity> query, int pageNumber, int pageSize)
        {
            var totalCount = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).OrderByDescending(x => x.CreatedAt).ToListAsync();
            return new PagedResult<BaseEntity>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
    }
} 