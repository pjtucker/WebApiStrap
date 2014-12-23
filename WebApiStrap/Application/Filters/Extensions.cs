namespace WebApiStrap.Application.Filters
{
    using System;
    using System.Net;
    using System.Net.Http;
    using Domain.Exception;
    using Search;

    public static class Extensions
    {
        public static HttpResponseMessage CreateHandledExceptionResponse(this HttpRequestMessage request, Exception exception)
        {
            var baseExcpetion = exception.GetBaseException();
            var validationException = baseExcpetion as ValidationException;
            if (validationException != null)
                return request.CreateResponse(HttpStatusCode.BadRequest, validationException.InvalidFields);
            var searchException = baseExcpetion as SearchException;
            if (searchException != null)
                return request.CreateResponse(HttpStatusCode.BadRequest, searchException.Message);
            var handledException = baseExcpetion as HandledException;
            if (handledException != null)
                return request.CreateResponse(HttpStatusCode.InternalServerError, new { handledException.Message, handledException.Reason });
            return request.CreateDefaultHandledExceptionResponse();
        }

        private static HttpResponseMessage CreateDefaultHandledExceptionResponse(this HttpRequestMessage request)
        {
            return request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}