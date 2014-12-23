namespace WebApiStrap.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Infrastructure.Data;
    using Infrastructure.Data.OrderBy;
    using Infrastructure.Data.Repositories;
    using Infrastructure.Data.Searching;

    public abstract class Model
    {
        public static TModel Find<TModel>(IRepository<TModel> repository, object id) where TModel : Model
        {
            return repository.Find(id);
        }

        public static IEnumerable<TModel> Find<TModel>(IRepository<TModel> repository) where TModel : Model
        {
            return repository.Find();
        }

        public static IEnumerable<TModel> Find<TModel>(IRepository<TModel> repository, Expression<Func<TModel, bool>> filter) where TModel : Model
        {
            return repository.Find(filter);
        }

        public static SearchResultSet<TModel> Search<TModel>(IRepository<TModel> repository, Expression<Func<TModel, bool>> filter, OrderBy orderBy, Pagination pagination) where TModel : Model
        {
            return repository.Search(filter, orderBy, pagination);
        }
    }
}
