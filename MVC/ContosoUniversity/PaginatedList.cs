using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity
{
    public class PaginatedList<T>: List<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize) 
        {
            //List<T> - знак T означает шаблон
            this.PageIndex = pageIndex;
            this.TotalPages = count;
            this.AddRange(items);
        }
        public bool HasPreviosPage => PageIndex > 1;
        public bool HasNext6Page => PageIndex < TotalPages;
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize) 
        {
            int count = await source.CountAsync();
            List<T> items = await source
                                            .Skip(pageIndex)
                                            .Take(pageSize)
                                            .ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
