namespace WebApi.Core.Application
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Domain;
    using Infrastructure.Data;
    using Infrastructure.Data.Searching;

    public static class Extensions
    {
        public static TServiceModel ToServiceModel<TModel, TServiceModel>(this TModel model)
            where TModel : Model
            where TServiceModel : class, IServiceModel
        {
            return model == null ? null : Mapper.Map<TServiceModel>(model);
        }

        public static IEnumerable<TServiceModel> ToServiceModels<TModel, TServiceModel>(this IEnumerable<TModel> models)
            where TModel : Model
            where TServiceModel : class, IServiceModel
        {
            return models.Select(m => m.ToServiceModel<TModel, TServiceModel>());
        }

        public static SearchResultSet<TServiceModel> ToServiceModel<TModel, TServiceModel>(this SearchResultSet<TModel> searchResults)
            where TModel : Model
            where TServiceModel : class, IServiceModel
        {
            var resultSet = searchResults.ResultSet.ToServiceModels<TModel, TServiceModel>().ToArray();
            var total = searchResults.Total;
            var pagination = new Pagination(searchResults.Paging.CurrentPageNumber, searchResults.Paging.PageSize);
            return new SearchResultSet<TServiceModel>(resultSet, total, pagination);
        }

        public static TModel ToModel<TModel, TModelDto, TServiceModel>(this TServiceModel serviceModel)
            where TModel : Model
            where TModelDto : IModelDto<TModel>
            where TServiceModel : class, IServiceModel
        {
            if (serviceModel == null)
                return null;
            var modelDto = Mapper.Map<TModelDto>(serviceModel);
            return modelDto.ToModel();
        }

        public static IEnumerable<TModel> ToModels<TModel, TModelDto>(this IEnumerable<TModelDto> modelDtoCollection)
            where TModel : Model
            where TModelDto : IModelDto<TModel>
        {
            return modelDtoCollection.Select(d => d.ToModel());
        }
    }
}