namespace WebApiStrap.Domain.Exception
{
    using System;

    public class HandledException : Exception
    {
        public HandledException(Enum reason, string message, Exception innerException = null) : base(message, innerException)
        {
            Reason = reason;
        }

        public Enum Reason { get; protected set; }
    }
}