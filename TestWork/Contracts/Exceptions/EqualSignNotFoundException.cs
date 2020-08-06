namespace Contracts.Exceptions
{
    public class EqualSignNotFoundException : ParsingExceptionBase
    {
        public EqualSignNotFoundException() : base("Equal sign not found in input text")
        {
        }
    }
}