using Contracts.Operations;
using Contracts.Terms;
using static Utils.EquationTreeHelper;

namespace EquationProcessor.Rulers
{
    public class ExchangeVariableAndConstantInMultiplicationTermRule : IRule
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

            EquationTermBase result;
            
            if (MatchShiftRight(operation))
            {
                var leftOperation = (EquationBinaryOperationBase) operation.Left;
            
                decimal multiplier = ((EquationConstant) leftOperation.Left).Value;

                leftOperation.Left = CreateConstant(multiplier + decimal.One);

                result = leftOperation;
            }
            else
            {
                var rightOperation = (EquationBinaryOperationBase) operation.Right;
                
                decimal multiplier = ((EquationConstant) rightOperation.Left).Value;
                rightOperation.Left = CreateConstant(multiplier + decimal.One);

                result = rightOperation;
            }

            return result;
        }
        
        private static bool MatchShiftRight(EquationBinaryOperationBase operation) =>
            operation.Type == OperationsEnum.Addition &&
            operation.Left is EquationBinaryOperationBase leftOperation &&
            leftOperation.Type == OperationsEnum.Multiplication &&
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