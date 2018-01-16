using NUnit.Framework;

/// TClientServer.cs Copyright Guidance Automation Ltd

using System;
using System.Net;
using System.Threading;

namespace UDPCasts.Test
{
    [TestFixture]
    public class TClientServer
    {
        private static int Port = 8080;

        private AutoResetEvent castReceived = new AutoResetEvent(false);

        private ByteArrayCast receivedCast;

        [Test]
        [TestCase(3, 20)]
        public void UDP_LocalClientSever(int numSessions = 1, int numCasts = 5)
        {
            for (int i = 0; i < numSessions; i++)
            {
                CastTest(numCasts);
                Thread.Sleep(1000);
            }
        }

        private void CastTest(int numCasts)
        {
            byte tick = 0;

            using (UDPServer<ByteArrayCast> UDPServer = new UDPServer<ByteArrayCast>(Port))
            {
                using (UDPClient<ByteArrayCast> udpClient = new UDPClient<ByteArrayCast>(Port))
                {
                    udpClient.Received += UdpClient_Received;
                    UDPServer.AddSubscriber(IPAddress.Loopback);

                    for (int i = 0; i < numCasts; i++)
                    {
                        byte[] data1 = new byte[] { 1, 2, 3, 4 };
                        byte[] data2 = new byte[] { 4, 3, 2, 1 };

                        ByteArrayCast sentCast = new ByteArrayCast(tick, new byte[][] { data1, data2 });
                        UDPServer.Broadcast(sentCast);

                        if (castReceived.WaitOne(5000))
                        {
                            Assert.AreEqual(receivedCast, sentCast);
                        }
                        else
                        {
                            throw new Exception("Receive failure");
                        }

                        tick++;
                    }
                }
            }
        }

        private void UdpClient_Received(ByteArrayCast cast)
        {
            receivedCast = cast;
            castReceived.Set();
        }
    }
}