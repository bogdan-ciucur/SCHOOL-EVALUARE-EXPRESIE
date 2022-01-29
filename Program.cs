using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace _01_10
{
    class Program
    {
        
        
        static void Main(string[] args)
        {

            string input;
            Console.WriteLine("introduceti expresia:");
            input = Console.ReadLine();

            string inputCopy = input; 

            input = "( " + input + " )";
            string[] tokens = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            List<string> rpn = RPN(tokens);


            Console.WriteLine($"{inputCopy} = {EvaluateRPN(rpn)}");



        }

        private static double EvaluateRPN(List<string> rpn)
        {
            Stack<double> stack = new Stack<double>();

            foreach (var item in rpn)
            {
                if (IsOperator(item))
                {
                    double op1, op2;
                    op2 = stack.Pop();
                    op1 = stack.Pop();
                    stack.Push(Operate(op1, op2, item));
                }
                else
                {
                    stack.Push(double.Parse(item));
                }
            }

            return stack.Pop();
        }

        private static double Operate(double op1, double op2, string item)
        {
            double retValue;
            switch (item)
            {
                case "+":
                    retValue = op1 + op2;
                    break;

                case "-":
                    retValue = op1 - op2;
                    break;

                case "*":
                    retValue = op1 * op2;
                    break;

                case "/":
                    retValue = op1 / op2;
                    break;

                case "%":
                    retValue = op1 + op2;
                    break;
                
                case "^":
                    retValue = (int) Math.Pow(op1, op2);
                    break;

                default:
                    retValue = 0;
                    break;
            }
            return retValue;
        }

        private static bool IsOperator(string item)
        {
            switch (item)
            {
                case "+":
                case "-":
                case "*":
                case "/":
                case "%":
                case "^":
                    return true;
                default:
                    return false;
            }
        }

        private static List<string> RPN(string[] tokens)
        {
            Stack<string> stack = new Stack<string>();
            List<string> rpn = new List<string>();

            foreach (var token in tokens)
            {
                if (token == "(")
                {
                    stack.Push(token);
                }

                else if(IsOperator(token))
                {
                    string op;
                    while (stack.Peek() != "(" && Priority(stack.Peek()) >= Priority(token))
                    {
                        op = stack.Pop();
                        rpn.Add(op);
                    }
                    stack.Push(token);
                }

                else if (token == ")")
                {
                    string op;
                    op = stack.Pop();
                    while (op != "(")
                    {
                        rpn.Add(op);
                        op = stack.Pop();
                    }
                }

                else if(double.TryParse(token, out _))
                {
                    rpn.Add(token);
                }

                else
                {
                    throw new InvalidExpressionException($"token invalid: {token}");
                }
            }
            return rpn;
        }

        private static int Priority(string op)
        {
            int retValue;
            switch (op)
            {
                case "+":
                case "-":
                    retValue = 1;
                    break;

                case "*":
                case "/":
                case "%":
                    retValue = 2;
                    break;

                case "^":
                    retValue = 3;
                    break;

                default:
                    retValue = 0;
                    break;
            }
            return retValue;
        }
    }
}
