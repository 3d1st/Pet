using Contracts;
using Contracts.Operations;
using Contracts.Terms;
using EquationProcessor.Rulers;

namespace EquationProcessor
{
    public partial class SimpleEquationProcessorImpl : IEquationProcessor
    {
        private const string BracketOpen = "(";

        private const string BracketClose = ")";

        private readonly IRule[] _rules =
        {
            new SubtractConstantRule(),
            new CalculateConstantsRule(),
            new ZeroProcessingRule(),
            new SwapAdditionOperationRule(),
            new SwapMultiplicationRule(),
            new ExchangeConstantInAdditionTerm()
        };

        public EquationTermBase Simplify(EquationTermBase term)
        {
            var simplified = false;

            do
            {
                foreach (var rule in _rules)
                {
                    simplified = TrySimplify(rule, term, out var simplifiedTerm);
                    if (!simplified) continue;
                    
                    term = simplifiedTerm;
                    break;
                }
            } while (simplified);
            
            return term;
        }

        private static bool TrySimplify(IRule rule, EquationTermBase term, out EquationTermBase simplified)
        {
            if (!(term is EquationBinaryOperationBase operation))
            {
                simplified = term;
                return false;
            }

            var termSimplified = false;
            
            bool leftSimplified = TrySimplify(rule, operation.Left, out var simplifiedLeft);

            if (leftSimplified)
            {
                operation.Left = simplifiedLeft;
            }

            bool rightSimplified = TrySimplify(rule, operation.Right, out var simplifiedRight);
            if (rightSimplified)
            {
                operation.Right = simplifiedRight;
            }

            termSimplified |= leftSimplified;
            termSimplified |= rightSimplified;

            if (rule.TermMatchRule(operation))
            {
                simplified = rule.Invoke(operation);
                return true;                
            }

            simplified = operation;
            return termSimplified;
        }
    }
}