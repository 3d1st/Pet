using Contracts.Operations;
using Contracts.Terms;

namespace EquationProcessor.Rulers
{
    public class CalculateAdditionVariableRule : IRule
    {
        public bool TermMatchRule(EquationTermBase term)
        {
            bool matched =
                term is EquationBinaryOperationBase operation &&
                operation.Left is EquationVariable &&
                operation.Right is EquationVariable;

            return matched;
        }

        public EquationTermBase Invoke(EquationTermBase term)
        {
            throw new System.NotImplementedException();
        }
    }
}