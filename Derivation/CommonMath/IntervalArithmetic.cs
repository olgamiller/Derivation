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
    // [a, b[
    public struct Interval
    {
        public double Min { get; set; }
        public double Max { get; set; }

        public Interval(double min, double max)
            : this()
        {
            Min = min;
            Max = max;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", Min, Max);
        }

        public Interval Intersect(Interval interval)
        {
            return new Interval(Math.Max(Min, interval.Min), Math.Min(Max, interval.Max));
        }

        public bool IsEmpty()
        {
            return Max < Min;
        }
    }

    public class IntervalArithmetic : IMath<Interval>
    {
        public Interval Add(Interval t1, Interval t2)
        {
            return new Interval(t1.Min + t2.Min, t1.Max + t2.Max);
        }

        public Interval Subtract(Interval t1, Interval t2)
        {
            return new Interval(t1.Min - t2.Max, t1.Max - t2.Min);
        }

        public Interval Negate(Interval t)
        {
            return new Interval(-t.Max, -t.Min);
        }

        public Interval Divide(Interval t1, Interval t2)
        {
            double v1 = t1.Min / t2.Min;
            double v2 = t1.Min / t2.Max;
            double v3 = t1.Max / t2.Min;
            double v4 = t1.Max / t2.Max;

            return new Interval(Minimum(v1, v2, v3, v4), Maximum(v1, v2, v3, v4));
        }

        public Interval Multiply(Interval t1, Interval t2)
        {
            double v1 = t1.Min * t2.Min;
            double v2 = t1.Min * t2.Max;
            double v3 = t1.Max * t2.Min;
            double v4 = t1.Max * t2.Max;

            return new Interval(Minimum(v1, v2, v3, v4), Maximum(v1, v2, v3, v4));
        }

        public Interval Power(Interval t1, Interval t2)
        {
            double v1 = Math.Pow(t1.Min, t2.Min);
            double v2 = Math.Pow(t1.Min, t2.Max);
            double v3 = Math.Pow(t1.Max, t2.Min);
            double v4 = Math.Pow(t1.Max, t2.Max);

            return new Interval(Minimum(v1, v2, v3, v4), Maximum(v1, v2, v3, v4));
        }

        public Interval Sin(Interval t)
        {
            return FunctionInterval(Math.Sin(t.Min), Math.Sin(t.Max));
        }

        public Interval Cos(Interval t)
        {
            return FunctionInterval(Math.Cos(t.Min), Math.Cos(t.Max));
        }

        public Interval Sqrt(Interval t)
        {
            return new Interval(Math.Sqrt(t.Min), Math.Sqrt(t.Max));
        }

        public Interval Ln(Interval t)
        {
            return new Interval(Math.Log(t.Min), Math.Log(t.Max));
        }

        public Interval Exp(Interval t)
        {
            return new Interval(Math.Exp(t.Min), Math.Exp(t.Max));
        }

        public Interval PI() { return NumberInterval(Math.PI); }

        public Interval E() { return NumberInterval(Math.E); }

        public Interval Number(double d) { return NumberInterval(d); }

        private double Minimum(double d1, double d2, double d3, double d4)
        {
            return Math.Min(Math.Min(d1, d2), Math.Min(d3, d4));
        }

        private double Maximum(double d1, double d2, double d3, double d4)
        {
            return Math.Max(Math.Max(d1, d2), Math.Max(d3, d4));
        }

        private Interval FunctionInterval(double d1, double d2)
        {
            return new Interval(Math.Min(d1, d2), Math.Max(d1, d2));
        }

        private Interval NumberInterval(double d) { return new Interval(d, d); }
    }
}
