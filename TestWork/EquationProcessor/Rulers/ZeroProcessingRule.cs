using System.ComponentModel;
using Contracts.Exceptions;
using Contracts.Operations;
using Contracts.Terms;
using static Utils.EquationTreeHelper;

namespace EquationProcessor.Rulers
{
    public class ZeroProcessingRule : IRule
    {
        public bool TermMatchRule(EquationTermBase term)
        {
            if (!(term is EquationBinaryOperationBase operation))
            {
                return false;
            }

            return operation.Right.IsZero() || operation.Left.IsZero();
        }

        public EquationTermBase Invoke(EquationTermBase term)
        {
            var operation = (EquationBinaryOperationBase) term;

            return operation.Type switch
            {
                OperationsEnum.Addition       => ProcessAddition(operation),
                OperationsEnum.Subtraction    => ProcessSubtraction(operation),
                OperationsEnum.Multiplication => ProcessMultiplication(operation),
                OperationsEnum.Division       => ProcessDivision(operation),
                OperationsEnum.Exponentiation => ProcessExponentiation(operation),
                _                             => throw new InvalidEnumArgumentException(nameof(operation.Type), (int) operation.Type, typeof(OperationsEnum))
            };
        }
        
        private static EquationTermBase ProcessAddition(EquationBinaryOperationBase operation) =>
            operation.Left.IsZero() ? operation.Right : operation.Left;

        private static EquationTermBase ProcessSubtraction(EquationBinaryOperationBase operation)
        {
            if (operation.Right.IsZero())
            {
                return operation.Left;
            }

            return new MultiplicationOperation(
                left: new EquationConstant(decimal.MinusOne),
                right: operation.Right
            );
        }

        // ReSharper disable once UnusedParameter.Local
        private static EquationTermBase ProcessMultiplication(EquationBinaryOperationBase operation) =>
            CreateZero();

        private static EquationTermBase ProcessDivision(EquationBinaryOperationBase operation)
        {
            if (operation.Right.IsZero())
            {
                throw new DevideByZeroInProcessingException();
            }

            return CreateZero();
        }

        private static EquationTermBase ProcessExponentiation(EquationBinaryOperationBase operation)
        {
            if (operation.Left.IsZero() && operation.Right.IsZero())
            {
                throw new AttemptToCalculateZeroAtZeroDegreeException();
            }

            return operation.Left.IsZero()
                ? CreateZero()
                : CreateOne();
        }
    }
}