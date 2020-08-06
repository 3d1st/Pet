namespace Contracts.Exceptions
{
    public class UnbalancedBracketsException : ParsingExceptionBase
    {
        public UnbalancedBracketsException() : base("Unbalanced brackets")
        {
        }
    }
}