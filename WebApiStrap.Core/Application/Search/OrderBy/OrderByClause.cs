namespace WebApi.Core.Application.Search.OrderBy
{
    using Infrastructure.Data.OrderBy;

    public class OrderByClause
    {
        public OrderByClause(string property, Direction direction)
        {
            Property = property;
            Direction = direction;
        }

        public string Property { get; private set; }
        public Direction Direction { get; private set; }
    }
}