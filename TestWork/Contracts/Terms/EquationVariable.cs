namespace Contracts.Terms
{
    public class EquationVariable : EquationTermBase
    {
        public char Name { get; }

        public EquationVariable(char value)
        {
            this.Name = value;
        }

        public EquationVariable(string value) : this(value[0])
        {
            
        }
        
        public static implicit operator EquationVariable(char value) => new EquationVariable(value);
        
        public static implicit operator EquationVariable(string value) => new EquationVariable(value); 
    }
}