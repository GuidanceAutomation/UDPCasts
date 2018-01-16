using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UDPCasts.Test
{
    [TestFixture]
    public class TUDPClient
    {
        [Test]
        [TestCase(41918)]
        public void Init(int port)
        {
            UDPClient<ByteArrayCast> udpClient = new UDPClient<ByteArrayCast>(port);

            Assert.AreEqual(port, udpClient.Port);
        }
    }
}