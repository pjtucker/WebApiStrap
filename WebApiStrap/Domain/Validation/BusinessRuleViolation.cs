namespace WebApiStrap.Domain.Validation
{
    using System;
    using System.Linq.Expressions;

    public class BusinessRuleViolation<TObject, TProperty> : InvalidField<TObject, TProperty>
    {
        private readonly string _message;

        public override string Message { get { return _message; } }

        public BusinessRuleViolation(Expression<Func<TObject, TProperty>> property, string message) : base(property, InvalidFieldType.BusinessRuleViolation)
        {
            _message = message;
        }
    }
}