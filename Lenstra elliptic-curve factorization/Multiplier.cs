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

            if (Q.x == 0 && Q.y == 0)
                return P;

            if (P.x == 0 && P.y == 0)
                return Q;

            BigInteger d = GCD(P.x - Q.x, _mod, out BigInteger x, out _);

            BigInteger lm = (P.y - Q.y) * x % _mod;
            BigInteger xr = (BigInteger.ModPow(lm, 2, _mod) - P.x - Q.x) % _mod;
            BigInteger yr = (lm * (Q.x - xr) - Q.y) % _mod;

            if (d > 1 && d < _mod)
            {
                divisor = d;
            }

            return new Point(xr, yr);
        }

        private Point DoublePoint(Point P) //удвоение точки
        {
            if (P.x == 0 && P.y == 0)
                return P;

            GCD(2 * P.y, _mod, out BigInteger x, out _);

            BigInteger lm = (3 * BigInteger.ModPow(P.x, 2, _mod) + _a) * x % _mod;
            BigInteger xr = (BigInteger.ModPow(lm, 2, _mod) - 2 * P.x) % _mod;
            BigInteger yr = (lm * (P.x - xr) - P.y) % _mod;

            return new Point(xr, yr);
        }

        private BigInteger GCD(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        //расширенный алгоритм евклида
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
