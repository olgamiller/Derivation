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

        public Node Left { get; protected set; }
        public Node Right { get; protected set; }

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

        public override bool Equals(object obj)
        {
            if (GetType() == obj.GetType())
            {
                BinaryNode node = (BinaryNode)obj;

                if (Left.Equals(node.Left) && Right.Equals(node.Right))
                    return true;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode() { return base.GetHashCode(); }
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
            return Negate(Node.Derive(p)).Reduce();
        }

        public override Node Reduce()
        {
            Node = Node.Reduce();

            if (Is0(Node))
                return Number(0.0);

            if (Node is NumberNode)
                return ((NumberNode)Node).Negate();

            return this;
        }

        public override string ToString()
        {
            return string.Format("(-{0})", Node.ToString());
        }

        public override bool Equals(object obj)
        {
            if (obj is NegateNode && Node.Equals(((NegateNode)obj).Node))
                return true;

            return base.Equals(obj);
        }

        public override int GetHashCode() { return base.GetHashCode(); }
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
            return Add(Left.Derive(p), Right.Derive(p)).Reduce();
        }

        public override Node Reduce()
        {
            Left = Left.Reduce();
            Right = Right.Reduce();

            if (Is0(Left) && Is0(Right))
                return Number(0.0);

            if (Is0(Left))
                return Right;

            if (Is0(Right))
                return Left;

            if (Left is NumberNode && Right is NumberNode)
                return ((NumberNode)Left).Add(Right);

            return this;
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
            return Subtract(Left.Derive(p), Right.Derive(p)).Reduce();
        }

        public override Node Reduce()
        {
            Left = Left.Reduce();
            Right = Right.Reduce();

            if (Is0(Left) && Is0(Right))
                return Number(0.0);

            if (Is0(Left))
                return Negate(Right);

            if (Is0(Right))
                return Left;

            if (Right is NegateNode)
                return Add(Left, ((NegateNode)Right).Node).Reduce();

            if (Left is NumberNode && Right is NumberNode)
                return ((NumberNode)Left).Subtract(Right);

            return this;
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
                        return Multiply(((NumberNode)n.Left).Divide(Right), n.Right).Reduce();

                    if (n.Right is NumberNode)
                        return Multiply(((NumberNode)n.Right).Divide(Right), n.Left).Reduce();
                }

                return Divide(derivationL, Right).Reduce();
            }
            else
            {
                Node l = Multiply(Left.Derive(p), Right);
                Node r = Multiply(Left, Right.Derive(p));

                Node dividend = Subtract(l, r);
                Node divisor = Power(Right, Number(2.0));

                return Divide(dividend, divisor).Reduce();
            }
        }

        public override Node Reduce()
        {
            Left = Left.Reduce();
            Right = Right.Reduce();

            if (Is0(Left))
                return Number(0.0);

            if (Is1(Right))
                return Left;

            if (Left is NumberNode && Right is NumberNode)
                return ((NumberNode)Left).Divide(Right);

            if (Left.Equals(Right))
                return Number(1);

            return this;
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
            Node l = Multiply(Left.Derive(p), Right);
            Node r = Multiply(Left, Right.Derive(p));
            return Add(l, r).Reduce();
        }

        public override Node Reduce()
        {
            Left = Left.Reduce();
            Right = Right.Reduce();

            if (Is0(Left) || Is0(Right))
                return Number(0.0);

            if (Is1(Left))
                return Right;

            if (Is1(Right))
                return Left;

            if (Left is NumberNode && Right is NumberNode)
                return ((NumberNode)Left).Multiply(Right);

            return this;
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
                Node power = Power(Left, exponent);
                return Multiply(Multiply(Right, power), Left.Derive(p)).Reduce();
            }
            else
            {
                Node fPowg = Power(Left, Right);

                Node derfMulgDivf = Multiply(Left.Derive(p), Divide(Right, Left));
                Node dergMullnf = Multiply(Right.Derive(p), Ln(Left));

                return Multiply(fPowg, Add(derfMulgDivf, dergMullnf)).Reduce();
            }
        }

        public override Node Reduce()
        {
            Left = Left.Reduce();
            Right = Right.Reduce();

            if (Is0(Right) || Is1(Left))
                return Number(1.0);

            if (Is0(Left))
                return Number(0.0);

            if (Is1(Right))
                return Left;

            if (Left is NumberNode && Right is NumberNode)
                return ((NumberNode)Left).Power(Right);

            return this;
        }
    }
}
