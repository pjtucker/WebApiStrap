namespace WebApiStrap.Domain.Validation
{
    using System;
    using System.Linq.Expressions;

    public class InvalidFormatField<TObject, TProperty> : InvalidField<TObject, TProperty>
    {
        private readonly string _message;
        public Enum ExpectedFormat { get; private set; }

        public InvalidFormatField(Expression<Func<TObject, TProperty>> property, Enum expectedFormat, string message = null) : base(property, InvalidFieldType.InvalidFormat)
        {
            ExpectedFormat = expectedFormat;
            _message = message;
        }

        public override string Message
        {
            get { return _message ?? string.Format("The format of {0} is in an unsupported format.", Name); }
        }
    }
}