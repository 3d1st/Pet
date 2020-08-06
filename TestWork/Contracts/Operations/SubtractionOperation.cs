using Contracts.Terms;

namespace Contracts.Operations
{
    public class SubtractionOperation : EquationBinaryOperationBase
    {
        public SubtractionOperation(EquationTermBase left, EquationTermBase right) : base(OperationsEnum.Subtraction, left, right)
        {
        }
    }
}