using System;
using System.Numerics;

namespace Lenstra_elliptic_curve_factorization
{
    public class EllipticCurve
    {
        public const Point PointAtInfinity = null;

        public BigInteger A { get; private set; }

        public BigInteger B { get; private set; }

        public BigInteger Modulus { get; private set; }

        public EllipticCurve(BigInteger a, BigInteger b, BigInteger modulus)
        {
            if (modulus < 4) throw new ArgumentException();
            this.Modulus = modulus;
            this.A = a.PosMod(this.Modulus);
            this.B = b.PosMod(this.Modulus);
        }

        private Point PreparePoint(Point p)
        {
            if (p == PointAtInfinity) return PointAtInfinity;
            return new Point(p.X.PosMod(this.Modulus), p.Y.PosMod(this.Modulus));
        }

        private Point AddPreparedPoints(Point p1, Point p2)
        {
            if (p1 == PointAtInfinity) return p2;
            if (p2 == PointAtInfinity) return p1;

            if (p1.X != p2.X)
            {
                BigInteger lambda = Helper.ExtendedGCD(p2.X - p1.X, this.Modulus,
                    out BigInteger inverse, out _);
                if (lambda != 1 && lambda != this.Modulus) throw new ComputationException(lambda);

                lambda = (p2.Y - p1.Y) * inverse;
                BigInteger xr = BigInteger.ModPow(lambda, 2, this.Modulus) - p1.X - p2.X;
                BigInteger yr = lambda * (p1.X - xr) - p1.Y;
                return new Point(xr.PosMod(this.Modulus), yr.PosMod(this.Modulus));
            }
            else if (p1.Y == (-p2.Y).PosMod(this.Modulus))
            {
                return PointAtInfinity;
            }
            else
            {
                return DoublePoint(p1);
            }
        }

        public Point AddPoints(Point p, Point q)
        {
            if (!IsOnCurve(p)) throw new ArgumentException();
            if (!IsOnCurve(q)) throw new ArgumentException();
            p = PreparePoint(p);
            q = PreparePoint(q);
            return AddPreparedPoints(p, q);
        }

        private Point DoublePoint(Point p)
        {
            BigInteger lambda = Helper.ExtendedGCD(2 * p.Y, this.Modulus,
                out BigInteger inverse, out _);
            if (lambda != 1 && lambda != this.Modulus) throw new ComputationException(lambda);

            lambda = (3 * BigInteger.ModPow(p.X, 2, this.Modulus) + this.A) * inverse;
            BigInteger xr = BigInteger.ModPow(lambda, 2, this.Modulus) - 2 * p.X;
            BigInteger yr = lambda * (p.X - xr) - p.Y;
            return new Point(xr.PosMod(this.Modulus), yr.PosMod(this.Modulus));
        }

        private Point LocalNegate(Point p)
        {
            if (p == PointAtInfinity) return PointAtInfinity;
            return new Point(p.X.PosMod(this.Modulus), (-p.Y).PosMod(this.Modulus));
        }

        public Point Negate(Point p)
        {
            if (!IsOnCurve(p)) throw new ArgumentException();
            return LocalNegate(p);
        }

        public bool IsOnCurve(Point p)
        {
            if (p == PointAtInfinity) return true;
            return (BigInteger.ModPow(p.Y, 2, this.Modulus) -
                BigInteger.ModPow(p.X, 3, this.Modulus) - this.A * p.X - this.B)
                % this.Modulus == 0;
        }

        public Point Multiply(BigInteger factor, Point p)
        {
            if (!IsOnCurve(p)) throw new ArgumentException();
            if (factor.IsZero || p == PointAtInfinity) return PointAtInfinity;
            p = PreparePoint(p);

            bool negate = false;
            if (factor.Sign < 0)
            {
                negate = true;
                factor *= -1;
            }

            Point degree = (Point)p.Clone();
            Point result = PointAtInfinity;

            while (!factor.IsZero)
            {
                if (!factor.IsEven)
                {
                    result = AddPreparedPoints(result, degree);
                }
                degree = DoublePoint(degree);
                factor >>= 1;
            }

            return negate ? LocalNegate(result) : result;
        }

        public override string ToString()
        {
            return string.Format("A = {0}, B = {1}, Modulus = {2}",
                this.A, this.B, this.Modulus);
        }

        public class ComputationException : Exception
        {
            public BigInteger ElementGCD { get; private set; }
            public ComputationException(BigInteger elementGCD)
            {
                this.ElementGCD = elementGCD;
            }
        }
    }
}
