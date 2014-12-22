namespace WebApi.Core.Domain.Validation.Interfaces
{
    public interface IOutOfRangeInvalidField<out TProperty> : IInvalidField
    {
        TProperty Minumum { get; }
        TProperty Maximum { get; }
        TProperty EnteredValue { get; }
    }
}