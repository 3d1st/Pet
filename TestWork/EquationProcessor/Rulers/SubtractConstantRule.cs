using System;
using Contracts.Operations;
using Contracts.Terms;

namespace EquationProcessor.Rulers
{
    public class SubtractConstantRule : IRule

    {
        public bool TermMatchRule(EquationTermBase term)
        {
            return term is EquationBinaryOperationBase operation &&
                   operation.Type == OperationsEnum.Subtraction &&
                   operation.Right is EquationConstant;
        }

        public EquationTermBase Invoke(EquationTermBase term)
        {
            var operation = (EquationBinaryOperationBase) term;
            decimal constantValue = ((EquationConstant) operation.Right).Value;
            
            return new AdditionOperation(operation.Left, new EquationConstant(constantValue * decimal.MinusOne));
        }
    }
}