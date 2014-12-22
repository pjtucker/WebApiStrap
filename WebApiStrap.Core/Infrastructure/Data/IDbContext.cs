namespace WebApi.Core.Infrastructure.Data
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