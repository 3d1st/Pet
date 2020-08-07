using Contracts.Operations;
using Contracts.Terms;

namespace EquationProcessor.Rulers
{
    public class SwapAdditionOperationRule : SwapTermBase
    {
        public override bool TermMatchRule(EquationTermBase term)
        {
            bool matched =
                term is EquationBinaryOperationBase operation &&
                operation.Type == OperationsEnum.Addition &&
                operation.Left is EquationConstant &&
                (
                    operation.Right is EquationVariable ||
                    operation.Right is EquationBinaryOperationBase
                );

            return matched;
        }
    }
}