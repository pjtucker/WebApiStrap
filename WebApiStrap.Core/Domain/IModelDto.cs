namespace WebApi.Core.Domain
{
    public interface IModelDto<out TModel>
    {
        TModel ToModel();
    }
}