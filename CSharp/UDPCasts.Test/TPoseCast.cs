using System;
using NUnit.Framework;
using System.Net;

namespace UDPCasts.Test
{
    [TestFixture]
    [Category("PoseCast")]
    public class TPoseCast
    {
        private double tol = 1e-6;

        [Test]
        [TestCase("192.168.4.16", 10, 0, Math.PI)]
        [TestCase("127.0.0.1", -1, -2, -Math.PI/2)]
        public void Constructor(string ipString, double x, double y, double heading)
        {
            IPAddress ipAddress = System.Net.IPAddress.Parse(ipString);

            PoseCast cast = new PoseCast(1, ipAddress, x, y, heading);

            Assert.AreEqual(cast.IPAddress, ipAddress);
            Assert.That(cast.X, Is.EqualTo(x).Within(tol));
            Assert.That(cast.Y, Is.EqualTo(y).Within(tol));
            Assert.That(cast.Heading, Is.EqualTo(heading).Within(tol));
        }
    }
}
