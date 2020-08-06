namespace Contracts.Exceptions
{
    public class EqualSignNotSingleException : ParsingExceptionBase
    {
        public EqualSignNotSingleException() : base("Equal sign not single in input text")
        {
            
        }
    }
}