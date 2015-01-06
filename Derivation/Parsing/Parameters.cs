/*
Derivation

Written in 2015 by <Olga Miller> <olga.rgb@googlemail.com>
To the extent possible under law, the author(s) have dedicated all copyright and related and neighboring rights to this software to the public domain worldwide.
This software is distributed without any warranty.
You should have received a copy of the CC0 Public Domain Dedication along with this software. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.
*/

using Derivation.Nodes;

namespace Derivation.Parsing
{
    public class Parameters
    {
        #region Properties

        public ParameterNode X { get; private set; }
        public ParameterNode Y { get; private set; }
        public ParameterNode Z { get; private set; }
        public ParameterNode T { get; private set; }

        #endregion

        public Parameters()
        {
            X = Node.Parameter("x");
            Y = Node.Parameter("y");
            Z = Node.Parameter("z");
            T = Node.Parameter("t");
        }
    }
}
