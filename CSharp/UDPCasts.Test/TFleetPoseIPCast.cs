using NUnit.Framework;
using System;
using System.Linq;
using System.Net;

namespace UDPCasts.Test
{
    [TestFixture]
    [Category("FleetPoseIPCast")]
    public class TFleetPoseIPCast
    {
        private PoseIPStruct alpha;
        private PoseIPStruct beta;
        private byte[][] testData;

        [Test]
        [TestCase(255)]
        public void Init(byte tick)
        {
            FleetPoseIPCast cast = new FleetPoseIPCast(tick, testData);

            Assert.AreEqual(tick, cast.Tick);
            CollectionAssert.IsNotEmpty(cast.DataSet);

            PoseIPStruct alphaDecoded = cast.DataSet.First();
            Assert.AreEqual(alpha.IPAddress, alphaDecoded.IPAddress);
            Assert.AreEqual(alpha.X, alphaDecoded.X);
            Assert.AreEqual(alpha.Y, alphaDecoded.Y);
            Assert.AreEqual(alpha.Heading, alphaDecoded.Heading);

            PoseIPStruct betaDecoded = cast.DataSet.Last();
            Assert.AreEqual(beta.IPAddress, betaDecoded.IPAddress);
            Assert.AreEqual(beta.X, betaDecoded.X);
            Assert.AreEqual(beta.Y, betaDecoded.Y);
            Assert.AreEqual(beta.Heading, betaDecoded.Heading);
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            alpha = new PoseIPStruct(IPAddress.Parse("192.66.1.10"), 1, 1, Math.PI / 2);
            beta = new PoseIPStruct(IPAddress.Parse("192.66.1.11"), -1, -1, -Math.PI / 2);

            testData = new byte[2][];
            testData[0] = alpha.Bytes;
            testData[1] = beta.Bytes;
        }
    }
}