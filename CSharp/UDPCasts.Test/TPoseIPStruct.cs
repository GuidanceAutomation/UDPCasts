using NUnit.Framework;
using System;
using System.Net;
using System.Text.RegularExpressions;

namespace UDPCasts.Test
{
    [TestFixture]
    [Category("PoseIPStruct")]
    public class TPoseIPStruct
    {
        private double tol = 1e-3;

        Regex poseStringRegex = new Regex(@"(?:X:) (?<xVal>\d{1,}.\d{1,}), (?:Y:) (?<yVal>\d{1,}.\d{1,}), (?:Heading:) (?<headingVal>\d{1,}.\d{1,}) (?:\(rad\))");


        Regex degPoseStringRegex = new Regex(@"(?:X:) (?<xVal>\d{1,}.\d{1,}), (?:Y:) (?<yVal>\d{1,}.\d{1,}), (?:Heading:) (?<headingVal>\d{1,}.\d{1,}) (?:\(deg\))");

        [Test]
        [TestCase(0.123, 4.567, 0)]
        [TestCase(0.123, 4.567, 90)]
        public void ToPoseStringDeg(double x, double y, double headingDeg)
        {
            double headingRad = headingDeg.DegToRad();

            PoseIPStruct poseIPStruct = new PoseIPStruct(IPAddress.Loopback, x, y, headingRad);
            string poseString = poseIPStruct.ToPoseDegString();

            Match match = degPoseStringRegex.Match(poseString);

            double xVal = double.Parse(match.Groups["xVal"].Value);
            double yVal = double.Parse(match.Groups["yVal"].Value);
            double headingVal = double.Parse(match.Groups["headingVal"].Value);

            Assert.That(x, Is.EqualTo(xVal).Within(0.01));
            Assert.That(y, Is.EqualTo(yVal).Within(0.01));
            Assert.That(headingDeg, Is.EqualTo(headingVal).Within(0.001));
        }

        [Test]
        [TestCase(0.123, 4.567, Math.PI/2)]
        [TestCase(0.123,4.567,3.14)]
        public void ToPoseString(double x, double y, double headingRad)
        {
            PoseIPStruct poseIPStruct = new PoseIPStruct(IPAddress.Loopback, x, y, headingRad);
            string poseString = poseIPStruct.ToPoseIPString();
            
            Match match = poseStringRegex.Match(poseString);

            double xVal = double.Parse(match.Groups["xVal"].Value);
            double yVal = double.Parse(match.Groups["yVal"].Value);
            double headingVal = double.Parse(match.Groups["headingVal"].Value);

            Assert.That(x, Is.EqualTo(xVal).Within(0.01));
            Assert.That(y, Is.EqualTo(yVal).Within(0.01));
            Assert.That(headingRad, Is.EqualTo(headingVal).Within(0.001));
        }       


        [Test]
        [TestCase("192.168.4.16", 10, 0, Math.PI)]
        [TestCase("127.0.0.1", -1, -2, -Math.PI / 2)]
        public void Constructor(string ipString, double x, double y, double heading)
        {
            IPAddress ipAddress = System.Net.IPAddress.Parse(ipString);

            PoseIPStruct cast = new PoseIPStruct(ipAddress, x, y, heading);

            Assert.AreEqual(cast.IPAddress, ipAddress);
            Assert.That(cast.X, Is.EqualTo(x).Within(tol));
            Assert.That(cast.Y, Is.EqualTo(y).Within(tol));
            Assert.That(cast.Heading, Is.EqualTo(heading).Within(tol));

            byte[] rawBytes = cast.Bytes;

            PoseIPStruct cloneCast = new PoseIPStruct(rawBytes);

            Assert.AreEqual(cloneCast.IPAddress, ipAddress);
            Assert.That(cloneCast.X, Is.EqualTo(x).Within(tol));
            Assert.That(cloneCast.Y, Is.EqualTo(y).Within(tol));
            Assert.That(cloneCast.Heading, Is.EqualTo(heading).Within(tol));
        }
    }
}