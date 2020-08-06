using Contracts.Terms;

namespace EquationProcessor
{
    public interface IRule
    {
        bool TermMatchRule(EquationTermBase term);

        EquationTermBase Invoke(EquationTermBase term);
    }
}