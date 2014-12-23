namespace WebApiStrap.Infrastructure.Data.ExpressionMapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public sealed class ExpressionMapper<TSource, TDestination> : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> _parameters = new Dictionary<ParameterExpression, ParameterExpression>();
        private readonly Dictionary<Tuple<int, Module>, LambdaExpression> _mappings;

        public ExpressionMapper(Dictionary<Tuple<int, Module>, LambdaExpression> mappings)
        {
            _mappings = mappings;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node.Type == typeof(TSource))
            {
                ParameterExpression parameter;
                if (!_parameters.TryGetValue(node, out parameter))
                {
                    _parameters.Add(node, parameter = Expression.Parameter(typeof(TDestination), node.Name));
                }
                return parameter;
            }
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression == null)
            {
                return base.VisitMember(node);
            }

            Expression rootExpression = GetRootOfMemberExpression(node);

            // If the root expression is of the source type but the expression type is Constant, that means we
            // are trying to extract the value of a property of an instance of the source type (i.e., the lambda
            // was constructed inside an instance method of the source type and refers to a member variable).
            // In this case, we don't want to map the expression, just get the constant value out, so call base.
            if (rootExpression.Type != typeof(TSource) || rootExpression.NodeType == ExpressionType.Constant)
            {
                return base.VisitMember(node);
            }

            var expression = Visit(rootExpression);
            if (expression.Type != typeof(TDestination))
            {
                throw new Exception("Types don't match");
            }

            LambdaExpression lambdaExpression;
            if (_mappings != null && _mappings.TryGetValue(Tuple.Create(node.Member.MetadataToken, node.Member.Module), out lambdaExpression))
                return new ExpressionReplacer(lambdaExpression.Parameters.Single(), expression).Visit(lambdaExpression.Body);

            throw new InvalidOperationException("Unmapped member: " + node.Member);
        }

        private Expression GetRootOfMemberExpression(MemberExpression node)
        {
            MemberExpression parent = node;

            while (parent.Expression is MemberExpression)
            {
                parent = (MemberExpression) parent.Expression;
            }

            return parent.Expression;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return Expression.Lambda(
                Visit(node.Body),
                node.Parameters.Select(Visit).Cast<ParameterExpression>()
                );
        }

        internal sealed class ExpressionReplacer : ExpressionVisitor
        {
            private readonly Expression _replacement;
            private readonly Expression _toFind;

            public ExpressionReplacer(Expression toFind, Expression replacement)
            {
                _toFind = toFind;
                _replacement = replacement;
            }

            public override Expression Visit(Expression node)
            {
                return node == _toFind ? _replacement : base.Visit(node);
            }
        }
    }
}