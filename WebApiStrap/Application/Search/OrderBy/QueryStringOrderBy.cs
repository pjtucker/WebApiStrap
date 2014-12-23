namespace WebApiStrap.Application.Search.OrderBy
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Infrastructure.Data.ExpressionMapping;
    using Infrastructure.Data.OrderBy;

    [TypeConverter(typeof(QueryStringOrderByTypeConverter))]
    public class QueryStringOrderBy<T>
    {
        private readonly string _order;
        private readonly Type _type;
        private readonly PropertyInfo[] _properties;

        public QueryStringOrderBy(string order)
        {
            _order = order;
            _type = typeof(T);
            _properties = _type.GetProperties();
        }

        public OrderBy ToModel<TSource, TDestination>(ExpressionMapping<TSource, TDestination> expressionMapping)
        {
            var parameter = Expression.Parameter(_type, _type.Name.ToLower());
            OrderByClauseCollection byClauses;
            if (!OrderByClauseCollection.TryParse(_order, out byClauses))
                throw new SearchException("Failed to parse order");
            if (byClauses == null || byClauses.Count < 1)
                return null;
            var order = new OrderBy();
            foreach (var clause in byClauses)
            {
                var propertyToMatch = clause.Property;
                var matchedProperty = _properties.FirstOrDefault(p => p.Name.Equals(propertyToMatch, StringComparison.InvariantCultureIgnoreCase));
                if (matchedProperty == null)
                    throw new SearchException(string.Format("Poorly formed order by clause: '{0}' is not a known property", propertyToMatch));
                var sourceBody = Expression.Property(parameter, matchedProperty);
                var sourceExpression = Expression.Lambda(sourceBody, new[] { parameter });
                var destinationExpression = expressionMapping.GetKeySelector(sourceExpression);
                order.AddClause(new Infrastructure.Data.OrderBy.OrderByClause((dynamic) destinationExpression, clause.Direction));
            }
            return order;
        }
    }
}