using Contracts.Operations;
using Contracts.Terms;

namespace EquationProcessor.Rulers
{
    public class ExchangeVariableInAdditionTermRule : IRule
    {
        public bool TermMatchRule(EquationTermBase term)
        {
            bool matched = term is EquationBinaryOperationBase operation &&
                           operation.Type == OperationsEnum.Addition &&
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
            
                result = new AdditionOperation(leftOperation.Right, new AdditionOperation(leftOperation.Left, operation.Right));
            }
            else
            {
                var rightOperation = (EquationBinaryOperationBase) operation.Right;
                result = new AdditionOperation(rightOperation.Right, new AdditionOperation(rightOperation.Left, operation.Left));
            }

            return result;
        }

        private static bool MatchShiftRight(EquationBinaryOperationBase operation) =>
            operation.Left is EquationBinaryOperationBase leftOperation &&
            leftOperation.Type == OperationsEnum.Addition &&
            leftOperation.Left is EquationVariable &&
            leftOperation.Right is EquationConstant &&
            operation.Right is EquationVariable;

        private static bool MatchShiftLeft(EquationBinaryOperationBase operation) =>
            operation.Right is EquationBinaryOperationBase rightOperation &&
            rightOperation.Type == OperationsEnum.Addition &&
            rightOperation.Left is EquationVariable &&
            rightOperation.Right is EquationConstant &&
            operation.Left is EquationVariable;
    }
}