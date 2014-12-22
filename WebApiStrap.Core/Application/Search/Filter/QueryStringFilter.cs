namespace WebApi.Core.Application.Search.Filter
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Reflection;
    using Infrastructure.Data.ExpressionMapping;

    [TypeConverter(typeof (QueryStringFilterTypeConverter))]
    public class QueryStringFilter<T>
    {
        private readonly string _filter;
        private readonly Type _type;
        private readonly PropertyInfo[] _properties;

        public QueryStringFilter(string filter)
        {
            _filter = filter;
            _type = typeof (T);
            _properties = _type.GetProperties();
        }

        public Expression<Func<TDestination, bool>> Convert<TSource, TDestination>(ExpressionMapping<TSource, TDestination> mapping)
        {
            var clauses = new FilterClauseCollection(_filter);
            var parameter = Expression.Parameter(_type, _type.Name.ToLower());
            var predicateBody = clauses.ToExpression(_properties, parameter);
            var expression = Expression.Lambda<Func<TSource, bool>>(predicateBody, new[] { parameter });
            return mapping.GetPredicate(expression);
        }
    }
}