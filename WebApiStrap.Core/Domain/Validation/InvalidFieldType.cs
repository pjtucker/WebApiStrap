namespace WebApi.Core.Domain.Validation
{
    public enum InvalidFieldType
    {
        None = 0,
        Required = 1,
        OutOfRange = 2,
        InvalidFormat = 3,
        ExceededMaximumLength = 4,
        InvalidOption = 5,
        BusinessRuleViolation = 6
    }
}