using Microsoft.EntityFrameworkCore;

namespace HeartSpace.Domain.RequestFeatures
{
    public class PagedList<T> : List<T>
    {
        public MetaData MetaData { get; set; }
        public PagedList(List<T> items, int totalCount, int pageNumber, int pageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
            AddRange(items);
        }
        public async static Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var totalCount = await source.CountAsync();
            var items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
            return new PagedList<T>(items, totalCount, pageNumber, pageSize);
        }
    }
}
