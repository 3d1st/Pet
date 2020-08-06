using Contracts.Terms;

namespace Contracts
{
    public interface IEquationProcessor
    {
        string Display(EquationTermBase term);

        EquationTermBase Simplify(EquationTermBase term);
    }
}