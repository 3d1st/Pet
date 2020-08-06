using System;
using Contracts.Terms;

namespace Utils
{
    public static class EquationTreeHelper
    {
        public static EquationTermBase CreateZero() => CreateConstant(decimal.Zero);
        public static EquationTermBase CreateOne() => CreateConstant(decimal.One);

        public static EquationTermBase CreateConstant(decimal value) => new EquationConstant(value);
        
        public static bool IsZero(this EquationTermBase term) =>
            term is EquationConstant constant &&
            constant.Value == decimal.Zero;
    }
}