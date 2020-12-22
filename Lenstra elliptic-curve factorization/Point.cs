using System;
using System.Numerics;

namespace Lenstra_elliptic_curve_factorization
{
    public class Point : ICloneable
    {
        public BigInteger X { get; private set; }
        public BigInteger Y { get; private set; }

        public Point(BigInteger x, BigInteger y)
        {
            this.X = x;
            this.Y = y;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override bool Equals(object other)
        {
            if(other is Point p)
            {
                return this.X == p.X && this.Y == p.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() + this.Y.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("X = {0}, Y = {1}", this.X, this.Y);
        }

        public static implicit operator Point((BigInteger x, BigInteger y) pair)
        {
            return new Point(pair.x, pair.y);
        }
    }
}
