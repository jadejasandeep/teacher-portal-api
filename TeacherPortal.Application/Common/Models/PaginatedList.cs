using Microsoft.EntityFrameworkCore;

namespace TeacherPortal.Application.Common.Models
{
   
    public class PaginatedList<T>
    {
        public List<T> Items { get; private set; }
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }

        private const int pageIndexDefault = 1;
        private const int pageSizeDefault = 100;

        public PaginatedList() { }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }

        /// <summary>
        /// Used by the PaginatedListBuilder to build a new List type using another paginated list's data
        /// </summary>
        /// <param name="items"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalPages"></param>
        internal void PopulatePaginatedList(List<T> items, int totalCount, int pageIndex, int totalPages)
        {
            Items = items;
            TotalCount = totalCount;
            PageIndex = pageIndex;
            TotalPages = totalPages;
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int? pageIndex, int? pageSize, CancellationToken cancellationToken)
        {
            var count = await source.CountAsync(cancellationToken);

            var index = (pageIndex ?? 0) == 0 ? pageIndexDefault : (pageIndex ?? pageIndexDefault);
            var size = (pageSize == null && pageIndex == null) ? count : ((pageSize ?? 0) == 0 ? pageSizeDefault : (pageSize ?? pageSizeDefault));

            var items = await source.Skip((index - 1) * size).Take(size).ToListAsync(cancellationToken);

            return new PaginatedList<T>(items, count, index, size);
        }

        public static PaginatedList<T> CreateAsync(IList<T> source, int? pageIndex, int? pageSize)
        {
            var count = source.Count;

            var index = (pageIndex ?? 0) == 0 ? pageIndexDefault : (pageIndex ?? pageIndexDefault);
            var size = (pageSize == null && pageIndex == null) ? count : ((pageSize ?? 0) == 0 ? pageSizeDefault : (pageSize ?? pageSizeDefault));

            var items = source.Skip((index - 1) * size).Take(size).ToList();

            return new PaginatedList<T>(items, count, index, size);
        }
    }
}
