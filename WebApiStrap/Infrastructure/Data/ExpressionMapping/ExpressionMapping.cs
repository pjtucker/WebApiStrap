namespace WebApiStrap.Infrastructure.Data.ExpressionMapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    public class ExpressionMapping<TSource, TDestination>
    {
        private readonly Dictionary<Tuple<int, Module>, LambdaExpression> _mappings = new Dictionary<Tuple<int, Module>, LambdaExpression>();
        
        public void Add<TValue>(Expression<Func<TSource, TValue>> fromExpression, Expression<Func<TDestination, TValue>> toExpression)
        {
            var expression = fromExpression.Body;
            if (expression is UnaryExpression)
                expression = ((UnaryExpression) expression).Operand;
            var memberExpression = expression as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException("Unable to cast source to MemberExpression.", "source");
            _mappings.AddMapping(memberExpression, toExpression);
        }

        public Expression<Func<TDestination, bool>> GetPredicate(Expression<Func<TSource, bool>> source)
        {
            // Convert the expression filter into a search predicate
            var predicate = (Expression<Func<TDestination, bool>>) new ExpressionMapper<TSource, TDestination>(_mappings).Visit(source);
            return predicate;
        }

        public object GetKeySelector(LambdaExpression source)
        {
            var mappedExpression = new ExpressionMapper<TSource, TDestination>(_mappings).Visit(source);
            var keySelector = Convert.ChangeType(mappedExpression, mappedExpression.GetType());
            return keySelector;
        }
    }
}