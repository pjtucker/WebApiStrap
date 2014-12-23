﻿namespace WebApiStrap.Infrastructure.Data
{
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
    }
}