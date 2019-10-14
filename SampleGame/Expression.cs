using System;

namespace MathOlympics
{
    public class Expression
    {
        public double FirstOperand { get; set; }
        
        public double SecondOperand { get; set; }
        
        public Operator Operator { get; set; }

        public double Compute() =>
            Operator switch
            {
                Operator.Add => FirstOperand + SecondOperand,
                Operator.Subtract => FirstOperand - SecondOperand,
                Operator.Multiply => FirstOperand * SecondOperand,
                Operator.Divide => FirstOperand / SecondOperand,
                _ => throw new InvalidOperationException($"Invalid enum value {Operator}")
            };

        public override String ToString() =>
            $"{FirstOperand} {Operator switch {Operator.Add => "+", Operator.Subtract => "-", Operator.Multiply => "×", Operator.Divide => "÷", _ => "<ERROR>"}} {SecondOperand}";
    }

    public enum Operator
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }
}