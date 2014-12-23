namespace WebApiStrap.Infrastructure.Data
{
    using System;

    public interface IDbContext : IDisposable
    {
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        int SaveChanges();
    }
}