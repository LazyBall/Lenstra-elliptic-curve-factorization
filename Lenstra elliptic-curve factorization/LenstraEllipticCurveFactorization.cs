using System;
using System.Numerics;

namespace Lenstra_elliptic_curve_factorization
{
    static class LenstraEllipticCurveFactorization
    {
        public static BigInteger Factorize(BigInteger n, int m = 10, int attempts = 10000)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            ListOfPrimes primes = new ListOfPrimes();

            for (int k = 0; k < attempts; k++)
            {
                Point q = new Point(random.Next(), random.Next());
                Multiplier mult = new Multiplier(random.Next(), n);

                for (int i = 0; i < m; i++)
                {
                    int prime = primes[i];
                    int alpha = (int)Math.Floor(0.5 * BigInteger.Log(n) / BigInteger.Log(prime));

                    for (int j = 0; j < alpha; j++)
                    {
                        q = mult.Mult(q, prime, out BigInteger divisor);
                        if (!divisor.IsZero)
                        {
                            return divisor;
                        }
                    }
                }
            }

            return n;
        }
    }
}
