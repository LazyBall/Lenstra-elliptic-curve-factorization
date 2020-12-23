using System;
using System.Numerics;

namespace Lenstra_elliptic_curve_factorization
{
    public class LenstraECFactorization
    {
        public int Attempts { get; private set; }

        public int SizeOfBase { get; private set; }

        private readonly ListOfPrimes _primes;

        public LenstraECFactorization(int attempts = 1000, int baseSize = 10)
        {
            if (attempts < 1) throw new ArgumentOutOfRangeException();
            if (baseSize < 1) throw new ArgumentOutOfRangeException();
            this.Attempts = attempts;
            this.SizeOfBase = baseSize;
            this._primes = new ListOfPrimes();
        }

        public BigInteger Factorize(BigInteger number, IProgress<double> progress = null)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            BigInteger result = BigInteger.One;

            for (int i = 0; result.IsOne && i < this.Attempts; i++)
            {               
                Point p = new Point(random.Next(), random.Next());
                EllipticCurve curve;
                {
                    BigInteger a = random.Next();
                    BigInteger b = BigInteger.Pow(p.Y, 2) - BigInteger.Pow(p.X, 3) - a * p.X;
                    curve = new EllipticCurve(a, b, number);
                }

                result = RunRound(p, curve);
                progress?.Report((i + 1.0) / this.Attempts);
            }

            return result;
        }

        private BigInteger RunRound(Point p, EllipticCurve curve)
        {
            BigInteger result = BigInteger.One;

            for (int i = 0; i < this.SizeOfBase; i++)
            {
                int prime = this._primes[i];
                int alpha = (int)Math.Floor(0.5 *
                    BigInteger.Log(curve.Modulus) / BigInteger.Log(prime));

                for (int k = 0; k <= alpha; k++)
                {
                    //Console.WriteLine("prime: {0}, P: {1}, Curve - {2}", prime, p, curve);
                    try
                    {
                        p = curve.Multiply(prime, p);
                        if (p == EllipticCurve.PointAtInfinity) return result;
                    }
                    catch (EllipticCurve.ComputationException exception)
                    {
                        return exception.ElementGCD;
                    }
                }
            }

            return result;
        }
    }
}
