/*
Derivation
  
Written in 2015 by <Olga Miller> <olga.rgb@googlemail.com>
To the extent possible under law, the author(s) have dedicated all copyright and related and neighboring rights to this software to the public domain worldwide.
This software is distributed without any warranty.
You should have received a copy of the CC0 Public Domain Dedication along with this software. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.
*/

using Derivation.CommonMath;

namespace Derivation.Nodes
{
    public abstract class FunctionNode : Node
    {
        private string mToken;

        public Node Node { get; protected set; }

        internal FunctionNode(Node node, string token)
        {
            Node = node;
            mToken = token;
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", mToken, Node.ToString());
        }

        public override bool Equals(object obj)
        {
            if (GetType() == obj.GetType() && Node.Equals(((FunctionNode)obj).Node))
                return true;

            return base.Equals(obj);
        }

        public override int GetHashCode() { return base.GetHashCode(); }
    }

    public class SinNode : FunctionNode
    {
        internal SinNode(Node node)
            : base(node, "Sin")
        {
        }

        public override T Apply<T>(IMath<T> m, T x, T y, T z, T t)
        {
            return m.Sin(Node.Apply(m, x, y, z, t));
        }

        public override Node Derive(ParameterNode p)
        {
            return Multiply(Cos(Node), Node.Derive(p)).Reduce();
        }

        public override Node Reduce()
        {
            Node = Node.Reduce();

            if (Node is PiNode)
                return Number(0);

            return this;
        }
    }

    public class CosNode : FunctionNode
    {
        internal CosNode(Node node)
            : base(node, "Cos")
        {
        }

        public override T Apply<T>(IMath<T> m, T x, T y, T z, T t)
        {
            return m.Cos(Node.Apply(m, x, y, z, t));
        }

        public override Node Derive(ParameterNode p)
        {
            return Multiply(Negate(Sin(Node)), Node.Derive(p)).Reduce();
        }

        public override Node Reduce()
        {
            Node = Node.Reduce();

            if (Node is PiNode)
                return Number(-1);

            return this;
        }
    }

    public class SqrtNode : FunctionNode
    {
        internal SqrtNode(Node node)
            : base(node, "Sqrt")
        {
        }

        public override T Apply<T>(IMath<T> m, T x, T y, T z, T t)
        {
            return m.Sqrt(Node.Apply(m, x, y, z, t));
        }

        public override Node Derive(ParameterNode p)
        {
            Node tmp = Multiply(Number(2), Sqrt(Node));
            Node sqrtDerivation = Divide(Number(1), tmp);

            return Multiply(sqrtDerivation, Node.Derive(p)).Reduce();
        }

        public override Node Reduce()
        {
            Node = Node.Reduce();
            return this;
        }
    }

    public class LnNode : FunctionNode
    {
        internal LnNode(Node node)
            : base(node, "Ln")
        {
        }

        public override T Apply<T>(IMath<T> m, T x, T y, T z, T t)
        {
            return m.Ln(Node.Apply(m, x, y, z, t));
        }

        public override Node Derive(ParameterNode p)
        {
            return Divide(Node.Derive(p), Node).Reduce();
        }

        public override Node Reduce()
        {
            Node = Node.Reduce();
            return this;
        }
    }

    public class ExpNode : FunctionNode
    {
        internal ExpNode(Node node)
            : base(node, "Exp")
        {
        }

        public override T Apply<T>(IMath<T> m, T x, T y, T z, T t)
        {
            return m.Exp(Node.Apply(m, x, y, z, t));
        }

        public override Node Derive(ParameterNode p)
        {
            return Multiply(this, Node.Derive(p)).Reduce();
        }

        public override Node Reduce()
        {
            Node = Node.Reduce();
            return this;
        }
    }
}
