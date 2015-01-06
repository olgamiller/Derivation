/*
Derivation
  
Written in 2015 by <Olga Miller> <olga.rgb@googlemail.com>
To the extent possible under law, the author(s) have dedicated all copyright and related and neighboring rights to this software to the public domain worldwide.
This software is distributed without any warranty.
You should have received a copy of the CC0 Public Domain Dedication along with this software. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.
*/

using System;

namespace Derivation.CommonMath
{
    public class PointMath : IMath<double>
    {
        public double Add(double t1, double t2) { return t1 + t2; }
        public double Subtract(double t1, double t2) { return t1 - t2; }
        public double Negate(double t) { return -t; }
        public double Divide(double t1, double t2) { return t1 / t2; }
        public double Multiply(double t1, double t2) { return t1 * t2; }
        public double Power(double t1, double t2) { return Math.Pow(t1, t2); }

        public double Sin(double t) { return Math.Sin(t); }
        public double Cos(double t) { return Math.Cos(t); }
        public double Sqrt(double t) { return Math.Sqrt(t); }
        public double Ln(double t) { return Math.Log(t); }
        public double Exp(double t) { return Math.Exp(t); }

        public double PI() { return Math.PI; }
        public double E() { return Math.E; }
        public double Number(double d) { return d; }
    }
}
