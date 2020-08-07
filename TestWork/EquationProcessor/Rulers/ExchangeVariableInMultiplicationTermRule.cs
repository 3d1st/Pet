using Contracts.Operations;
using Contracts.Terms;

namespace EquationProcessor.Rulers
{
    public class ExchangeVariableInMultiplicationTermRule : IRule
    {
        public bool TermMatchRule(EquationTermBase term)
        {
            bool matched = term is EquationBinaryOperationBase operation &&
                           operation.Type == OperationsEnum.Multiplication &&
                           (
                               MatchShiftRight(operation) ||
                               MatchShiftLeft(operation)
                           );
            return matched;
        }

        public EquationTermBase Invoke(EquationTermBase term)
        {
            var operation = (EquationBinaryOperationBase)term;

            MultiplicationOperation result;
            
            if (MatchShiftRight(operation))
            {
                var leftOperation = (EquationBinaryOperationBase) operation.Left;
            
                result = new MultiplicationOperation(leftOperation.Left, new MultiplicationOperation(leftOperation.Right, operation.Right));
            }
            else
            {
                var rightOperation = (EquationBinaryOperationBase) operation.Right;
                result = new MultiplicationOperation(rightOperation.Left, new MultiplicationOperation(rightOperation.Right, operation.Left));
            }

            return result;
        }

        private static bool MatchShiftRight(EquationBinaryOperationBase operation) =>
            operation.Left is EquationBinaryOperationBase leftOperation &&
            leftOperation.Type == OperationsEnum.Multiplication &&
            leftOperation.Left is EquationConstant &&
            leftOperation.Right is EquationVariable &&
            operation.Right is EquationVariable;

        private static bool MatchShiftLeft(EquationBinaryOperationBase operation) =>
            operation.Right is EquationBinaryOperationBase rightOperation &&
            rightOperation.Type == OperationsEnum.Multiplication &&
            rightOperation.Left is EquationConstant &&
            rightOperation.Right is EquationVariable &&
            operation.Left is EquationVariable;
    }
}