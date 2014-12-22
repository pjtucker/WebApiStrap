namespace WebApi.Core.Infrastructure.Data.OrderBy
{
    using System.Linq.Expressions;

    public class OrderByClause
    {
        public OrderByClause(LambdaExpression keySelector, Direction direction)
        {
            KeySelector = keySelector;
            Direction = direction;
        }

        public LambdaExpression KeySelector { get; private set; }
        public Direction Direction { get; private set; }
    }
}