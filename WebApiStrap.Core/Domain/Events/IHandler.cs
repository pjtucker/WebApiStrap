namespace WebApi.Core.Domain.Events
{
    public interface IHandler<in T> where T : IDomainEvent
    {
        void Handle(T args); 
    }
}