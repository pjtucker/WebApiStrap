namespace WebApiStrap.Infrastructure.Data.Searching
{
    using System;

    public class PagingInformation
    {
        public PagingInformation(Pagination pagination, int resultsCount, int total)
        {
            if (pagination == null)
            {
                int page = resultsCount > 0 ? 1 : 0;
                pagination = new Pagination(page, resultsCount);
            }
            var totalPages = pagination.PageSize > 0 ?
                (int) Math.Ceiling((decimal) total / (decimal) pagination.PageSize) : 0;

            TotalPages = totalPages;
            HasNext = pagination.Page < totalPages;
            HasPrevious = pagination.Page > 1;
            CurrentPageNumber = pagination.Page;
            CurrentPageSize = resultsCount;
            PageSize = pagination.PageSize;
        }

        public int CurrentPageSize { get; private set; }
        public int CurrentPageNumber { get; private set; }
        public bool HasNext { get; private set; }
        public bool HasPrevious { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
    }
}