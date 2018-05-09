using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Zad4
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> l;



            //l.Where()

            DbContext db;

            //db.Set<Program>().Where(p => p.Age > 17 ).OrderBy( p => p.Age )
            //DBNull.Users.Where(x => x.Name == "Kowalski")

            Expression<Func<int, int>> f = n => 4 * (7 + n);
            ParameterExpression value = Expression.Parameter(typeof(int), "value");

            // Creating an expression to hold a local variable. 
            ParameterExpression result = Expression.Parameter(typeof(int), "result");

            // Creating a label to jump to from a loop.
            LabelTarget label = Expression.Label(typeof(int));

            // Creating a method body.
            BlockExpression block = Expression.Block(
                new[] { result },
                Expression.Assign(result, Expression.Constant(1)),
                    Expression.Loop(
                       Expression.IfThenElse(
                           Expression.GreaterThan(value, Expression.Constant(1)),
                           Expression.MultiplyAssign(result,
                               Expression.PostDecrementAssign(value)),
                           Expression.Break(label, result)
                       ),
                   label
                )
            );

            PrintExpressionVisitor v = new PrintExpressionVisitor();
            v.Visit(block);


            Console.ReadLine();
        }
    }

    public class PrintExpressionVisitor : ExpressionVisitor
    {
        protected override Expression VisitBinary(BinaryExpression expression)
        {
            Console.WriteLine("{0} {1} {2}",
            expression.Left, expression.NodeType, expression.Right);

            return base.VisitBinary(expression);
        }

        protected override Expression VisitLambda<T>(Expression<T> expression)
        {
            Console.WriteLine("{0} -> {1}",
            expression.Parameters.Aggregate(string.Empty, (a, e) => a += e),
            expression.Body);

            return base.VisitLambda<T>(expression);
        }

        protected override Expression VisitLoop(LoopExpression node)
        {
            Console.WriteLine("while(true):\n   {0}",
            node.Body);

            return base.VisitLoop(node);
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            Console.WriteLine("if {0} then {1}, else {2}", node.Test, node.IfTrue, node.IfFalse);

            return base.VisitConditional(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            Console.WriteLine("constant value = {0}", node.Value);
            return base.VisitConstant(node);
        }



    }
}
