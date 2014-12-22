namespace WebApi.Core.Domain.Validation
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Interfaces;

    public abstract class InvalidField<TObject, TProperty> : IInvalidField
    {
        protected Expression<Func<TObject, TProperty>> Property;
        private string _fieldName;

        protected InvalidField(Expression<Func<TObject, TProperty>> property, InvalidFieldType reason)
        {
            Reason = reason;
            Property = property;
        }

        public string Name
        {
            get { return _fieldName ?? (_fieldName = GetPropertyName()); }
        }

        public InvalidFieldType Reason { get; private set; }
        public abstract string Message { get; }

        private string GetPropertyName()
        {
            var member = Property.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.",
                    Property.ToString()));

            var propertyInfo = member.Member as PropertyInfo;
            if (propertyInfo == null)
                throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property.",
                    Property.ToString()));

            return propertyInfo.Name;
        }
    }
}