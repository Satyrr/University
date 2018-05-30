using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad2
{
    public class Context
    {
        Dictionary<string, bool> _values = new Dictionary<string, bool>();
        public bool GetValue(string VariableName)
        {
            if (!_values.ContainsKey(VariableName))
                throw new ArgumentException();

            return _values[VariableName];
        }

        public void SetValue(string VariableName, bool Value)
        {
            _values[VariableName] = Value;
        }
    }

    public abstract class AbstractExpression
    {
        public abstract bool Interpret(Context context);
    }

    public class ConstExpression : AbstractExpression
    {
        bool _value;
        public ConstExpression(bool value)
        {
            _value = value;
        }

        public override bool Interpret(Context context)
        {
            return _value;
        }
    }

    public class VariableExpression : AbstractExpression
    {
        string _variableName;
        public VariableExpression(string variableName)
        {
            _variableName = variableName;
        }

        public override bool Interpret(Context context)
        {
            return context.GetValue(_variableName);
        }
    }

    public enum LogicOperator
    {
        OR,
        AND
    }

    public class BinaryExpression : AbstractExpression
    {
        AbstractExpression _exp1, _exp2;
        LogicOperator _oper;
        public BinaryExpression(AbstractExpression exp1, AbstractExpression exp2, LogicOperator oper)
        {
            _exp1 = exp1;
            _exp2 = exp2;
            _oper = oper;
        }

        public override bool Interpret(Context context)
        {
            if(_oper == LogicOperator.OR)
            {
                return _exp1.Interpret(context) || _exp2.Interpret(context);
            }
            else
            {
                return _exp1.Interpret(context) && _exp2.Interpret(context);
            }
        }
    }
    public class UnaryExpression : AbstractExpression
    {
        AbstractExpression _expression;
        bool _negation;
        public UnaryExpression(AbstractExpression expression, bool negation)
        {
            _expression = expression;
            _negation = negation;
        }

        public override bool Interpret(Context context)
        {
            if(_negation)
            {
                return !_expression.Interpret(context);
            }
            else
            {
                return _expression.Interpret(context);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

        }
    }
}
