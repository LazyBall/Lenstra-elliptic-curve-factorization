using Lenstra_elliptic_curve_factorization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace LenstraECFactorizationUnitTests
{
    [TestClass]
    public class EllipticCurveUnitTests
    {

        [TestMethod]
        public void TestAddPoints()
        {
            EllipticCurve curve = new EllipticCurve(a: 1, b: 1, modulus: 23);
            Point[] expected = new Point[]{
                (0,1), (6,19), (3,13), (13,16), (18,3), (7,11),
                (11,3), (5,19), (19,18), (12,4), (1,16), (17,20),
                (9,16), (4,0), (9,7), (17,3), (1,7), (12,19),
                (19,5), (5,4), (11,20), (7,12), (18,20), (13,7),
                (3,10), (6,4), (0,22) , null
            };

            Point p = (0, 1);
            Point[] actual = new Point[expected.Length];
            actual[0] = p;

            for (int i = 1; i < expected.Length; i++)
            {
                actual[i] = curve.AddPoints(actual[i - 1], p);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestAddPointsSwapArg()
        {
            EllipticCurve curve = new EllipticCurve(a: 1, b: 1, modulus: 23);
            Point[] expected = new Point[]{
                (0,1), (6,19), (3,13), (13,16), (18,3), (7,11),
                (11,3), (5,19), (19,18), (12,4), (1,16), (17,20),
                (9,16), (4,0), (9,7), (17,3), (1,7), (12,19),
                (19,5), (5,4), (11,20), (7,12), (18,20), (13,7),
                (3,10), (6,4), (0,22) , null
            };

            Point p = (0, 1);
            Point[] actual = new Point[expected.Length];
            actual[0] = p;

            for (int i = 1; i < expected.Length; i++)
            {
                actual[i] = curve.AddPoints(p, actual[i - 1]);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMult()
        {
            EllipticCurve curve = new EllipticCurve(a: 1, b: 1, modulus: 23);
            Point[] expected = new Point[]{
                (0,1), (6,19), (3,13), (13,16), (18,3), (7,11),
                (11,3), (5,19), (19,18), (12,4), (1,16), (17,20),
                (9,16), (4,0), (9,7), (17,3), (1,7), (12,19),
                (19,5), (5,4), (11,20), (7,12), (18,20), (13,7),
                (3,10), (6,4), (0,22) , null
            };

            Point p = (0, 1);
            Point[] actual = new Point[expected.Length];

            for (int i = 0; i < expected.Length; i++)
            {
                actual[i] = curve.Multiply(i + 1, p);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMultBook()
        {
            EllipticCurve curve = new EllipticCurve(a: -1, b: 3231, modulus: 661643);

            Point q = (87, 2);
            q = curve.Multiply(BigInteger.Pow(2, 9), q);
            bool success = q.Equals((Point)(196083, 134895));
            q = curve.Multiply(BigInteger.Pow(3, 6), q);
            success = success && q.Equals((Point)(470021, 282574));
            Assert.IsTrue(success);
        }
    }
}
