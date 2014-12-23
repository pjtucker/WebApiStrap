namespace WebApiStrap.Domain.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    public class ValidationSummary
    {
        private readonly IInvalidField[] _invalidFields;

        public ValidationSummary(IEnumerable<IInvalidField> invalidFields = null)
        {
            _invalidFields = invalidFields == null ? new IInvalidField[0] : invalidFields.ToArray();
        }

        public bool IsValid
        {
            get { return !_invalidFields.Any(); }
        }

        public IReadOnlyList<IInvalidField> InvalidFields
        {
            get { return Array.AsReadOnly(_invalidFields); }
        }
    }
}