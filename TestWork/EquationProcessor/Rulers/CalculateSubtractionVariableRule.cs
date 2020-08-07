using Contracts.Operations;
using Contracts.Terms;
using static Utils.EquationTreeHelper;

namespace EquationProcessor.Rulers
{
    public class CalculateSubtractionVariableRule : IRule
    {
        public bool TermMatchRule(EquationTermBase term)
        {
            bool matched =
                term is EquationBinaryOperationBase operation &&
                operation.Type == OperationsEnum.Subtraction &&
                operation.Left is EquationVariable left &&
                operation.Right is EquationVariable right &&
                left.Name == right.Name;

            return matched;
        }

        public EquationTermBase Invoke(EquationTermBase term)
        {
            return CreateZero();
        }
    }
}