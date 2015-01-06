/*
Derivation
  
Written in 2015 by <Olga Miller> <olga.rgb@googlemail.com>
To the extent possible under law, the author(s) have dedicated all copyright and related and neighboring rights to this software to the public domain worldwide.
This software is distributed without any warranty.
You should have received a copy of the CC0 Public Domain Dedication along with this software. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.
*/

namespace Derivation.CommonMath
{
    public interface IMath<T>
    {
        T Add(T t1, T t2);
        T Subtract(T t1, T t2);
        T Negate(T t);
        T Divide(T t1, T t2);
        T Multiply(T t1, T t2);
        T Power(T t1, T t2);

        T Sin(T t);
        T Cos(T t);
        T Sqrt(T t);
        T Ln(T t);
        T Exp(T t);

        T PI();
        T E();
        T Number(double d);
    }
}
