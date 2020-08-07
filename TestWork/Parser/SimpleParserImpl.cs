using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeJam;
using CodeJam.Strings;
using Contracts;
using Contracts.Exceptions;
using Contracts.Operations;
using Contracts.Terms;
using static Utils.EquationTreeHelper;

namespace Parser
{
    public class SimpleParserImpl : IEquationParser
    {
        private const string EqualSign = "=";

        private const char BracketOpen = '(';

        private const char BracketClose = ')';

        private static readonly IReadOnlyDictionary<char, Func<EquationTermBase, EquationTermBase, EquationBinaryOperationBase>> _RegisteredOperations;
        private static readonly char[] _Operations;
        private static readonly string[] _StringOperations;

        static SimpleParserImpl()
        {
            _RegisteredOperations = new Dictionary<char, Func<EquationTermBase, EquationTermBase, EquationBinaryOperationBase>>
            {
                {'+', (l, r) => new AdditionOperation(l, r)},
                {'-', (l, r) => new SubtractionOperation(l, r)},
                {'*', (l, r) => new MultiplicationOperation(l, r)},
                {'/', (l, r) => new DivisionOperation(l, r)},
                {'^', (l, r) => new ExponentiationOperation(l, r)}
            };

            _Operations = _RegisteredOperations.Keys.ToArray();
            _StringOperations = Enumerable.Reverse(_Operations).Select(char.ToString).ToArray();
        }

        public (EquationTermBase left, EquationTermBase right) ParseEquation(string input)
        {
            Code.NotNullNorWhiteSpace(input, nameof(input));

            int equalSignPosition = input.IndexOf(EqualSign, startIndex: 0, StringComparison.InvariantCulture);

            if (equalSignPosition < 0)
            {
                throw new EqualSignNotFoundException();
            }

            int secondSignPosition = input.IndexOf(EqualSign, startIndex: equalSignPosition + 1, StringComparison.InvariantCulture);

            if (secondSignPosition >= 0)
            {
                throw new EqualSignNotSingleException();
            }

            string leftInput = input.Substring(0, equalSignPosition);

            int rightStartPosition = equalSignPosition + 1;

            string rightInput = input.Substring(rightStartPosition, input.Length - equalSignPosition - 1);

            var left = Parse(leftInput);
            var right = Parse(rightInput);

            return (left, right);
        }

        public EquationTermBase Parse(string input)
        {
            input = ClearInput(input);

            if (input.IsNullOrWhiteSpace())
            {
                throw new EmptyTermsException();
            }

            ValidateBrackets(input);

            string polish = ConvertToReversePolish(input);

            var result = ConvertFromPolishReverseString(polish);

            return result ?? CreateZero();
        }

        private static string ConvertToReversePolish(string input)
        {
            var output = new StringBuilder();
            var operations = new Stack<char>();

            for (var i = 0; i < input.Length; i++)
            {
                if (IsOperand(input[i]))
                {
                    while (!IsOperator(input[i]))
                    {
                        output.Append(char.ToString(input[i++]));

                        if (i == input.Length) break;
                    }

                    output.Append(" ");
                    i--;
                }

                if (!IsOperator(input[i])) continue;

                switch (input[i])
                {
                    case BracketOpen:
                        operations.Push(input[i]);
                        break;
                    case BracketClose:
                    {
                        char op = operations.Pop();
                        while (op != BracketOpen)
                        {
                            output
                                .Append(char.ToString(op))
                                .Append(" ");

                            op = operations.Pop();
                        }

                        break;
                    }
                    default:
                    {
                        if (operations.Count > 0)
                        {
                            if (GetPriority(input[i]) <= GetPriority(operations.Peek()))
                            {
                                output
                                    .Append(char.ToString(operations.Pop()))
                                    .Append(" ");
                            }
                        }

                        operations.Push(input[i]);
                        break;
                    }
                }
            }

            while (operations.Count > 0)
            {
                output
                    .Append(char.ToString(operations.Pop()))
                    .Append(" ");
            }

            return output.ToString();
        }

        private static byte GetPriority(char s)
        {
            return s switch
            {
                '(' => 0,
                ')' => 1,
                '+' => 2,
                '-' => 3,
                '*' => 4,
                '/' => 4,
                '^' => 5,
                _   => 6
            };
        }

        private static EquationTermBase ConvertFromPolishReverseString(string input)
        {
            var operands = new Stack<EquationTermBase>();
            var buffer = new List<char>();

            for (int i = 0; i < input.Length; i++)
            {
                if (IsOperand(input[i]))
                {
                    bool isDigit = char.IsDigit(input[i]);
                    while (!IsDelimiter(input[i]) && !IsOperator(input[i]))
                    {
                        buffer.Add(input[i++]);
                        if (i == input.Length) break;
                    }

                    var operandString = new string(buffer.ToArray());
                    buffer.Clear();
                    var operand = isDigit
                        ? (EquationTermBase) new EquationConstant(operandString)
                        : new EquationVariable(operandString);

                    operands.Push(operand);
                    i--;
                }
                else if (IsOperator(input[i]))
                {
                    if (operands.Count < 2)
                    {
                        throw new EmptyTermsException();
                    }

                    var right = operands.Pop();
                    var left = operands.Pop();

                    var operation = _RegisteredOperations[input[i]].Invoke(left, right);

                    operands.Push(operation);
                }
            }

            return operands.Peek();
        }

        private static bool IsOperator(char @char)
        {
            return @char == BracketOpen || @char == BracketClose || _Operations.Contains(@char);
        }

        private static bool IsDelimiter(char @char) => @char == ' ';

        private static bool IsOperand(char @char)
        {
            return char.IsDigit(@char) || char.IsLetter(@char);
        }

        // Для большей читабельности кода валидация сбалансированности вынесена в отдельный метод,
        // а не проводится в один проход с очисткой строки ввода 
        private static void ValidateBrackets(string input)
        {
            var balance = 0;

            foreach (char @char in input)
            {
                switch (@char)
                {
                    case BracketOpen:
                        balance++;
                        break;
                    case BracketClose:
                        balance--;
                        break;
                }
            }

            if (balance != 0)
            {
                throw new UnbalancedBracketsException();
            }
        }

        private static string ClearInput(string input)
        {
            var buffer = new List<char>();
            char? lastChar = null;

            // лишняя аллокация на Linq в данном случае не нужна
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (char currentChar in input)
            {
                if (currentChar == ' ')
                {
                    continue;
                }

                if (lastChar == null)
                {
                    lastChar = currentChar;

                    // устанавливаем 0 перед первой операцией вычитания для создания бинарной операции, вместо унарной
                    if (lastChar == '-')
                    {
                        buffer.Add('0');
                    }

                    buffer.Add(currentChar);
                    continue;
                }

                bool needInsertMultipleSign = NeedInsertMultipleSign(lastChar.Value, currentChar);

                if (needInsertMultipleSign)
                {
                    buffer.Add('*');
                }

                lastChar = currentChar;
                buffer.Add(currentChar);
            }

            return new string(buffer.ToArray());
        }

        private static bool NeedInsertMultipleSign(char previous, char current)
        {
            // @formatter:off
            return 
                // Previous                                             Current
                char.IsDigit (previous)                              && char.IsLetter(current)  ||                          // 2a
                char.IsLetter(previous)                              && char.IsDigit(current)   ||                          // a2
                char.IsLetter(previous)                              && char.IsLetter(current)  ||                          // ab
                (char.IsDigit (previous) || char.IsLetter(previous)) && current == BracketOpen  ||                          // 2( || a(
                previous == BracketClose                             && (char.IsDigit (current) || char.IsLetter(current)); // )2 || )a
            // @formatter:on
        }
    }
}