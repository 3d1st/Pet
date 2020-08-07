using Contracts.Operations;
using Contracts.Terms;

namespace Contracts
{
    public interface IEquationParser
    {
        (EquationTermBase left, EquationTermBase right) ParseEquation(string input);
        
        EquationTermBase Parse(string input);
    }
}