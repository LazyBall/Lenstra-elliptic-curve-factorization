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
            BigInteger d = BigInteger.Parse("113");
            LenstraECFactorization factorization = new
                LenstraECFactorization(1000, 20);

            Console.WriteLine(factorization.Factorize1(a*b));
        }
    }
}
