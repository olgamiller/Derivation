/*
Derivation
  
Written in 2015 by <Olga Miller> <olga.rgb@googlemail.com>
To the extent possible under law, the author(s) have dedicated all copyright and related and neighboring rights to this software to the public domain worldwide.
This software is distributed without any warranty.
You should have received a copy of the CC0 Public Domain Dedication along with this software. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.
*/

using Derivation.CommonMath;
using System;

namespace Derivation.Nodes
{
    public class NumberNode : Node
    {
        public double Value { get; private set; }

        internal NumberNode(double value)
        {
            Value = value;
        }

        public override T Apply<T>(IMath<T> m, T x, T y, T z, T t)
        {
            return m.Number(Value);
        }

        public override Node Derive(ParameterNode p)
        {
            return Number(0.0);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        internal Node Add(Node node)
        {
            return Number(Value + ((NumberNode)node).Value);
        }

        internal Node Subtract(Node node)
        {
            return Number(Value - ((NumberNode)node).Value);
        }

        internal Node Multiply(Node node)
        {
            return Number(Value * ((NumberNode)node).Value);
        }

        internal Node Power(Node node)
        {
            return Number(Math.Pow(Value, ((NumberNode)node).Value));
        }

        internal Node Divide(Node node)
        {
            return Number(Value / ((NumberNode)node).Value);
        }

        internal Node Negate()
        {
            return Number(-Value);
        }
    }
}
