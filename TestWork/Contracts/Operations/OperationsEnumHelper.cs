using System.ComponentModel;

namespace Contracts.Operations
{
    public static class OperationsEnumHelper
    {
        public static string DisplayType(OperationsEnum @enum) =>
            @enum switch
            {
                OperationsEnum.Addition       => "+",
                OperationsEnum.Subtraction    => "-",
                OperationsEnum.Multiplication => "*",
                OperationsEnum.Division       => "/",
                OperationsEnum.Exponentiation => "^",
                _                             => throw new InvalidEnumArgumentException(nameof(@enum), (int) @enum, typeof(OperationsEnum))
            };
    }
}