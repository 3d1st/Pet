namespace Contracts.Exceptions
{
    public class EmptyTermsException : ParsingExceptionBase
    {
        public EmptyTermsException() : base("Empty terms")
        {
        }
    }
}