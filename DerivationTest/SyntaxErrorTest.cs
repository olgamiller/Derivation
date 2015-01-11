/*
Derivation
  
Written in 2015 by <Olga Miller> <olga.rgb@googlemail.com>
To the extent possible under law, the author(s) have dedicated all copyright and related and neighboring rights to this software to the public domain worldwide.
This software is distributed without any warranty.
You should have received a copy of the CC0 Public Domain Dedication along with this software. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.
*/

using Derivation.CommonMath;
using Derivation.Nodes;
using Derivation.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DerivationTest
{
    [TestClass]
    public class SyntaxErrorTest
    {
        [TestMethod]
        public void SyntaxError_UnknownToken()
        {
            Test("j", typeof(UnknownTokenException));
            Test("xj", typeof(UnknownTokenException));
            Test("(j-x", typeof(UnknownTokenException));
            Test("ejp(x)", typeof(UnknownTokenException));
            Test("exp(x)j", typeof(UnknownTokenException));
        }

        [TestMethod]
        public void SyntaxError_UnknownWord()
        {
            Test("si", typeof(UnknownWordException));
            Test("sn(x)", typeof(UnknownWordException));
            Test("p", typeof(UnknownWordException));
            Test("lx", typeof(UnknownWordException));
            Test("sq(x)", typeof(UnknownWordException));
            Test("co(x)", typeof(UnknownWordException));
            Test("csin(x)", typeof(UnknownWordException));
        }

        [TestMethod]
        public void SyntaxError_Empty()
        {
            Test("", typeof(EmptyExpressionException));
            Test("   ", typeof(EmptyExpressionException));
            Test("()", typeof(EmptyExpressionException));
            Test("( )", typeof(EmptyExpressionException));

            Test("(x * ( + x)", typeof(EmptyExpressionException));
            Test("-*2", typeof(EmptyExpressionException));

            Test("x^-2", typeof(EmptyExpressionException));
            Test("x+-2", typeof(EmptyExpressionException));
            Test("x--2", typeof(EmptyExpressionException));

            Test("*", typeof(EmptyExpressionException));
            Test("+", typeof(EmptyExpressionException));
            Test("/", typeof(EmptyExpressionException));
            Test("^", typeof(EmptyExpressionException));
            Test("-", typeof(EmptyExpressionException));

            Test(" * x", typeof(EmptyExpressionException));
            Test(" + x", typeof(EmptyExpressionException));
            Test(" / x", typeof(EmptyExpressionException));
            Test(" ^ x", typeof(EmptyExpressionException));
            Test("--2", typeof(EmptyExpressionException));

            Test("x * ", typeof(EmptyExpressionException));
            Test("x + ", typeof(EmptyExpressionException));
            Test("x / ", typeof(EmptyExpressionException));
            Test("x ^ ", typeof(EmptyExpressionException));
            Test("x - ", typeof(EmptyExpressionException));

            Test("(y+) + x", typeof(EmptyExpressionException));
        }

        [TestMethod]
        public void SyntaxError_Operator()
        {
            Test("xx", typeof(OperatorException));
            Test("pix", typeof(OperatorException));
            Test("ex(x)", typeof(OperatorException));
            Test("eexp(x)", typeof(OperatorException));//TODO: place ^ better            
            Test("cos(ty)", typeof(OperatorException));
            Test("(x + y)(z + t)", typeof(OperatorException));
        }

        [TestMethod]
        public void SyntaxError_Parenthesis()
        {
            Test("(-x", typeof(ParenthesisException));
            Test("-x)", typeof(ParenthesisException));
            Test("(1 - (x * (-1))", typeof(ParenthesisException));
            Test("sin((x)", typeof(ParenthesisException));
            Test("lnx", typeof(ParenthesisException));
            Test("ln", typeof(ParenthesisException));
        }

        private void Test(string input, Type expectedException)
        {
            Exception actualException = null;

            try
            {
                try
                {
                    FunctionParser parser = new FunctionParser();
                    FunctionTree function = parser.Parse(input);

                    throw new ArgumentException();
                }
                catch (SyntaxException ex)
                {
                    actualException = ex;
                    Assert.AreEqual(expectedException.FullName, actualException.GetType().FullName);
                }
            }
            catch (Exception ex)
            {
                if (actualException == null)
                    actualException = ex;

                Assert.Fail(MessageHandler.GetMessage(ex, input, expectedException, actualException));
            }
        }
    }
}
