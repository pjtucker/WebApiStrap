namespace WebApi.Core.Application.Search.Filter
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text.RegularExpressions;

    public class FilterClauseCollection : IReadOnlyList<FilterClause>, IFilterExpressionBuilder
    {
        private const string Pattern = "[^\\s\"']+|\"([^\"]*)\"|'([^']*)'";
        private readonly IEnumerator<FilterClause> _enumerator;
        private readonly FilterClause[] _filterClauses;

        public FilterClauseCollection(string filter)
        {
            var matches = Regex.Matches(filter, Pattern);
            Count = (matches.Count + 1)/4;
            _filterClauses = new FilterClause[Count];
            for (var i = 0; i < Count; i++)
            {
                var index = i == 0 ? 0 : i*4;
                var left = GetGroupValue(matches, index);
                var right = GetGroupValue(matches, index + 2);
                var filterOperator = GetGroupValue(matches, index + 1);
                var logicalOperator = GetGroupValue(matches, index - 1);
                _filterClauses[i] = new FilterClause(left, right, filterOperator, logicalOperator);
            }
            _enumerator = ((IEnumerable<FilterClause>) _filterClauses).GetEnumerator();
        }

        public IEnumerator<FilterClause> GetEnumerator()
        {
            return _enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get; private set; }

        public FilterClause this[int index]
        {
            get { return _filterClauses[index]; }
        }

        public Expression ToExpression(PropertyInfo[] properties, ParameterExpression parameter)
        {
            Expression source = null;
            foreach (var filterClause in _filterClauses)
            {
                if ((source != null && !filterClause.HasLogicalOperator) || (source == null && filterClause.HasLogicalOperator))
                    throw new Exception("Invalid filter");
                var expression = filterClause.ToExpression(properties, parameter);
                if (filterClause.LogicalOperator == "and")
                    source = Expression.AndAlso(source, expression);
                else if (filterClause.LogicalOperator == "or")
                    source = Expression.AndAlso(source, expression);
                else
                    source = expression;
            }
            return source;
        }

        private string GetGroupValue(MatchCollection matches, int index)
        {
            if (matches == null || index >= matches.Count || index < 0)
                return null;
            var match = matches[index];
            if (match.Groups.Count < 0)
                return null;
            return match.Groups[0].Value;
        }
    }
}