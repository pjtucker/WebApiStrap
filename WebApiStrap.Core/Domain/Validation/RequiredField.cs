namespace WebApi.Core.Domain.Validation
{
    using System;
    using System.Linq.Expressions;

    public class RequiredField<TObject, TProperty> : InvalidField<TObject, TProperty>
    {
        public RequiredField(Expression<Func<TObject, TProperty>> requiredField) : base(requiredField, InvalidFieldType.Required)
        {
        }

        public override string Message
        {
            get { return string.Format("{0} is required", Name); }
        }
    }
}