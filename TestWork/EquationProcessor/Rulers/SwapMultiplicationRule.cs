using Contracts.Operations;
using Contracts.Terms;

namespace EquationProcessor.Rulers
{
    public class SwapMultiplicationRule : SwapTermBase
    {
        public override bool TermMatchRule(EquationTermBase term)
        {
            bool matched = term is EquationBinaryOperationBase operation &&
                           operation.Type == OperationsEnum.Multiplication &&
                           operation.Left is EquationVariable &&
                           operation.Right is EquationConstant;
            return matched;
        }
    }
}