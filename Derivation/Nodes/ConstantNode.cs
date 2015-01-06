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
    public abstract class ConstantNode : Node
    {
        private string mToken;

        internal ConstantNode(string token)
        {
            mToken = token;
        }

        public override Node Derive(ParameterNode p)
        {
            return Number(0.0);
        }

        public override string ToString()
        {
            return mToken;
        }
    }

    public class ENode : ConstantNode
    {
        internal ENode()
            : base("E")
        {
        }

        public override T Apply<T>(IMath<T> m, T x, T y, T z, T t)
        {
            return m.E();
        }
    }

    public class PiNode : ConstantNode
    {
        internal PiNode()
            : base("PI")
        {
        }

        public override T Apply<T>(IMath<T> m, T x, T y, T z, T t)
        {
            return m.PI();
        }
    }
}
