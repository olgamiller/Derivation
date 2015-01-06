/*
Derivation Example

Written in 2015 by <Olga Miller> <olga.rgb@googlemail.com>
To the extent possible under law, the author(s) have dedicated all copyright and related and neighboring rights to this software to the public domain worldwide.
This software is distributed without any warranty.
You should have received a copy of the CC0 Public Domain Dedication along with this software. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.
*/

using Derivation.CommonMath;
using Derivation.Nodes;
using Derivation.Parsing;
using System;

namespace Example.Derivation
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();

                if (line == "quit" || line == "exit")
                    return;

                string[] input = GetInput(line);
                if (input == null) continue;

                FunctionTree function = ParseFunction(input[0]);
                if (function == null) continue;

                double[] values = ParseValues(input, line);
                if (values == null) continue;

                Node[] partialDerivative = GetPartialDerivative(function);

                Console.WriteLine();
                WriteFunction(function.Expression);
                WriteFunctionValue(function.Expression, values);
                WritePartialDerivative(partialDerivative);
                WritePartialDerivativeValue(partialDerivative, values);
                Console.WriteLine();
            }
        }

        private static string[] GetInput(string line)
        {
            string[] input = line.Split(',');

            if (input.Length > 5)
            {
                Console.WriteLine("Maximum 5 comma separated items can be entered: Function and values for x, y, z and t.\n"
                    + "Example: x * y * z * t, 1, 2, 3, 4");
                return null;
            }

            return input;
        }

        private static FunctionTree ParseFunction(string input)
        {
            FunctionParser parser = new FunctionParser();

            try
            {
                return parser.Parse(input);
            }
            catch (SyntaxException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Internal parser error: " + ex.Message);
            }

            return null;
        }

        private static double[] ParseValues(string[] input, string line)
        {
            double[] values = new double[4];
            string s = "Invalid {0} value: {1}\n";

            if (input.Length >= 2 && !double.TryParse(input[1], out values[0]))
            {
                Console.WriteLine(string.Format(s, "x", input[1]));
                return null;
            }

            if (input.Length >= 3 && !double.TryParse(input[2], out values[1]))
            {
                Console.WriteLine(string.Format(s, "y", input[2]));
                return null;
            }

            if (input.Length >= 4 && !double.TryParse(input[3], out values[2]))
            {
                Console.WriteLine(string.Format(s, "z", input[3]));
                return null;
            }

            if (input.Length == 5 && !double.TryParse(input[4], out values[3]))
            {
                Console.WriteLine(string.Format(s, "t", input[4]));
                return null;
            }

            return values;
        }

        private static void WriteFunction(Node function)
        {
            Console.WriteLine(string.Format("f(x, y, z, t) = {0}\n", function.ToString()));
        }

        private static void WriteFunctionValue(Node function, double[] values)
        {
            PointMath math = new PointMath();
            double value = function.Apply(math, values[0], values[1], values[2], values[3]);

            Console.WriteLine(string.Format("f({0}, {1}, {2}, {3}) = {4}\n",
                values[0], values[1], values[2], values[3], value));
        }

        private static Node[] GetPartialDerivative(FunctionTree function)
        {
            Node[] result = new Node[4];
            result[0] = function.Expression.Derive(function.Parameters.X);
            result[1] = function.Expression.Derive(function.Parameters.Y);
            result[2] = function.Expression.Derive(function.Parameters.Z);
            result[3] = function.Expression.Derive(function.Parameters.T);

            return result;
        }

        private static void WritePartialDerivative(Node[] partialDerivative)
        {
            Console.WriteLine(string.Format("grad f(x, y, z, t) = ({0}, {1}, {2}, {3})\n",
                partialDerivative[0].ToString(),
                partialDerivative[1].ToString(),
                partialDerivative[2].ToString(),
                partialDerivative[3].ToString()));
        }

        private static void WritePartialDerivativeValue(Node[] partialDerivative, double[] values)
        {
            PointMath math = new PointMath();

            Console.WriteLine(string.Format("grad f({0}, {1}, {2}, {3}) = ({4}, {5}, {6}, {7})\n",
                values[0], values[1], values[2], values[3],
                partialDerivative[0].Apply(math, values[0], values[1], values[2], values[3]),
                partialDerivative[1].Apply(math, values[0], values[1], values[2], values[3]),
                partialDerivative[2].Apply(math, values[0], values[1], values[2], values[3]),
                partialDerivative[3].Apply(math, values[0], values[1], values[2], values[3])));
        }
    }
}
