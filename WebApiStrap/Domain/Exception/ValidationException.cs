namespace WebApiStrap.Domain.Exception
{
    using System;
    using System.Collections.Generic;
    using Validation.Interfaces;

    public class ValidationException : Exception
    {
        public ValidationException(IReadOnlyList<IInvalidField> invalidFields) : base("Validation exception")
        {
            InvalidFields = invalidFields;
        }

        public IReadOnlyList<IInvalidField> InvalidFields { get; private set; }
    }
}