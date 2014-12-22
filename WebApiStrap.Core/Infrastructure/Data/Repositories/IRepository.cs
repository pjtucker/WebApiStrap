namespace WebApi.Core.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Domain;
    using OrderBy;
    using Searching;

    public interface IRepository<TModel> where TModel : Model
    {
        TModel Add(TModel model);
        bool Any(Expression<Func<TModel, bool>> filter = null);
        int Count(Expression<Func<TModel, bool>> filter);
        TModel Find(object id);
        IEnumerable<TModel> Find(Expression<Func<TModel, bool>> filter = null);
        void Remove(object id);
        TModel Update(object id, TModel model);
        SearchResultSet<TModel> Search(Expression<Func<TModel, bool>> filter = null, OrderBy orderBy = null, Pagination pagination = null);
    }
}