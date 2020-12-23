using Lenstra_elliptic_curve_factorization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace LenstraECFactorizationUnitTests
{
    [TestClass]
    public class LenstraFactorUnitTests
    {
        [TestMethod]
        public void TestFactorizeOnBookExample()
        {
            BigInteger number = 661643;
            BigInteger[] expected = new BigInteger[] { 541, 1223 };
            BigInteger[] actual = new BigInteger[2];
            actual[0] = new LenstraECFactorization().Factorize(number);
            actual[1] = number / actual[0];

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void TestFactorizeOnMZIExample()
        {
            BigInteger number = 455839;
            BigInteger[] expected = new BigInteger[] { 599, 761 };
            BigInteger[] actual = new BigInteger[2];
            actual[0] = new LenstraECFactorization().Factorize(number);
            actual[1] = number / actual[0];

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void TestFactorizeOnBigPrimes()
        {
            BigInteger a = BigInteger.Parse("274876858367");
            BigInteger b = BigInteger.Parse("68718952447");
            BigInteger number = a * b;
            BigInteger[] expected = new BigInteger[] { a, b };
            BigInteger[] actual = new BigInteger[2];
            actual[0] = new LenstraECFactorization(baseSize: 100).Factorize(number);
            actual[1] = number / actual[0];

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void TestFactorizeOnPrime()
        {
            BigInteger number = 10061;
            BigInteger[] expected = new BigInteger[] {1, number};
            BigInteger[] actual = new BigInteger[2];
            actual[0] = new LenstraECFactorization().Factorize(number);
            actual[1] = number / actual[0];

            CollectionAssert.AreEqual(expected, actual);
        }

    }
}
