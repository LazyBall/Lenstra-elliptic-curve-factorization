using System;
using System.Collections.Generic;

namespace Lenstra_elliptic_curve_factorization
{
    class ListOfPrimes
    {
        private static readonly List<int> _primes;
        private static readonly object _lock;

        static ListOfPrimes()
        {
            _primes = new List<int>() { 2, 3 };
            _lock = new object();
        }

        public int this[int index]
        {
            get
            {
                if (_primes.Count <= index)
                {
                    lock (_lock)
                    {
                        while (_primes.Count <= index)
                        {
                            _primes.Add(GetNextPrime());
                        }
                    }
                }
                
                return _primes[index];
            }
        }

        private int GetNextPrime()
        {
            int candidate = _primes[^1];

            do
            {
                candidate += 2;
                int limit = (int)Math.Ceiling(Math.Sqrt(candidate));
                bool isPrime = true;

                for (int i = 1; i < _primes.Count && isPrime; i++)
                {
                    int prime = _primes[i];
                    if (prime > limit) break;
                    else isPrime = (candidate % prime != 0);
                }

                if (isPrime) return candidate;
            } while (true);
        }        
    }
}
