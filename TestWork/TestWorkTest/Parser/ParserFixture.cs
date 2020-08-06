using Contracts;
using Parser;

namespace TestWorkTest.Parser
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ParserFixture
    {
        public IEquationParser Parser { get; } = new SimpleParserImpl();
    }
}