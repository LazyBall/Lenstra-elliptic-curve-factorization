using System;
using System.Collections.Generic;

namespace Lenstra_elliptic_curve_factorization
{
    class ListOfPrimes
    {
        private readonly List<int> _primes;

        public ListOfPrimes()
        {
            this._primes = new List<int>() { 2, 3 };
        }

        public int this[int index]
        {
            get
            {
                while (this._primes.Count <= index)
                {
                    this._primes.Add(GetNextPrime());
                }

                return this._primes[index];
            }
        }

        private int GetNextPrime()
        {
            int candidate = this._primes[^1];

            do
            {
                candidate += 2;
                int limit = (int)Math.Ceiling(Math.Sqrt(candidate));
                bool isPrime = true;

                for (int i = 1; i < this._primes.Count && isPrime; i++)
                {
                    int prime = this._primes[i];
                    if (prime > limit) break;
                    else isPrime = (candidate % prime != 0);
                }

                if (isPrime) return candidate;
            } while (true);
        }
    }
}
