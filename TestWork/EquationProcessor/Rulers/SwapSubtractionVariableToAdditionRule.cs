using Contracts.Operations;
using Contracts.Terms;
using static Utils.EquationTreeHelper;

namespace EquationProcessor.Rulers
{
    public class SwapSubtractionVariableToAdditionRule : IRule
    {
        public bool TermMatchRule(EquationTermBase term)
        {
            bool matched = term is EquationBinaryOperationBase operation &&
                           operation.Type == OperationsEnum.Subtraction &&
                           operation.Right is EquationVariable;

            return matched;
        }

        public EquationTermBase Invoke(EquationTermBase term)
        {
            var operation = (EquationBinaryOperationBase) term;

            return new AdditionOperation(
                left: operation.Left,
                right: new MultiplicationOperation(CreateConstant(decimal.MinusOne), operation.Right));
        }
    }
}