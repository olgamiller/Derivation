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
    public abstract class Node
    {
        public T Apply<T>(IMath<T> m, T x) { return Apply(m, x, x, x, x); }
        public abstract T Apply<T>(IMath<T> m, T x, T y, T z, T t);
        public abstract Node Derive(ParameterNode p);

        #region Operator Nodes

        public static Node Add(Node l, Node r) { return new AddNode(l, r); }
        public static Node Subtract(Node l, Node r) { return new SubtractNode(l, r); }
        public static Node Negate(Node n) { return new NegateNode(n); }

        public static Node Divide(Node l, Node r) { return new DivideNode(l, r); }
        public static Node Multiply(Node l, Node r) { return new MultiplyNode(l, r); }

        public static Node Power(Node l, Node r) { return new PowerNode(l, r); }

        #endregion

        #region Function Nodes

        public static Node Sin(Node n) { return new SinNode(n); }
        public static Node Cos(Node n) { return new CosNode(n); }
        public static Node Sqrt(Node n) { return new SqrtNode(n); }
        public static Node Ln(Node n) { return new LnNode(n); }
        public static Node Exp(Node n) { return new ExpNode(n); }

        #endregion

        #region Constant Nodes

        public static Node PI() { return new PiNode(); }
        public static Node E() { return new ENode(); }

        #endregion

        #region Other Nodes

        public static Node Number(double value) { return new NumberNode(value); }
        public static ParameterNode Parameter(string name) { return new ParameterNode(name); }

        #endregion

        #region Reduce

        protected static bool Is0(Node node)
        {
            if (node is NumberNode)
                return ((NumberNode)node).Value == 0.0;

            return false;
        }

        protected static bool Is1(Node node)
        {
            if (node is NumberNode)
                return ((NumberNode)node).Value == 1.0;

            return false;
        }

        protected static Node ReduceMultiply(Node l, Node r)
        {
            if (Is0(l) || Is0(r))
                return Number(0.0);

            if (Is1(l))
                return r;

            if (Is1(r))
                return l;

            if (l is NumberNode && r is NumberNode)
                return ((NumberNode)l).Multiply(r);

            return Multiply(l, r);
        }

        protected static Node ReduceAdd(Node l, Node r)
        {
            if (Is0(l) && Is0(r))
                return Number(0.0);

            if (Is0(l))
                return r;

            if (Is0(r))
                return l;

            if (l is NumberNode && r is NumberNode)
                return ((NumberNode)l).Add(r);

            return Add(l, r);
        }

        protected static Node ReduceSubtract(Node l, Node r)
        {
            if (Is0(l) && Is0(r))
                return Number(0.0);

            if (Is0(l))
                return Negate(r);

            if (Is0(r))
                return l;

            if (l is NumberNode && r is NumberNode)
                return ((NumberNode)l).Subtract(r);

            return Subtract(l, r);
        }

        protected static Node ReducePower(Node l, Node r)
        {
            if (Is0(r) || Is1(l))
                return Number(1.0);

            if (Is0(l))
                return Number(0.0);

            if (Is1(r))
                return l;

            if (l is NumberNode && r is NumberNode)
                return ((NumberNode)l).Power(r);

            return Power(l, r);
        }

        protected static Node ReduceDivide(Node l, Node r)
        {
            if (Is0(l))
                return Number(0.0);

            if (Is1(r))
                return l;

            if (l is NumberNode && r is NumberNode)
                return ((NumberNode)l).Divide(r);

            return Divide(l, r);
        }

        #endregion
    }
}
