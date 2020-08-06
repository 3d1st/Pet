using System;

namespace Contracts.Exceptions
{
    public abstract class ParsingExceptionBase : Exception
    {
        protected ParsingExceptionBase(string message) : base(message: message)
        {
        }
    }
}