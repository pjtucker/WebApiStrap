namespace WebApi.Core.Infrastructure.Data.OrderBy
{
    using System;
    using System.Linq.Expressions;

    internal static class OrderByExtensions
    {
        public static OrderBy ThenBy<TSource, TKey>(this OrderBy orderBy, Expression<Func<TSource, TKey>> keySelector,
            Direction direction = Direction.Ascending)
        {
            var orderByClause = new OrderByClause(keySelector, direction);
            orderBy.AddClause(orderByClause);
            return orderBy;
        }

        public static OrderBy ThenByDescending<TSource, TKey>(this OrderBy orderBy, Expression<Func<TSource, TKey>> keySelector)
        {
            var orderByClause = new OrderByClause(keySelector, Direction.Descending);
            orderBy.AddClause(orderByClause);
            return orderBy;
        }
    }
}