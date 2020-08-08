using Contracts.Operations;
using Contracts.Terms;

namespace EquationProcessor.Rulers
{
    public class ExchangeVariableAndMultiplicationInAdditionRule : IRule
    {
        public bool TermMatchRule(EquationTermBase term)
        {
            bool matched = term is EquationBinaryOperationBase operation &&
                           operation.Type == OperationsEnum.Addition &&
                           (
                               MatchShiftRight(operation) ||
                               MatchShiftLeft(operation)
                           );
        }

        public EquationTermBase Invoke(EquationTermBase term)
        {
            throw new System.NotImplementedException();
        }
        
        private static bool MatchShiftRight(EquationBinaryOperationBase operation) =>
            operation.Left is EquationBinaryOperationBase leftOperation &&
            leftOperation.Type == OperationsEnum.Addition &&
            leftOperation.Left is EquationConstant &&
            leftOperation.Right is EquationVariable &&
            operation.Right is EquationVariable rightVariable &&
            leftOperation.Right is EquationVariable leftOperationVariable &&
            leftOperationVariable.Name == rightVariable.Name;

        private static bool MatchShiftLeft(EquationBinaryOperationBase operation) =>
            operation.Type == OperationsEnum.Addition &&
            operation.Left is EquationVariable leftVariable &&
            operation.Right is EquationBinaryOperationBase rightOperation &&
            rightOperation.Type == OperationsEnum.Multiplication &&
            rightOperation.Left is EquationConstant &&
            rightOperation.Right is EquationVariable rightOperationVariable &&
            leftVariable.Name == rightOperationVariable.Name;
    }
}