﻿using Contracts.Operations;
using Contracts.Terms;

namespace EquationProcessor.Rulers
{
    public class CalculateAdditionVariableRule : IRule
    {
        public bool TermMatchRule(EquationTermBase term)
        {
            bool matched =
                term is EquationBinaryOperationBase operation &&
                operation.Type == OperationsEnum.Addition &&
                operation.Left is EquationVariable left &&
                operation.Right is EquationVariable right &&
                left.Name == right.Name;

            return matched;
        }

        public EquationTermBase Invoke(EquationTermBase term)
        {
            char variableName = ((EquationVariable) ((EquationBinaryOperationBase) term).Left).Name;
                
            return new MultiplicationOperation(new EquationConstant(2), new EquationVariable(variableName));
        }
    }
}