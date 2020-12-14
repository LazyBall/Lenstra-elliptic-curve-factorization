using System;
using System.Numerics;

namespace Lenstra_elliptic_curve_factorization
{
    class Program
    {
        static void Main(string[] args)
        {
            BigInteger a = BigInteger.Parse("274876858367");
            BigInteger b = BigInteger.Parse("68718952447");
            BigInteger c = BigInteger.Parse("10501");

            Console.WriteLine(LenstraEllipticCurveFactorization.Factorize(
            661643));
        }
    }
}
