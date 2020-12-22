using System.Numerics;

namespace Lenstra_elliptic_curve_factorization
{
    public static class BigInteherExtension
    {
        public static BigInteger PosMod(this BigInteger number, BigInteger modulus)
        {
            number %= modulus;
            if (number.Sign < 0) number += BigInteger.Abs(modulus);
            return number;
        }
    }
}
