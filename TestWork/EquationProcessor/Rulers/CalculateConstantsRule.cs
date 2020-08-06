using System;
using System.ComponentModel;
using Contracts.Operations;
using Contracts.Terms;

namespace EquationProcessor.Rulers
{
    public class CalculateConstantsRule : IRule
    {
        public bool TermMatchRule(EquationTermBase term)
        {
            bool matched =
                term is EquationBinaryOperationBase operation &&
                operation.Left is EquationConstant &&
                operation.Right is EquationConstant;

            return matched;
        }

        public EquationTermBase Invoke(EquationTermBase term)
        {
            var operation = (EquationBinaryOperationBase) term;

            decimal value = operation.Type switch
            {
                OperationsEnum.Addition       => Left(operation) + Right(operation),
                OperationsEnum.Subtraction    => Left(operation) - Right(operation),
                OperationsEnum.Multiplication => Left(operation) * Right(operation),
                OperationsEnum.Division       => Left(operation) / Right(operation),
                //теряется разрядность, в случае требования к большей точности - переработать
                OperationsEnum.Exponentiation => (decimal) Math.Pow((double) Left(operation), (double) Right(operation)),
                _                             => throw new InvalidEnumArgumentException(nameof(operation.Type))
            };
            
            return new EquationConstant(value);
        }

        private static decimal Left(EquationBinaryOperationBase operation) => Value(operation.Left);
        private static decimal Right(EquationBinaryOperationBase operation) => Value(operation.Right);
        private static decimal Value(EquationTermBase term) =>
            term is EquationConstant constant ? constant.Value : throw new ArgumentException("Unsupported type");
    }
}