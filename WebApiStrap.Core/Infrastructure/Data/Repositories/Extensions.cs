namespace WebApi.Core.Infrastructure.Data.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Domain;

    public static class Extensions
    {
        public static TEntity Load<TModel, TEntity>(this TEntity entity, TModel model)
            where TModel : Model
            where TEntity : class, IEntity
        {
            if (entity == null || model == null)
                return null;
            return Mapper.Map(model, entity);
        }

        public static TEntity ToEntity<TModel, TEntity>(this TModel model)
            where TModel : Model
            where TEntity : class, IEntity, new()
        {
            if (model == null)
                return null;
            var entity = new TEntity();
            return Load(entity, model);
        }

        public static TModel ToModel<TModel, TModelDto, TEntity>(this TEntity entity)
            where TModel : Model
            where TModelDto : IModelDto<TModel>
            where TEntity : class, IEntity
        {
            if (entity == null)
                return null;
            var modelDto = Mapper.Map<TModelDto>(entity);
            return modelDto.ToModel();
        }

        public static IEnumerable<TModel> ToModels<TModel, TModelDto>(this IEnumerable<TModelDto> modelDtoCollection)
            where TModel : Model
            where TModelDto : IModelDto<TModel>
        {
            return modelDtoCollection == null ? null : modelDtoCollection.Select(d => d.ToModel());
        }

        public static IQueryable<TModelDto> ToSelection<TModelDto, TEntity>(this IQueryable<TEntity> entities)
            where TModelDto : IModelDto<Model>
            where TEntity : IEntity
        {
            return entities.Project().To<TModelDto>();
        }
    }
}