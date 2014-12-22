namespace WebApi.Core.Domain.Validation
{
    using System;
    using System.Linq.Expressions;
    using Interfaces;

    public class OutOfRangeField<TObject, TProperty> : InvalidField<TObject, TProperty>, IOutOfRangeInvalidField<TProperty>
    {
        public TProperty Minumum { get; private set; }
        public TProperty Maximum { get; private set; }
        public TProperty EnteredValue { get; private set; }

        public OutOfRangeField(Expression<Func<TObject, TProperty>> property, TProperty enteredValue, TProperty minumum, TProperty maximum)
            : base(property, InvalidFieldType.OutOfRange)
        {
            EnteredValue = enteredValue;
            Minumum = minumum;
            Maximum = maximum;
        }

        public override string Message
        {
            get { return string.Format("{0} must be a value between {1} and {2}", Name, Minumum, Maximum); }
        }
    }
}