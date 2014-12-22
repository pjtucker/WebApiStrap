namespace WebApi.Core.Domain.Validation
{
    using System;
    using System.Linq.Expressions;

    public class InvalidOptionField<TObject, TProperty> : InvalidField<TObject, TProperty>
    {
        public bool HasSpecifiedValidOptions
        {
            get { return ValidOptions != null && ValidOptions.Length > 0; }
        }

        public TProperty[] ValidOptions { get; private set; }

        public InvalidOptionField(Expression<Func<TObject, TProperty>> property, TProperty[] validOptions = null) : base(property, InvalidFieldType.InvalidOption)
        {
            ValidOptions = validOptions;
        }

        public override string Message
        {
            get { return HasSpecifiedValidOptions ? string.Format("{0} must be one of the following: .{1}", string.Join(", ", ValidOptions)) : string.Format("{0} is an invalid option.{1}", Name); }
        }
    }
}