namespace WebApi.Core.Infrastructure.Data.Searching
{
    public class SearchResultSet<T>
    {
        public SearchResultSet(T[] resultSet, int total, Pagination pagination)
        {
            Paging = new PagingInformation(pagination, resultSet.Length, total);
            ResultSet = resultSet;
            Total = total;
        }

        public PagingInformation Paging { get; private set; }
        public T[] ResultSet { get; private set; }
        public int Total { get; private set; }
    }
}