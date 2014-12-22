namespace WebApi.Core.Application.Search.Filter
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Castle.Core.Internal;

    public class FilterClause : IFilterExpressionBuilder
    {
        public FilterClause(string left, string right, string filterOperator, string logicalOperator)
        {
            Left = left;
            Right = right;
            FilterOperator = filterOperator;
            LogicalOperator = logicalOperator;
        }

        #region Properties

        public string Left { get; private set; }
        public string Right { get; private set; }
        public string FilterOperator { get; private set; }
        public string LogicalOperator { get; private set; }

        public bool HasLogicalOperator
        {
            get { return LogicalOperator != null; }
        }

        #endregion

        public Expression ToExpression(PropertyInfo[] properties, ParameterExpression parameter)
        {
            var property = properties.FirstOrDefault(p => p.Name.Equals(Left, StringComparison.InvariantCultureIgnoreCase));
            if (property == null)
                throw new SearchException(string.Format("Poorly formed filter: '{0}' is not a known property", Left));
            var left = Expression.Property(parameter, property.Name);
            FilterOperator filterOperator;
            if (!TryParseOperator(FilterOperator, out filterOperator))
                throw new SearchException(string.Format("Poorly formed filter: '{0}' is an invalid operator", FilterOperator));
            var @operator = filterOperator;
            var right = BuildRightExpression(property.PropertyType, Right);
            if (@operator == Filter.FilterOperator.IsEqual)
                return Expression.Equal(left, right);
            if (@operator == Filter.FilterOperator.IsNotEqual)
                return Expression.NotEqual(left, right);
            if (@operator == Filter.FilterOperator.IsLike)
                return BuildLikeExpression(left, right, property.PropertyType);
            return null;
        }

        private Expression BuildLikeExpression(MemberExpression left, ConstantExpression right, Type propertyType)
        {
            if (propertyType.Is<string>())
            {
                var containsMethod = propertyType.GetMethod("Contains", BindingFlags.Instance | BindingFlags.Public);
                return Expression.Call(left, containsMethod, right);
            }
            throw new SearchException("Poorly formed filter: the Like operator can only be used on string properties");
        }

        private ConstantExpression BuildRightExpression(Type type, string source)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            if (type.Is<string>())
            {
                if (source == null || source.Length < 2 || source[0] != '\'' || source[source.Length - 1] != '\'')
                    throw new SearchException(string.Format("Poorly formed filter: '{0}' cannot be parsed as a string", source));
                var value = source.Substring(1, source.Length - 2);
                return Expression.Constant(value, type);
            }
            if (type.Is<bool>() || type.Is<bool?>())
            {
                bool value;
                if (!bool.TryParse(source, out value))
                    throw new SearchException(string.Format("Poorly formed filter: '{0}' cannot be parsed as a boolean", source));
                return Expression.Constant(value, type);
            }
            if (type.Is<int>() || type.Is<int?>())
            {
                int value;
                if (!int.TryParse(source, out value))
                    throw new SearchException(string.Format("Poorly formed filter: '{0}' cannot be parsed as an integer", source));
                return Expression.Constant(value, type);
            }
            if (type.Is<Enum>())
            {
                var value = Enum.Parse(type, source);
                return Expression.Constant(value, type);
            }
            if (underlyingType.Is<Enum>())
            {
                var value = Enum.Parse(underlyingType, source);
                return Expression.Constant(value, type);
            }
            throw new SearchException(string.Format("Poorly formed filter: '{0}' is not a supported type", type.FullName));
        }
        
        private bool TryParseOperator(string operatorValue, out FilterOperator filterOperator)
        {
            switch (operatorValue)
            {
                case "eq":
                    filterOperator = Filter.FilterOperator.IsEqual;
                    return true;
                case "nq":
                    filterOperator = Filter.FilterOperator.IsNotEqual;
                    return true;
                case "lk":
                    filterOperator = Filter.FilterOperator.IsLike;
                    return true;
                default:
                    filterOperator = default(FilterOperator);
                    return false;
            }
        }
    }
}