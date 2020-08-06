using Contracts.Terms;

namespace Contracts.Operations
{
    public abstract class EquationBinaryOperationBase : EquationOperationBase
    {
        public EquationTermBase Left { get; set; }
        
        public EquationTermBase Right { get; set; }

        protected EquationBinaryOperationBase(OperationsEnum type, EquationTermBase left, EquationTermBase right) : base(type)
        {
            this.Left = left;
            this.Right = right;
        }
    }
}