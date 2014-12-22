namespace WebApi.Core.Infrastructure.Data
{
    public class Pagination
    {
        public Pagination(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }

        public int Page { get; private set; }
        public int PageSize { get; private set; }
        public int RecordsToSkip
        {
            get { return Page == 0 ? 0 : (Page - 1) * PageSize; }
        }
    }
}