using Contracts.Terms;

namespace Contracts.Operations
{
    public class ExponentiationOperation : EquationBinaryOperationBase
    {
        public ExponentiationOperation(EquationTermBase left, EquationTermBase right) : base(OperationsEnum.Exponentiation, left, right)
        {
        }
    }
}