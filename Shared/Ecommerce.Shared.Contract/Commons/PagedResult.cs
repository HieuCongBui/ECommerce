namespace Ecommerce.Shared.Contract.Commons
{
    public class PagedResult<TEntity>
    {
        public IReadOnlyList<TEntity> Items { get; }
        public int TotalCount { get; }
        public int PageIndex { get; }
        public int PageSize { get; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public PagedResult(IReadOnlyList<TEntity> items, int totalCount, int pageIndex, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }

    public record PaginationRequest(int pageSize = 10, int pageIndex = 1)
    { }
}
