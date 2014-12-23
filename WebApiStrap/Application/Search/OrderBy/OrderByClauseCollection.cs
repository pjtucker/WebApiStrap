namespace WebApiStrap.Application.Search.OrderBy
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Infrastructure.Data.OrderBy;

    public class OrderByClauseCollection : IReadOnlyList<OrderByClause>
    {
        private readonly OrderByClause[] _orderByClauses;
        private readonly IEnumerator<OrderByClause> _enumerator;

        public OrderByClauseCollection(OrderByClause[] orderByClauses)
        {
            _orderByClauses = orderByClauses;
            _enumerator = ((IEnumerable<OrderByClause>) _orderByClauses).GetEnumerator();
        }

        public IEnumerator<OrderByClause> GetEnumerator()
        {
            return _enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get { return _orderByClauses.Length; } }

        public OrderByClause this[int index]
        {
            get { return _orderByClauses[index]; }
        }

        public static bool TryParse(string s, out OrderByClauseCollection result)
        {
            var matches = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var clauses = new List<OrderByClause>();
            for (var index = 0; index < matches.Length; index++)
            {
                var value = matches[index];
                if (value == "then" || value == "asc" || value == "desc")
                    continue;
                var nextIndex = index + 1;
                Direction direction;
                if (nextIndex >= matches.Length || matches[nextIndex] == "asc" || matches[nextIndex] == "then")
                    direction = Direction.Ascending;
                else
                    direction = Direction.Descending;
                var clause = new OrderByClause(value, direction);
                clauses.Add(clause);
            }
            result = new OrderByClauseCollection(clauses.ToArray());
            return true;
        }
    }
}