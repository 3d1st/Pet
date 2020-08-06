using Contracts;
using EquationProcessor;
using Parser;

namespace TestWorkTest.EquationProcessor
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EquationProcessorFixture
    {
        public IEquationParser Parser { get; } = new SimpleParserImpl();
        public IEquationProcessor Processor { get; } = new SimpleEquationProcessorImpl();
    }
}