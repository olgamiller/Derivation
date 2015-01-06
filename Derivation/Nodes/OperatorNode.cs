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
    public abstract class BinaryNode : Node
    {
        protected string mToken;

        public Node Left { get; private set; }
        public Node Right { get; private set; }

        internal BinaryNode(Node l, Node r, string token)
        {
            Left = l;
            Right = r;

            mToken = token;
        }

        public override string ToString()
        {
            return string.Format("({0} {1} {2})", Left.ToString(), mToken, Right.ToString());
        }
    }

    public class NegateNode : Node
    {
        public Node Node { get; private set; }

        internal NegateNode(Node n)
        {
            Node = n;
        }

        public override T Apply<T>(IMath<T> m, T x, T y, T z, T t)
        {
            return m.Negate(Node.Apply(m, x, y, z, t));
        }

        public override Node Derive(ParameterNode p)
        {
            Node innerDerivation = Node.Derive(p);

            if (Is0(innerDerivation))
                return Number(0.0);

            if (innerDerivation is NumberNode)
                return ((NumberNode)innerDerivation).Negate();

            return Negate(innerDerivation);
        }

        public override string ToString()
        {
            return string.Format("-{0}", Node.ToString());
        }
    }

    public class AddNode : BinaryNode
    {
        internal AddNode(Node l, Node r)
            : base(l, r, "+")
        {
        }

        public override T Apply<T>(IMath<T> m, T x, T y, T z, T t)
        {
            return m.Add(Left.Apply(m, x, y, z, t), Right.Apply(m, x, y, z, t));
        }

        public override Node Derive(ParameterNode p)
        {
            return ReduceAdd(Left.Derive(p), Right.Derive(p));
        }
    }

    public class SubtractNode : BinaryNode
    {
        internal SubtractNode(Node l, Node r)
            : base(l, r, "-")
        {
        }

        public override T Apply<T>(IMath<T> m, T x, T y, T z, T t)
        {
            return m.Subtract(Left.Apply(m, x, y, z, t), Right.Apply(m, x, y, z, t));
        }

        public override Node Derive(ParameterNode p)
        {
            return ReduceSubtract(Left.Derive(p), Right.Derive(p));
        }
    }

    public class DivideNode : BinaryNode
    {
        internal DivideNode(Node l, Node r)
            : base(l, r, "/")
        {
        }

        public override T Apply<T>(IMath<T> m, T x, T y, T z, T t)
        {
            return m.Divide(Left.Apply(m, x, y, z, t), Right.Apply(m, x, y, z, t));
        }

        public override Node Derive(ParameterNode p)
        {
            if (Right is NumberNode)
            {
                Node derivationL = Left.Derive(p);

                if (derivationL is BinaryNode)
                {
                    BinaryNode n = (BinaryNode)derivationL;

                    if (n.Left is NumberNode)
                        return ReduceMultiply(((NumberNode)n.Left).Divide(Right), n.Right);

                    if (n.Right is NumberNode)
                        return ReduceMultiply(((NumberNode)n.Right).Divide(Right), n.Left);
                }

                return ReduceDivide(derivationL, Right);
            }
            else
            {
                Node l = ReduceMultiply(Left.Derive(p), Right);
                Node r = ReduceMultiply(Left, Right.Derive(p));

                Node dividend = ReduceSubtract(l, r);
                Node divisor = ReducePower(Right, Number(2.0));

                return ReduceDivide(dividend, divisor);
            }
        }
    }

    public class MultiplyNode : BinaryNode
    {
        internal MultiplyNode(Node l, Node r)
            : base(l, r, "*")
        {
        }

        public override T Apply<T>(IMath<T> m, T x, T y, T z, T t)
        {
            return m.Multiply(Left.Apply(m, x, y, z, t), Right.Apply(m, x, y, z, t));
        }

        public override Node Derive(ParameterNode p)
        {
            Node l = ReduceMultiply(Left.Derive(p), Right);
            Node r = ReduceMultiply(Left, Right.Derive(p));

            return ReduceAdd(l, r);
        }
    }

    public class PowerNode : BinaryNode
    {
        internal PowerNode(Node l, Node r)
            : base(l, r, "^")
        {
        }

        public override T Apply<T>(IMath<T> m, T x, T y, T z, T t)
        {
            return m.Power(Left.Apply(m, x, y, z, t), Right.Apply(m, x, y, z, t));
        }

        public override Node Derive(ParameterNode p)
        {
            if (Right is NumberNode)
            {
                Node exponent = ((NumberNode)Right).Subtract(Number(1.0));
                Node power = ReducePower(Left, exponent);
                return ReduceMultiply(ReduceMultiply(Right, power), Left.Derive(p));
            }
            else
            {
                Node fPowg = ReducePower(Left, Right);

                Node derfMulgDivf = ReduceMultiply(Left.Derive(p), ReduceDivide(Right, Left));
                Node dergMullnf = ReduceMultiply(Right.Derive(p), Ln(Left));

                return ReduceMultiply(fPowg, ReduceAdd(derfMulgDivf, dergMullnf));
            }
        }
    }
}
