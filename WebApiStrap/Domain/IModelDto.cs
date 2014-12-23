namespace WebApiStrap.Domain
{
    public interface IModelDto<out TModel>
    {
        TModel ToModel();
    }
}