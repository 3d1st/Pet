using Contracts.Terms;

namespace Contracts.Operations
{
    public class DivisionOperation : EquationBinaryOperationBase
    {
        public DivisionOperation(EquationTermBase left, EquationTermBase right) : base(OperationsEnum.Division, left, right)
        {
        }
    }
}