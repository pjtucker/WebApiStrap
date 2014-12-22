namespace WebApi.Core.Application.Search
{
    using System;

    public class SearchException : Exception
    {
        public SearchException(string message) : base(message)
        {
        }
    }
}