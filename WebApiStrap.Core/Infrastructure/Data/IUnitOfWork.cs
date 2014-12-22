namespace WebApi.Core.Infrastructure.Data
{
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
    }
}