using System;
using System.Reflection.Emit;
using Contracts.Exceptions;
using Contracts.Operations;
using Contracts.Terms;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace TestWorkTest.Parser
{
    public class ParserTest : IClassFixture<ParserFixture>
    {
        private ParserFixture Fixture { get; }

        public ParserTest(ParserFixture fixture)
        {
            this.Fixture = fixture;
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void ParseEmptyInput_ShouldSucceed(string input)
        {
            // Arrange

            // Act
            Action act = () => _ = this.Fixture.Parser.ParseEquation(input);

            // Assert
            // @formatter:off
            using (new AssertionScope())
            {
                act.Should().Throw<ArgumentException>();
            }
            // @formatter:on
        }

        [Theory]
        [InlineData("a", "Equal sign not found in input text")]
        [InlineData("a + b", "Equal sign not found in input text")]
        [InlineData("42", "Equal sign not found in input text")]
        public void ParseWithoutEqualSignInput_ShouldSucceed(string input, string errorMessage)
        {
            // Arrange

            // Act
            Action act = () => _ = this.Fixture.Parser.ParseEquation(input);

            // Assert
            // @formatter:off
            using (new AssertionScope())
            {
                act.Should()
                   .Throw<EqualSignNotFoundException>()
                    .WithMessage(errorMessage);
            }
            // @formatter:on
        }

        [Theory]
        [InlineData("a = b =c", "Equal sign not single in input text")]
        [InlineData("0 = a + b = a", "Equal sign not single in input text")]
        [InlineData("==", "Equal sign not single in input text")]
        public void ParseWithMultipleEqualSignInput_ShouldSucceed(string input, string errorMessage)
        {
            // Arrange

            // Act
            Action act = () => _ = this.Fixture.Parser.ParseEquation(input);

            // Assert
            // @formatter:off
            using (new AssertionScope())
            {
                act.Should()
                   .Throw<EqualSignNotSingleException>()
                   .WithMessage(errorMessage);
            }
            // @formatter:on
        }

        [Theory]
        [InlineData("= a", "Empty terms")]
        [InlineData("a =", "Empty terms")]
        [InlineData("a + 2b - 2(a - b) =+", "Empty terms")]
        public void ParseEmptyTermsInput_ShouldSucceed(string input, string errorMessage)
        {
            // Arrange

            // Act
            Action act = () => _ = this.Fixture.Parser.ParseEquation(input);

            // Assert
            // @formatter:off
            using (new AssertionScope())
            {
                act.Should()
                   .Throw<EmptyTermsException>()
                   .WithMessage(errorMessage);
            }
            // @formatter:on
        }

        [Fact]
        public void ParseSimpleEquitationShouldSucceed()
        {
            // Arrange
            const string input = "a+ b =  42  -a";
            EquationTermBase left;
            EquationTermBase right;
            
            
            // Act
            (left, right) = this.Fixture.Parser.ParseEquation(input);

            // Assert
            // @formatter:off
            using (new AssertionScope())
            {
                left.Should()
                    .NotBeNull().And
                    .BeOfType<AdditionOperation>();
                    
                right.Should()
                     .NotBeNull().And
                     .BeOfType<SubtractionOperation>();
                    
                var leftOperation = (AdditionOperation)left;
                var rightOperation = (SubtractionOperation)right;
                
                leftOperation.Left.Should()
                             .NotBeNull().And
                             .BeOfType<EquationVariable>()
                             .Which.Name.Should()
                             .Be('a');
                leftOperation.Right.Should()
                             .NotBeNull().And
                             .BeOfType<EquationVariable>()
                             .Which.Name.Should()
                             .Be('b');
                leftOperation.Type.Should().Be(OperationsEnum.Addition);
                
                 rightOperation.Left.Should()
                             .NotBeNull().And
                             .BeOfType<EquationConstant>()
                             .Which.Value.Should()
                             .Be(42M);
                rightOperation.Right.Should()
                             .NotBeNull().And
                             .BeOfType<EquationVariable>()
                             .Which.Name.Should()
                             .Be('a');
                rightOperation.Type.Should().Be(OperationsEnum.Subtraction);
            }
            // @formatter:on
        }
    }
}