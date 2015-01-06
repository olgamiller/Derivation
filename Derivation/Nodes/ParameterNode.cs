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
    public class ParameterNode : Node
    {
        private static int mCount = 0;

        private string mName;
        private int mPos;

        private ParameterNode()
        {           
            if (mCount > 3)
                mCount = 0;

            mPos = mCount; 
            
            mCount++;
        }

        internal ParameterNode(string name)
            : this()
        {
            mName = name;
        }

        public override T Apply<T>(IMath<T> m, T x, T y, T z, T t)
        {
            switch (mPos)
            {
                case 0:
                    return x;
                case 1:
                    return y;
                case 2:
                    return z;
                case 3:
                    return t;
                default:
                    throw new Exception();
            }
        }

        public override Node Derive(ParameterNode p)
        {
            if (mPos == p.mPos)
                return Number(1.0);

            return Number(0.0);
        }

        public override string ToString()
        {
            return mName;
        }
    }
}
