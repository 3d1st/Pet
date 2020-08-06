using Contracts.Operations;

namespace Contracts
{
    public interface IEquationParser
    {
        (EquationBinaryOperationBase left, EquationBinaryOperationBase right) ParseEquation(string input);
        
        EquationBinaryOperationBase Parse(string input);
    }
}