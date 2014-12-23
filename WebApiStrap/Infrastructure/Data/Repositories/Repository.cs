namespace WebApiStrap.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using Domain;
    using Domain.Exception;
    using ExpressionMapping;
    using OrderBy;
    using Searching;

    public abstract class Repository<TModel, TModelDto, TEntity> : IRepository<TModel>
        where TModel : Model
        where TModelDto : IModelDto<TModel>
        where TEntity : class, IEntity, new()
    {
        private readonly IDbContext _context;

        protected Repository(IDbContext context)
        {
            _context = context;
        }

        protected abstract OrderBy DefaultOrderBy { get; }
        protected abstract IDbSet<TEntity> Entities { get; }
        protected abstract ExpressionMapping<TModel, TModelDto> ExpressionMapping { get; }

        public virtual TModel Add(TModel model)
        {
            var entityToAdd = model.ToEntity<TModel, TEntity>();
            var newEntity = Entities.Add(entityToAdd);
            _context.SaveChanges();
            return newEntity.ToModel<TModel, TModelDto, TEntity>();
        }

        public virtual bool Any(Expression<Func<TModel, bool>> filter = null)
        {
            var selection = Entities.ToSelection<TModelDto, TEntity>();
            var predicate = ExpressionMapping.GetPredicate(filter);
            return predicate == null ? Entities.Any() : selection.Any(predicate);
        }

        public virtual int Count(Expression<Func<TModel, bool>> filter)
        {
            var predicate = ExpressionMapping.GetPredicate(filter);
            return Find(predicate).Count();
        }

        protected int Count(Expression<Func<TModelDto, bool>> predicate)
        {
            return Find(predicate).Count();
        }

        public virtual TModel Find(object id)
        {
            var entity = Entities.Find(id);
            return entity.ToModel<TModel, TModelDto, TEntity>();
        }

        public virtual IEnumerable<TModel> Find(Expression<Func<TModel, bool>> filter = null)
        {
            var predicate = ExpressionMapping.GetPredicate(filter);
            return Find(predicate).ToModels<TModel, TModelDto>();
        }

        protected IQueryable<TModelDto> Find(Expression<Func<TModelDto, bool>> predicate)
        {
            var selection = Entities.ToSelection<TModelDto, TEntity>();
            return predicate == null ? selection : selection.Where(predicate);
        }

        public virtual void Remove(object id)
        {
            var entityToRemove = Entities.Find(id);
            if (entityToRemove == null)
                throw new HandledException(ModelExceptionReasons.NotFound, string.Format("Could not find id {0}", id));
            Entities.Remove(entityToRemove);
            _context.SaveChanges();
        }

        public SearchResultSet<TModel> Search(Expression<Func<TModel, bool>> filter = null, OrderBy orderBy = null, Pagination pagination = null)
        {
            var predicate = ExpressionMapping.GetPredicate(filter);
            var resultSet = Find(predicate);

            // Apply sorting and paging when applicable
            if (orderBy != null || pagination != null)
            {
                var sortedResults = Sort(resultSet, orderBy);
                if (pagination != null)
                    resultSet = sortedResults.Skip(pagination.RecordsToSkip).Take(pagination.PageSize);
                else
                    resultSet = sortedResults;
            }

            // Get the total count
            var count = Count(predicate);

            // Specify page and page size
            var page = 1;
            var pageSize = count;
            if (pagination != null)
            {
                page = pagination.Page;
                pageSize = pagination.PageSize;
            }

            // Convert results back to models
            var models = resultSet.ToModels<TModel, TModelDto>().ToArray();

            // Build the Search Results
            return new SearchResultSet<TModel>(models, count, new Pagination(page, pageSize));
        }

        protected IOrderedQueryable<TModelDto> Sort(IQueryable<TModelDto> source, OrderBy orderBy)
        {
            if (orderBy == null || !orderBy.HasClauses)
                orderBy = DefaultOrderBy;
            var results = Sort(source, orderBy.Clauses.First(), true);
            foreach (var orderByClause in orderBy.Clauses.Skip(1))
            {
                results = Sort(results, orderByClause);
            }
            return results;
        }

        protected IOrderedQueryable<TModelDto> Sort(IQueryable<TModelDto> source, OrderByClause orderByClause, bool isFirst = false)
        {
            var keySelector = ExpressionMapping.GetKeySelector(orderByClause.KeySelector);

            if (orderByClause.Direction == Direction.Ascending)
            {
                if (isFirst)
                    return Queryable.OrderBy(source, (dynamic) keySelector);
                return Queryable.ThenBy((IOrderedQueryable<TEntity>) source, (dynamic) keySelector);
            }

            if (orderByClause.Direction == Direction.Descending)
            {
                if (isFirst)
                    return Queryable.OrderByDescending(source, (dynamic) keySelector);
                return Queryable.ThenByDescending((IOrderedQueryable<TEntity>) source, (dynamic) keySelector);
            }
            throw new NotSupportedException(string.Format("Specified direction is not supported ({0}).", orderByClause.Direction));
        }

        public virtual TModel Update(object id, TModel model)
        {
            var entityToUpdate = Entities.Find(id);
            return Update(entityToUpdate, model);
        }

        protected TModel Update(TEntity entityToUpdate, TModel model)
        {
            if (entityToUpdate == null)
                throw new HandledException(ModelExceptionReasons.NotFound, "Could not find model");
            entityToUpdate.Load(model);
            _context.SaveChanges();
            return model;
        }
    }
}