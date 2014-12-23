namespace WebApiStrap.Infrastructure.Data.ExpressionMapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class ExpressionMapperExtensions
    {
        public static Dictionary<MemberInfo, LambdaExpression> AddMapping<TSource, TDestination, TValue>(this Dictionary<MemberInfo, LambdaExpression> mappings,
            Expression<Func<TSource, TValue>> fromExpression,
            Expression<Func<TDestination, TValue>> toExpression)
        {
            var mapping = Tuple.Create(((MemberExpression) fromExpression.Body).Member, (LambdaExpression) toExpression);
            mappings.Add(mapping.Item1, mapping.Item2);
            return mappings;
        }

        public static Dictionary<MemberInfo, LambdaExpression> AddMapping(this Dictionary<MemberInfo, LambdaExpression> mappings,
            MemberExpression fromExpression, LambdaExpression toExpression)
        {
            var mapping = Tuple.Create(fromExpression.Member, toExpression);
            mappings.Add(mapping.Item1, mapping.Item2);
            return mappings;
        }

        public static Dictionary<Tuple<int, Module>, LambdaExpression> AddMapping(this Dictionary<Tuple<int, Module>, LambdaExpression> mappings,
            MemberExpression fromExpression, LambdaExpression toExpression)
        {
            var key = Tuple.Create(fromExpression.Member.MetadataToken, fromExpression.Member.Module);
            mappings.Add(key, toExpression);
            return mappings;
        }
    }
}