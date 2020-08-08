using Contracts.Terms;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace TestWorkTest.EquationProcessor
{
    public class EquationProcessorTest : IClassFixture<EquationProcessorFixture>
    {
        private EquationProcessorFixture Fixture { get; }
        
        public EquationProcessorTest(EquationProcessorFixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void EquationProcessorDisplaySimpleTerm_ShouldSucceed()
        {
            // Arrange
            const string input = "a+42(v+d^2)";
            var term = this.Fixture.Parser.Parse(input);
            string display;
            
            // Act
            display = this.Fixture.Processor.Display(term);

            // Assert
            // @formatter:off
            using (new AssertionScope())
            {
                display.Should()
                    .NotBeNullOrWhiteSpace().And
                    .Be("a+42*(v+d^2)");
            }
            // @formatter:on
        }

        [Theory]
        [InlineData("0 + x", "x")]
        [InlineData("0x", "0")]
        [InlineData("x^0-1", "0")]
        [InlineData("5 + x", "x+5")]
        [InlineData("x * 5", "5*x")]        
        [InlineData("x + 1 + 1", "x+2")]
        [InlineData("1 + x + 1 + 1", "x+3")]
        [InlineData("1 + x - 1 + 1", "x+1")]
        [InlineData("x-1+3", "x+2")]
        [InlineData("k5", "5*k")]
        [InlineData("2 + x -4 + 2", "x")]
        [InlineData("x + x", "2*x")]
        [InlineData("x - x", "0")]
        [InlineData("x + 2 + x", "2*x+2")]
        [InlineData("x+2-x", "2")]
        [InlineData("2x + x", "3*x")]
        [InlineData("x + 2x", "3*x")]
        [InlineData("y*2*x", "2*x*y")]
        [InlineData("2x + 2x", "4*x")]
        [InlineData("2xy + 2xy", "4*x*y")]
        [InlineData("2(x + y)", "2*x+2*y")]
        //[InlineData("x^2 + 3,5xy + y -y^2 + xy - y", "x^2 - y^2 + 4,5xy")]
        public void EquationProcessorSimplify_ShouldSucceed(string input, string expected)
        {
            // Arrange
            var term = this.Fixture.Parser.Parse(input);
            EquationTermBase simplified;
            string display;
            
            // Act
            simplified = this.Fixture.Processor.Simplify(term);
            display = this.Fixture.Processor.Display(simplified);

            // Assert
            // @formatter:off
            using (new AssertionScope())
            {
                display.Should()
                    .NotBeNullOrWhiteSpace().And
                    .Be(expected);
            }
            // @formatter:on
        }
            
    }
}