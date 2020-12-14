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
            foreach (var t in LenstraEllipticCurveFactorization.Factorize(
                a*100003*100019*100043*100057, 100, 10000000))
            {
                Console.WriteLine(t);
            }
        }
    }
}
