namespace WebApiStrap.Application.Filters
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Filters;

    public class HandledExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response = actionExecutedContext.Request.CreateHandledExceptionResponse(actionExecutedContext.Exception);
        }

        public override Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            actionExecutedContext.Response = actionExecutedContext.Request.CreateHandledExceptionResponse(actionExecutedContext.Exception);
            return base.OnExceptionAsync(actionExecutedContext, cancellationToken);
        }
    }
}
