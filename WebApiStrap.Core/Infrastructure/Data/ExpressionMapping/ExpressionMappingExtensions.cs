namespace WebApi.Core.Infrastructure.Data.ExpressionMapping
{
    using System;
    using System.Linq.Expressions;

    public static class ExpressionMappingExtensions
    {
        public static ExpressionMapping<TModel, TEntity> AddMapping<TModel, TEntity, TValue>(this ExpressionMapping<TModel, TEntity> mapping,
            Expression<Func<TModel, TValue>> fromExpression, Expression<Func<TEntity, TValue>> toExpression)
        {
            mapping.Add(fromExpression, toExpression);
            return mapping;
        }
    }
}