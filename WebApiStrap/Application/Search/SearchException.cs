namespace WebApiStrap.Application.Search
{
    using System;

    public class SearchException : Exception
    {
        public SearchException(string message) : base(message)
        {
        }
    }
}