namespace WebApiStrap.Infrastructure.Data.OrderBy
{
    using System;
    using System.Linq.Expressions;

    public static class Order<TSource>
    {
        public static OrderBy By<TKey>(Expression<Func<TSource, TKey>> keySelector, Direction direction = Direction.Ascending)
        {
            var orderBy = new OrderBy();
            var orderByClause = new OrderByClause(keySelector, direction);
            orderBy.AddClause(orderByClause);
            return orderBy;
        }

        public static OrderBy ByDescending<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            var orderBy = new OrderBy();
            var orderByClause = new OrderByClause(keySelector, Direction.Descending);
            orderBy.AddClause(orderByClause);
            return orderBy;
        }
    }
}