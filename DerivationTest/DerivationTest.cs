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
    public class DerivationTest
    {
        private Parameters mParameters = new Parameters();

        [TestMethod]
        public void Derivation_Numbers()
        {
            Test("1", "0", "0", "0", "0");
            Test(".1", "0", "0", "0", "0");
            Test("-.1", "0", "0", "0", "0");
            Test("(-.1)", "0", "0", "0", "0");
        }

        [TestMethod]
        public void Derivation_Constants()
        {
            Test("e", "0", "0", "0", "0");
            Test("pi", "0", "0", "0", "0");

            Test("(pi)", "0", "0", "0", "0");
            Test("pi - .1", "0", "0", "0", "0");
        }

        [TestMethod]
        public void Derivation_Variables()
        {
            Test("x", "1", "0", "0", "0");
            Test("y", "0", "1", "0", "0");
            Test("z", "0", "0", "1", "0");
            Test("t", "0", "0", "0", "1");

            Test("-x", "-1", "0", "0", "0");
            Test("2 * x", "2", "0", "0", "0");
            Test("e * x", "E", "0", "0", "0");
            Test("-e * x", "(-E)", "0", "0", "0");
            Test("x + pi + 1", "1", "0", "0", "0");
        }

        [TestMethod]
        public void Derivation_Functions()
        {
            Test("Exp(x)", "Exp(x)", "0", "0", "0");
            Test("Exp(y)", "0", "Exp(y)", "0", "0");
            Test("Exp(z)", "0", "0", "Exp(z)", "0");
            Test("Exp(t)", "0", "0", "0", "Exp(t)");

            Test("Ln(x)", "(1 / x)", "0", "0", "0");
            Test("Ln(y)", "0", "(1 / y)", "0", "0");
            Test("Ln(z)", "0", "0", "(1 / z)", "0");
            Test("Ln(t)", "0", "0", "0", "(1 / t)");

            Test("Sqrt(x)", "(1 / (2 * Sqrt(x)))", "0", "0", "0");
            Test("Sqrt(y)", "0", "(1 / (2 * Sqrt(y)))", "0", "0");
            Test("Sqrt(z)", "0", "0", "(1 / (2 * Sqrt(z)))", "0");
            Test("Sqrt(t)", "0", "0", "0", "(1 / (2 * Sqrt(t)))");

            Test("Sin(x)", "Cos(x)", "0", "0", "0");
            Test("Sin(y)", "0", "Cos(y)", "0", "0");
            Test("Sin(z)", "0", "0", "Cos(z)", "0");
            Test("Sin(t)", "0", "0", "0", "Cos(t)");

            Test("Cos(x)", "(-Sin(x))", "0", "0", "0");
            Test("Cos(y)", "0", "(-Sin(y))", "0", "0");
            Test("Cos(z)", "0", "0", "(-Sin(z))", "0");
            Test("Cos(t)", "0", "0", "0", "(-Sin(t))");

            Test("Cos(1)", "0", "0", "0", "0");
            Test("Sin(1)", "0", "0", "0", "0");
            Test("Exp(1)", "0", "0", "0", "0");
            Test("Ln(1)", "0", "0", "0", "0");

            Test("-Cos(-1)", "0", "0", "0", "0");
            Test("Cos(Pi)", "0", "0", "0", "0");
            Test("-Cos(x)", "(-(-Sin(x)))", "0", "0", "0");
        }

        [TestMethod]
        public void Derivation_Operators()
        {
            Test("Sin(PI) * x", "0", "0", "0", "0");
            Test("Cos(PI) * x", "-1", "0", "0", "0");
            //Test(" x * Sin(5) / Sin(5)", "1", "0", "0", "0");//TODO: simplify 
            Test("(Sin(5) / Sin(5)) * x", "1", "0", "0", "0");
            Test("-(-(-x))", "-1", "0", "0", "0");
            //Test("(1 / 3) * (-x) ^ 3", "(-x) ^ 2", "0", "0", "0");//TODO: not calc 1/3
            Test("-Cos(x)", "(-(-Sin(x)))", "0", "0", "0");
            Test("x - Cos(x)", "(1 + Sin(x))", "0", "0", "0");

            Test("5 ^ x", "((5 ^ x) * Ln(5))", "0", "0", "0");
            Test("x ^ 5", "(5 * (x ^ 4))", "0", "0", "0");
            Test("x ^ x", "((x ^ x) * (1 + Ln(x)))", "0", "0", "0");
            Test("(5 ^ x) / Ln(5)", "((((5 ^ x) * Ln(5)) * Ln(5)) / (Ln(5) ^ 2))", "0", "0", "0");//TODO: simplify (2 * x)/x -> 2
            Test("(x ^ 5) / 5", "(x ^ 4)", "0", "0", "0");
            Test("x / Sin(5)", "(Sin(5) / (Sin(5) ^ 2))", "0", "0", "0");//TODO: simplify 
            Test("x * (1 + 3)", "4", "0", "0", "0");
        }

        [TestMethod]
        public void Derivation_Mixed()
        {
            Test("x + y + z + t", "1", "1", "1", "1");
            Test("-x - y", "-1", "(-1)", "0", "0");//TODO: why last is Negate?
            Test("-x - y - z - t", "-1", "-1", "-1", "(-1)");//TODO: why last is Negate?
            Test("x * y * z * t", "((y * z) * t)", "((x * z) * t)", "((x * y) * t)", "((x * y) * z)");

            Test("cos(x+1)", "(-Sin((x + 1)))", "0", "0", "0");
            Test("ln(x+1)", "(1 / (x + 1))", "0", "0", "0");

            Test("x^(x+1)", "((x ^ (x + 1)) * (((x + 1) / x) + Ln(x)))", "0", "0", "0");

            Test("x^3", "(3 * (x ^ 2))", "0", "0", "0");
            Test("3^x", "((3 ^ x) * Ln(3))", "0", "0", "0");

            Test("(x+x)", "2", "0", "0", "0");
            Test("(x+y)", "1", "1", "0", "0");

            Test("(x^4 + 2 * y) / 2", "(2 * (x ^ 3))", "1", "0", "0");
            Test("cos(pi)", "0", "0", "0", "0");
            Test("cos(x + pi)", "(-Sin((x + PI)))", "0", "0", "0");
        }

        private void Test(string input, string expectedX, string expectedY, string expectedZ, string expectedT)
        {
            string expected = string.Empty;
            string actual = string.Empty;

            try
            {
                FunctionParser parser = new FunctionParser();
                FunctionTree function = parser.Parse(input);

                expected = expectedX;
                actual = function.Expression.Derive(function.Parameters.X).ToString();
                Assert.AreEqual(expected, actual);

                expected = expectedY;
                actual = function.Expression.Derive(function.Parameters.Y).ToString();
                Assert.AreEqual(expected, actual);

                expected = expectedZ;
                actual = function.Expression.Derive(function.Parameters.Z).ToString();
                Assert.AreEqual(expected, actual);

                expected = expectedT;
                actual = function.Expression.Derive(function.Parameters.T).ToString();
                Assert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
                Assert.Fail(MessageHandler.GetMessage(ex, input, expected, actual));
            }
        }
    }
}
