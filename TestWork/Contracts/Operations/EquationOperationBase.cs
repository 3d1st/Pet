using Contracts.Terms;

namespace Contracts.Operations
{
    public abstract class EquationOperationBase : EquationTermBase
    {
        public OperationsEnum Type { get; }
        
        protected EquationOperationBase(OperationsEnum type)
        {
            this.Type = type;
        }
    }
}