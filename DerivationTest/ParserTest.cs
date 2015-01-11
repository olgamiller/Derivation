/*
Derivation
  
Written in 2015 by <Olga Miller> <olga.rgb@googlemail.com>
To the extent possible under law, the author(s) have dedicated all copyright and related and neighboring rights to this software to the public domain worldwide.
This software is distributed without any warranty.
You should have received a copy of the CC0 Public Domain Dedication along with this software. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.
*/

using Derivation.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DerivationTest
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void Parser_Numbers()
        {
            Test("0", "0");
            Test("-0", "0");
            Test("0.0", "0");
            Test("0.000", "0");
            Test("000.000", "0");
            Test("010.010", "10.01");
            Test("1", "1");
            Test("  1  ", "1");
            Test("-3", "(-3)");
            Test(".001", "0.001");
            Test("-.001", "(-0.001)");
            Test(".0010", "0.001");
            Test("1234567890", "1234567890");
            Test("0.987654321", "0.987654321");
            //Test("1e2", "100"); //is not supported

            Test("1/2", "(1 / 2)");
            Test("1*2", "(1 * 2)");
            Test("1+2", "(1 + 2)");
            Test("1-2", "(1 - 2)");
            Test("1^2", "(1 ^ 2)");
            Test("-1/2", "(-(1 / 2))");
            Test("-1+2", "((-1) + 2)");
            Test("-1-2", "((-1) - 2)");
        }

        [TestMethod]
        public void Parser_Whitespace()
        {
            Test("   1         ", "1");
            Test("-   1 /2", "(-(1 / 2))");
            Test(" -1 /  2", "(-(1 / 2))");
            Test(" - 1 /  2  ", "(-(1 / 2))");
            Test("-x * y^  8", "(-(x * (y ^ 8)))");
            Test("-  cos(  2*pi*x    ) ", "(-Cos(((2 * PI) * x)))");
        }

        [TestMethod]
        public void Parser_Variables()
        {
            Test("x", "x");
            Test("y", "y");
            Test("z", "z");
            Test("t", "t");

            Test("X", "x");
            Test("Y", "y");
            Test("Z", "z");
            Test("T", "t");
        }

        [TestMethod]
        public void Parser_Operators()
        {
            Test("-x", "(-x)");
            Test("x+y", "(x + y)");
            Test("y*z", "(y * z)");
            Test("z/t", "(z / t)");
            Test("2^t", "(2 ^ t)");

            Test("x-0", "(x - 0)");
            Test("-(-(-(-x)))", "(-(-(-(-x))))"); // should be reduced?
            Test("-(-(-x))", "(-(-(-x)))");
            Test("-(-x-(-x))", "(-((-x) - (-x)))");
            Test("-(-2 * x)", "(-(-(2 * x)))");
            Test("-(-2 ^ x)", "(-(-(2 ^ x)))");
            Test("-((-2) ^ x)", "(-((-2) ^ x))");

            Test("-1 - x + y * z / t ^ 2", "(((-1) - x) + ((y * z) / (t ^ 2)))");
            Test("-1 ^ 2", "(-(1 ^ 2))");
            Test("1 + 2 ^ 3 * 4", "(1 + ((2 ^ 3) * 4))");
            Test("1 + 2 ^ 3 * 4", "(1 + ((2 ^ 3) * 4))");
        }

        [TestMethod]
        public void Parser_Constants()
        {
            Test("pi", "PI");
            Test("PI", "PI");
            Test("pI", "PI");

            Test("e", "E");
            Test("E", "E");
        }

        [TestMethod]
        public void Parser_Functions()
        {
            Test("sin(x)", "Sin(x)");
            Test("SIN(x)", "Sin(x)");
            Test("sIN(x)", "Sin(x)");

            Test("cos(x)", "Cos(x)");
            Test("COS(x)", "Cos(x)");
            Test("cOS(x)", "Cos(x)");

            Test("exp(x)", "Exp(x)");
            Test("EXP(x)", "Exp(x)");
            Test("eXP(x)", "Exp(x)");

            Test("ln(x)", "Ln(x)");
            Test("LN(x)", "Ln(x)");
            Test("lN(x)", "Ln(x)");

            Test("sqrt(x)", "Sqrt(x)");
            Test("SQRT(x)", "Sqrt(x)");
            Test("sQRT(x)", "Sqrt(x)");
            Test("sQRt(x)", "Sqrt(x)");

            Test("sin(exp(cos(sqrt(ln(x)))))", "Sin(Exp(Cos(Sqrt(Ln(x)))))");
            Test("sin(x) + cos(y) * sqrt(z)", "(Sin(x) + (Cos(y) * Sqrt(z)))");
            Test("sin(cos(y) + sqrt(z))", "Sin((Cos(y) + Sqrt(z)))");
            Test("Sin(Cos(y) + Sqrt(z) ^ Cos(x))", "Sin((Cos(y) + (Sqrt(z) ^ Cos(x))))");
        }

        [TestMethod]
        public void Parser_Parenthesis()
        {
            Test("x^(-3)", "(x ^ (-3))");
            Test("(-1)^3", "((-1) ^ 3)");
            Test("((((-1))))^3", "((-1) ^ 3)");
            Test("((((1))) + (2)) * (3)", "((1 + 2) * 3)");
            Test("(1 - x) + ((y + 3) ^ (y - 1))", "((1 - x) + ((y + 3) ^ (y - 1)))");
            Test("-(1) + (-x)", "((-1) + (-x))");
            Test("-(1) * (-x)", "(-(1 * (-x)))");
            Test("(-1) ^ (-x)", "((-1) ^ (-x))");
            Test("cos(x) ^ (-2)", "(Cos(x) ^ (-2))");
            Test("Cos(x) * ((Sin((x))))", "(Cos(x) * Sin(x))");
            Test("(-1)^3", "((-1) ^ 3)");
            Test("(x * 10) -3", "((x * 10) - 3)");
        }

        [TestMethod]
        public void Parser_Mixed()
        {
            Test("1-x", "(1 - x)");
            Test("0.234+x", "(0.234 + x)");
            Test("x ^ .234", "(x ^ 0.234)");

            Test("(1*2+(-1))", "((1 * 2) + (-1))");
            Test("(1+2*(-1))", "(1 + (2 * (-1)))");
            Test("2 ^ (-x + y)", "(2 ^ ((-x) + y))");
            Test("- (x + y) + z", "((-(x + y)) + z)");

            Test("x ^ 2 + y ^ 2 - 1", "(((x ^ 2) + (y ^ 2)) - 1)");
            Test("- (x + y) ^ 2 + z", "((-((x + y) ^ 2)) + z)");
            Test("- x * y ^ 3 ^ 4", "(-(x * ((y ^ 3) ^ 4)))");

            Test("t+.234+x", "((t + 0.234) + x)");
            Test("x *( 10+y*x*z^4^7 - 2)",
                "(x * ((10 + ((y * x) * ((z ^ 4) ^ 7))) - 2))");

            Test("  1 + 2*3^4^7 -x ^10^y",
                "((1 + (2 * ((3 ^ 4) ^ 7))) - ((x ^ 10) ^ y))");

            Test("1-x^2^y*z*t", "(1 - ((((x ^ 2) ^ y) * z) * t))");

            Test("x + y * (1.0002345 + z) ^ t - .6 + sqrt(cos(2 * y) - sin(x))",
                "(((x + (y * ((1.0002345 + z) ^ t))) - 0.6) + Sqrt((Cos((2 * y)) - Sin(x))))");

            Test("sqrt((cos((2 * y)) - sin(x)))",
                "Sqrt((Cos((2 * y)) - Sin(x)))");

            Test("y * sqrt(1.00000010 + z) ^ t",
                "(y * (Sqrt((1.0000001 + z)) ^ t))");
        }

        private void Test(string input, string expected)
        {
            string actual = string.Empty;

            try
            {
                FunctionParser parser = new FunctionParser();
                FunctionTree function = parser.Parse(input);
                actual = function.Expression.ToString();

                Assert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
                Assert.Fail(MessageHandler.GetMessage(ex, input, expected, actual));
            }
        }
    }
}
