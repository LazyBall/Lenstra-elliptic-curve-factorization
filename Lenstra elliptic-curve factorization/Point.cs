using System.Numerics;

namespace Lenstra_elliptic_curve_factorization
{
    class Point
    {
        public readonly BigInteger x;
        public readonly BigInteger y;

        public Point(BigInteger x, BigInteger y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
