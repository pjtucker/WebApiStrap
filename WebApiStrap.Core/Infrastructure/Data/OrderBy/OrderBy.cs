namespace WebApi.Core.Infrastructure.Data.OrderBy
{
    using System.Collections.Generic;

    public class OrderBy
    {
        public OrderBy()
        {
            Clauses = new List<OrderByClause>();
        }

        public List<OrderByClause> Clauses { get; private set; }

        public bool HasClauses
        {
            get { return Clauses != null && Clauses.Count > 0; }
        }

        public void AddClause(OrderByClause orderByClause)
        {
            Clauses.Add(orderByClause);
        }
    }
}