namespace WebApiStrap.Domain.Validation
{
    using System;
    using System.Linq.Expressions;

    public class ExceededMaximumLengthField<TObject, TProperty> : InvalidField<TObject, TProperty>
    {
        public int MaximumLength { get; private set; }

        public ExceededMaximumLengthField(Expression<Func<TObject, TProperty>> property, int maximumLength) : base(property, InvalidFieldType.ExceededMaximumLength)
        {
            MaximumLength = maximumLength;
        }

        public override string Message
        {
            get { return string.Format("{0} exceeds maximum length of {1}", Name, MaximumLength); }
        }
    }
}