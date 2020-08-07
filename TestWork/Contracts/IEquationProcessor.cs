using Contracts.Terms;

namespace Contracts
{
    public interface IEquationProcessor
    {
        string Display(EquationTermBase term);

        EquationTermBase SwapSides(EquationTermBase left, EquationTermBase right);
        
        EquationTermBase Simplify(EquationTermBase term);
    }
}