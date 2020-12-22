using System;
using System.Numerics;

namespace Lenstra_elliptic_curve_factorization
{
    class Program
    {
        static void Main(string[] args)
        {
            BigInteger a = BigInteger.Parse("4469743212299");
            BigInteger b = BigInteger.Parse("440074369743212299");
            BigInteger c = BigInteger.Parse("4400743697434543212277");
            LenstraECFactorization factorization = new
                LenstraECFactorization(1000, 100);

            Console.WriteLine(factorization.Factorize(a*b*c));
        }
    }
}
