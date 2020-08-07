using Contracts.Operations;
using Contracts.Terms;

namespace EquationProcessor.Rulers
{
    public class ExchangeMultipliersByNameRule : IRule
    {
        public bool TermMatchRule(EquationTermBase term)
        {
            bool matched = term is EquationBinaryOperationBase operation &&
                           operation.Type == OperationsEnum.Multiplication &&
                           operation.Left is EquationVariable left &&
                           operation.Right is EquationVariable right &&
                           left.Name.CompareTo(right.Name) > 0;

            return matched;
        }

        public EquationTermBase Invoke(EquationTermBase term)
        {
            var operation = (EquationBinaryOperationBase) term;

            var buffer = operation.Left;
            operation.Left = operation.Right;
            operation.Right = buffer;

            return operation;
        }
    }
}