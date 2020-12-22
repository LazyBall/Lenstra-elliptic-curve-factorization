using System.Numerics;

namespace Lenstra_elliptic_curve_factorization
{
    class Multiplier
    {
        private readonly BigInteger _a;
        private readonly BigInteger _mod;

        public Multiplier(BigInteger a, BigInteger mod)
        {
            this._a = a;
            this._mod = mod;
        }

        private Point AddPoints(Point Q, Point P, out BigInteger divisor) //сложение точек
        {
            divisor = BigInteger.Zero;

            if (Q.X == 0 && Q.Y == 0)
                return P;

            if (P.X == 0 && P.Y == 0)
                return Q;

            BigInteger d = ExtendedGCD(P.X - Q.X, _mod, out BigInteger x, out _);

            BigInteger lm = (P.Y - Q.Y) * x % _mod;
            BigInteger xr = (BigInteger.ModPow(lm, 2, _mod) - P.X - Q.X) % _mod;
            BigInteger yr = (lm * (Q.X - xr) - Q.Y) % _mod;

            if (d > 1 && d < _mod)
            {
                divisor = d;
            }

            return new Point(xr, yr);
        }

        private Point DoublePoint(Point P) //удвоение точки
        {
            if (P.X == 0 && P.Y == 0)
                return P;

            ExtendedGCD(2 * P.Y, _mod, out BigInteger x, out _);

            BigInteger lm = (3 * BigInteger.ModPow(P.X, 2, _mod) + _a) * x % _mod;
            BigInteger xr = (BigInteger.ModPow(lm, 2, _mod) - 2 * P.X) % _mod;
            BigInteger yr = (lm * (P.X - xr) - P.Y) % _mod;

            return new Point(xr, yr);
        }

        private BigInteger ExtendedGCD(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            BigInteger x2 = 1, x1 = 0, y2 = 0, y1 = 1;

            while (b != 0)
            {
                BigInteger q = BigInteger.DivRem(a, b, out BigInteger r);
                x = x2 - q * x1;
                y = y2 - q * y1;
                a = b;
                b = r;
                x2 = x1;
                x1 = x;
                y2 = y1;
                y1 = y;
            }

            x = x2;
            y = y2;
            return a;
        }

        public Point Mult(Point P, BigInteger factor, out BigInteger divisor)
        {
            divisor = BigInteger.Zero;
            Point N = P;
            Point Q = new Point(0, 0);

            while ((!factor.IsZero) && divisor.IsZero)
            {
                if (!factor.IsEven)
                {
                    Q = AddPoints(Q, N, out divisor);
                }
                N = DoublePoint(N);
                factor >>= 1;
            }

            return Q;
        }
    }
}
