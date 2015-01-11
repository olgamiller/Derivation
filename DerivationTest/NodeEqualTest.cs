/*
Derivation
  
Written in 2015 by <Olga Miller> <olga.rgb@googlemail.com>
To the extent possible under law, the author(s) have dedicated all copyright and related and neighboring rights to this software to the public domain worldwide.
This software is distributed without any warranty.
You should have received a copy of the CC0 Public Domain Dedication along with this software. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.
*/

using Derivation.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DerivationTest
{
    [TestClass]
    public class NodeEqualTest
    {
        [TestMethod]
        public void NodeEqual_Numbers()
        {
            Node n1, n2;

            n1 = Node.Number(1);
            n2 = Node.Number(1);
            Assert.IsTrue(n1.Equals(n2));
            Assert.IsTrue(n1.Equals(n1));

            n1 = Node.Number(0);
            n2 = Node.Number(-0);
            Assert.IsTrue(n1.Equals(n2));

            n1 = Node.Number(1);
            n2 = Node.Number(2);
            Assert.IsFalse(n1.Equals(n2));

            n1 = Node.Number(.1);
            n2 = Node.Number(.01);
            Assert.IsFalse(n1.Equals(n2));

            n1 = Node.Number(1);
            n2 = Node.E();
            Assert.IsFalse(n1.Equals(n2));
            Assert.IsFalse(n2.Equals(n1));
        }

        [TestMethod]
        public void NodeEqual_Constants()
        {
            Node n1, n2;

            n1 = Node.PI();
            n2 = Node.PI();
            TestEqual(n1, n2);

            n1 = Node.E();
            n2 = Node.E();
            TestEqual(n1, n2);

            n1 = Node.PI();
            n2 = Node.Number(1);
            TestUnequal(n1, n2);

            n1 = Node.PI();
            n2 = Node.E();
            TestUnequal(n1, n2);
        }

        [TestMethod]
        public void NodeEqual_Variables()
        {
            Node n1, n2;

            n1 = Node.Parameter("x");
            n2 = Node.Parameter("x");
            TestEqual(n1, n2);

            n1 = Node.Parameter("t");
            n2 = Node.Parameter("t");
            TestEqual(n1, n2);

            n1 = Node.Parameter("x");
            n2 = Node.Parameter("t");
            TestUnequal(n1, n2);

            n1 = Node.Parameter("x");
            n2 = Node.Parameter("t");
            TestUnequal(n1, n2);

            n1 = Node.Parameter("x");
            n2 = Node.Number(1);
            TestUnequal(n1, n2);
        }

        [TestMethod]
        public void NodeEqual_Functions()
        {
            Node n1, n2;

            n1 = Node.Sin(Node.Number(1));
            n2 = Node.Sin(Node.Number(1));
            TestEqual(n1, n2);

            n1 = Node.Cos(Node.Number(1));
            n2 = Node.Cos(Node.Number(1));
            TestEqual(n1, n2);

            n1 = Node.Sqrt(Node.Number(1));
            n2 = Node.Sqrt(Node.Number(1));
            TestEqual(n1, n2);

            n1 = Node.Exp(Node.Number(1));
            n2 = Node.Exp(Node.Number(1));
            TestEqual(n1, n2);

            n1 = Node.Ln(Node.Number(1));
            n2 = Node.Ln(Node.Number(1));
            TestEqual(n1, n2);

            // diff type

            n1 = Node.Sin(Node.Number(1));
            n2 = Node.Cos(Node.Number(1));
            TestUnequal(n1, n2);

            n1 = Node.Sqrt(Node.Number(1));
            n2 = Node.Ln(Node.Number(1));
            TestUnequal(n1, n2);

            n1 = Node.Exp(Node.Number(1));
            n2 = Node.Ln(Node.Number(1));
            TestUnequal(n1, n2);

            // diff child nodes

            n1 = Node.Sin(Node.Number(1));
            n2 = Node.Sin(Node.Number(2));
            TestUnequal(n1, n2);

            n1 = Node.Cos(Node.Number(1));
            n2 = Node.Cos(Node.Number(2));
            TestUnequal(n1, n2);

            n1 = Node.Sqrt(Node.Number(1));
            n2 = Node.Sqrt(Node.Number(2));
            TestUnequal(n1, n2);

            n1 = Node.Exp(Node.Number(1));
            n2 = Node.Exp(Node.Number(2));
            TestUnequal(n1, n2);

            n1 = Node.Ln(Node.Number(1));
            n2 = Node.Ln(Node.Number(2));
            TestUnequal(n1, n2);
        }

        [TestMethod]
        public void NodeEqual_Operators()
        {
            Node n1, n2;
            Node x = Node.Parameter("x");

            n1 = Node.Add(x, Node.E());
            n2 = Node.Add(x, Node.E());
            TestEqual(n1, n2);

            n1 = Node.Subtract(x, Node.E());
            n2 = Node.Subtract(x, Node.E());
            TestEqual(n1, n2);

            n1 = Node.Divide(x, Node.E());
            n2 = Node.Divide(x, Node.E());
            TestEqual(n1, n2);

            n1 = Node.Negate(Node.E());
            n2 = Node.Negate(Node.E());
            TestEqual(n1, n2);

            n1 = Node.Multiply(x, Node.E());
            n2 = Node.Multiply(x, Node.E());
            TestEqual(n1, n2);

            n1 = Node.Power(x, Node.E());
            n2 = Node.Power(x, Node.E());
            TestEqual(n1, n2);

            // diff type

            n1 = Node.Add(x, Node.E());
            n2 = Node.Subtract(x, Node.E());
            TestUnequal(n1, n2);

            n1 = Node.Multiply(x, Node.E());
            n2 = Node.Divide(x, Node.E());
            TestUnequal(n1, n2);

            n1 = Node.Power(x, Node.E());
            n2 = Node.Divide(x, Node.E());
            TestUnequal(n1, n2);

            n1 = Node.Negate(x);
            n2 = Node.Subtract(x, Node.E());
            TestUnequal(n1, n2);

            // diff child nodes

            n1 = Node.Add(x, Node.E());
            n2 = Node.Add(Node.E(), x);
            TestUnequal(n1, n2);

            n1 = Node.Subtract(x, Node.E());
            n2 = Node.Subtract(Node.E(), x);
            TestUnequal(n1, n2);

            n1 = Node.Divide(x, Node.E());
            n2 = Node.Divide(Node.E(), x);
            TestUnequal(n1, n2);

            n1 = Node.Negate(x);
            n2 = Node.Negate(Node.E());
            TestUnequal(n1, n2);

            n1 = Node.Multiply(x, Node.E());
            n2 = Node.Multiply(Node.E(), x);
            TestUnequal(n1, n2);

            n1 = Node.Power(x, Node.E());
            n2 = Node.Power(Node.E(), x);
            TestUnequal(n1, n2);
        }

        private void TestEqual(Node n1, Node n2)
        {
            string message = MessageHandler.GetMessage(n1, n2, true);
            Assert.IsTrue(n1.Equals(n2), message);
            Assert.IsTrue(n1.Equals(n1), message);
        }

        private void TestUnequal(Node n1, Node n2)
        {
            string message = MessageHandler.GetMessage(n1, n2, false);
            Assert.IsFalse(n1.Equals(n2), message);
            Assert.IsFalse(n2.Equals(n1), message);
        }
    }
}
