using System.Linq.Expressions;

namespace Delegates
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var xExpression = Expression.Parameter(typeof(int), "x");
            var constantExpression = Expression.Constant(12);
            var greaterThen = Expression.GreaterThan(xExpression, constantExpression);

            var constant4Expression = Expression.Constant(4);
            var lessThen = Expression.LessThan(xExpression, constant4Expression);

            var or = Expression.Or(greaterThen, lessThen);

            var lambda = Expression.Lambda<Func<int, bool>>(
                or,
                false,
                new List<ParameterExpression> { xExpression }
                );
            Console.WriteLine("Lambda expression: " + lambda.ToString());

            var func = lambda.Compile();

            Console.WriteLine(func(2));
        }
    }
}