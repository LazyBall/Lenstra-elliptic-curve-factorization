using System;
using System.Numerics;
using System.Threading.Tasks;

namespace Lenstra_elliptic_curve_factorization
{
    class LenstraECFactorization
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

        public BigInteger Factorize(BigInteger number)
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
            }

            return result;
        }

        public BigInteger Factorize1(BigInteger number)
        {
            Task<BigInteger>[] tasks = new Task<BigInteger>[Environment.ProcessorCount];

            for(int i=0; i<tasks.Length; i++)
            {
                tasks[i] = Task.Run(() => Factorize(number));
            }

            int index = Task.WaitAny(tasks);

            if (index >= 0) return tasks[index].Result;

            return BigInteger.One;
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
