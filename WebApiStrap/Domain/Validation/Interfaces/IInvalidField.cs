﻿namespace WebApiStrap.Domain.Validation.Interfaces
{
    public interface IInvalidField
    {
        string Name { get; }
        InvalidFieldType Reason { get; }
        string Message { get; }
    }
}