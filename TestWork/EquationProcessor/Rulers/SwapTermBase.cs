using Contracts.Operations;
using Contracts.Terms;

namespace EquationProcessor.Rulers
{
    public abstract class SwapTermBase : IRule
    {
        public abstract bool TermMatchRule(EquationTermBase term);

        public EquationTermBase Invoke(EquationTermBase term)
        {
            var operation = (EquationBinaryOperationBase) term;
            var buffer = operation.Left;
            operation.Left = operation.Right;
            operation.Right = buffer;

            return term;
        }
    }
}