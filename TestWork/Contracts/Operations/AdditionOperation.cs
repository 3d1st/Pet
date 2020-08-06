using Contracts.Terms;

namespace Contracts.Operations
{
    public class AdditionOperation : EquationBinaryOperationBase
    {
        public AdditionOperation(EquationTermBase left, EquationTermBase right) : base(OperationsEnum.Addition, left, right)
        {
        }
    }
}