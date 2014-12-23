namespace WebApiStrap.Infrastructure.Data
{
    using System.Data;
    using System.Data.Common;
    using System.Data.Entity;

    public abstract class EntityFrameworkContext : DbContext, IDbContext
    {
        private DbTransaction _transaction;

        protected EntityFrameworkContext(string connectionString) : base(connectionString)
        {
        }

        public void BeginTransaction()
        {
            if (Database.Connection.State != ConnectionState.Open)
                Database.Connection.Open();
            _transaction = Database.Connection.BeginTransaction();
            Database.UseTransaction(_transaction);
        }

        public void CommitTransaction()
        {
            _transaction.Commit();
            BeginTransaction();
        }

        public void RollbackTransaction()
        {
            _transaction.Rollback();
        }

        protected override void Dispose(bool disposing)
        {
            if (_transaction != null)
                _transaction.Dispose();
            base.Dispose(disposing);
        }
    }
}