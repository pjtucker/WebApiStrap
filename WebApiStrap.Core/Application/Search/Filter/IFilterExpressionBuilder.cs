namespace WebApi.Core.Application.Search.Filter
{
    using System.Linq.Expressions;
    using System.Reflection;

    public interface IFilterExpressionBuilder
    {
        Expression ToExpression(PropertyInfo[] properties, ParameterExpression parameter);
    }
}