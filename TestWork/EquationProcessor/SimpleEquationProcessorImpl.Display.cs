﻿using System;
using Contracts.Operations;
using Contracts.Terms;
using static Contracts.Operations.OperationsEnumHelper;

namespace EquationProcessor
{
    public partial class SimpleEquationProcessorImpl
    {
        public string Display(EquationTermBase term) =>
            term switch
            {
                EquationVariable variable                   => char.ToString(variable.Name),
                EquationConstant constant                   => constant.Value.ToString(),
                EquationBinaryOperationBase binaryOperation => DisplayBinaryOperation(binaryOperation),
                _                                           => throw new ArgumentException("Unsupported argument type", nameof(term))
            };

        private string DisplayBinaryOperation(EquationBinaryOperationBase operation)
        {
            switch (operation.Type)
            {
                case OperationsEnum.Addition: 
                case OperationsEnum.Subtraction:
                    return Display(operation.Left) + DisplayType(operation.Type) + Display(operation.Right);
                case OperationsEnum.Multiplication:
                case OperationsEnum.Division:
                case OperationsEnum.Exponentiation:
                    return (operation.Left is EquationBinaryOperationBase
                               ? BracketOpen + Display(operation.Left) + BracketClose
                               : Display(operation.Left)) +
                           DisplayType(operation.Type) +
                           (operation.Right is EquationBinaryOperationBase
                               ? BracketOpen + Display(operation.Right) + BracketClose
                               : Display(operation.Right));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}