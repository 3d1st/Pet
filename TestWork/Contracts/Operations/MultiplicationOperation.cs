using Contracts.Terms;

namespace Contracts.Operations
{
    public class MultiplicationOperation : EquationBinaryOperationBase
    {
        public MultiplicationOperation(EquationTermBase left, EquationTermBase right) : base(OperationsEnum.Multiplication, left, right)
        {
        }
    }
}