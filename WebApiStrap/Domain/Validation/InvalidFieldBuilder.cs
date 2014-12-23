namespace WebApiStrap.Domain.Validation
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    public abstract class InvalidFieldBuilder<T>
    {
        protected readonly T Source;
        protected readonly IList<IInvalidField> InvalidFields;

        protected InvalidFieldBuilder(T source)
        {
            InvalidFields = new List<IInvalidField>();
            Source = source;
        }
        
        public static implicit operator IInvalidField[](InvalidFieldBuilder<T> builder)
        {
            return builder.InvalidFields.ToArray();
        }
    }
}