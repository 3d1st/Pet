namespace Contracts.Terms
{
    public class EquationConstant : EquationTermBase
    {
        public decimal Value { get; }
        
        public EquationConstant(decimal value)
        {
            this.Value = value;
        }

        public EquationConstant(string value) : this(decimal.Parse(value))
        {
        }

        public static implicit operator EquationConstant(decimal value) => new EquationConstant(value);
        
        public static implicit operator EquationConstant(string value) => new EquationConstant(value);
    }
}