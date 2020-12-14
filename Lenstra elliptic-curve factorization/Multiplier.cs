using System.Collections.Generic;
using System.Numerics;

namespace Lenstra_elliptic_curve_factorization
{
    class Multiplier
    {
        private readonly BigInteger _a;
        private readonly BigInteger _mod;
        private readonly HashSet<BigInteger> _divisors;

        public IReadOnlyCollection<BigInteger> Divisors => this._divisors;

        public Multiplier(BigInteger a, BigInteger mod)
        {
            this._a = a;
            this._mod = mod;
            this._divisors = new HashSet<BigInteger>();
        }

        private Point AddPoints(Point Q, Point P) //сложение точек
        {
            if (Q.x == 0 && Q.y == 0)
                return P;

            if (P.x == 0 && P.y == 0)
                return Q;

            BigInteger d = GCD(P.x - Q.x, _mod, out BigInteger x, out BigInteger y);

            BigInteger lm = (P.y - Q.y) * x % _mod;
            BigInteger xr = (BigInteger.ModPow(lm, 2, _mod) - P.x - Q.x) % _mod;
            BigInteger yr = (lm * (Q.x - xr) - Q.y) % _mod;

            if (d > 1 && d < _mod)
            {
                this._divisors.Add(d);
            }
            return new Point(xr, yr);
        }

        private Point DoublePoint(Point P) //удвоение точки
        {
            if (P.x == 0 && P.y == 0)
                return P;

            BigInteger d = GCD(2 * P.y, _mod, out BigInteger x, out BigInteger y);

            BigInteger lm = (3 * BigInteger.ModPow(P.x, 2, _mod) + _a) * x % _mod;
            BigInteger xr = (BigInteger.ModPow(lm, 2, _mod) - 2 * P.x) % _mod;
            BigInteger yr = (lm * (P.x - xr) - P.y) % _mod;

            if (d > 1 && d < _mod)
            {
                this._divisors.Add(d);
            }

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

        public Point Mult(Point P, BigInteger d)
        {
            Point N = P;
            Point Q = new Point(0, 0);

            while (!d.IsZero)
            {
                if (!d.IsEven)
                {
                    Q = AddPoints(Q, N);
                }
                N = DoublePoint(N);
                d >>= 1;
            }

            return Q;
        }
    }
}
