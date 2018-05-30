using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zad2;

namespace Testy
{
    [TestFixture]
    class Zad2Tests
    {
        [Test]
        public void BadContextTest()
        {
            Context context = new Context();
            context.SetValue("Var1", true);

            Assert.Throws<ArgumentException>(() => 
                new VariableExpression("Var2").Interpret(context));
        }

        [Test]
        public void ANDInterpretationTest()
        {
            Context context = new Context();
            context.SetValue("Var1", true);
            context.SetValue("Var2", false);

            bool result = new BinaryExpression(
                new VariableExpression("Var1"),
                new VariableExpression("Var2"),
                LogicOperator.AND)
                .Interpret(context);

            Assert.AreEqual(false, result);

        }
        [Test]
        public void ORInterpretationTest()
        {
            Context context = new Context();
            context.SetValue("Var1", true);
            context.SetValue("Var2", false);

            bool result = new BinaryExpression(
                new VariableExpression("Var1"),
                new VariableExpression("Var2"),
                LogicOperator.OR)
                .Interpret(context);

            Assert.AreEqual(true, result);
        }
        [Test]
        public void BinaryInterpretationTest()
        {
            Context context = new Context();
            context.SetValue("Var1", true);
            context.SetValue("Var2", false);

            bool result = new BinaryExpression(new BinaryExpression(
                new VariableExpression("Var1"),
                new VariableExpression("Var2"),
                LogicOperator.OR), 
                new BinaryExpression(
                new VariableExpression("Var1"),
                new VariableExpression("Var2"),
                LogicOperator.AND),
                LogicOperator.OR)
                .Interpret(context);

            Assert.AreEqual(result, true);
        }
    }
}
