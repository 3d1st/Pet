using Contracts.Operations;
using Contracts.Terms;

namespace EquationProcessor.Rulers
{
    public class ExchangeConstantInAdditionTerm : IRule
    {
        public bool TermMatchRule(EquationTermBase term)
        {
            bool matched = term is EquationBinaryOperationBase operation &&
                           (
                               MatchShiftRight(operation) ||
                               MatchShiftLeft(operation)
                           );
            return matched;
        }

        public EquationTermBase Invoke(EquationTermBase term)
        {
            var operation = (EquationBinaryOperationBase)term;

            AdditionOperation result;
            
            if (MatchShiftRight(operation))
            {
                var leftOperation = (EquationBinaryOperationBase) operation.Left;
            
                result = new AdditionOperation(leftOperation.Left, new AdditionOperation(leftOperation.Right, operation.Right));
            }
            else
            {
                var rightOperation = (EquationBinaryOperationBase) operation.Right;
                result = new AdditionOperation(rightOperation.Left, new AdditionOperation(rightOperation.Right, operation.Left));
            }

            return result;
        }

        private static bool MatchShiftRight(EquationBinaryOperationBase operation) =>
            operation.Left is EquationBinaryOperationBase leftOperation &&
            leftOperation.Type == OperationsEnum.Addition &&
            leftOperation.Left is EquationVariable &&
            leftOperation.Right is EquationConstant &&
            operation.Right is EquationConstant;

        private static bool MatchShiftLeft(EquationBinaryOperationBase operation) =>
            operation.Right is EquationBinaryOperationBase leftOperation &&
            leftOperation.Type == OperationsEnum.Addition &&
            leftOperation.Left is EquationVariable &&
            leftOperation.Right is EquationConstant &&
            operation.Left is EquationConstant;
    }
}