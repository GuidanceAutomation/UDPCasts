using NUnit.Framework;
using System.Net;

namespace UDPCasts.Test
{
    public abstract class AbstractClientServerTest<T>
    {
        protected static int port = 8080;

        private UDPServer<T> server;

        private UDPClient<T> client;

        public UDPServer<T> Server { get { return server; } }

        public UDPClient<T> Client { get { return client; } }

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            server = new UDPServer<T>(port);
            client = new UDPClient<T>(port);

            server.AddSubscriber(IPAddress.Loopback);
        }

        [OneTimeTearDown]
        public virtual void OneTimeTearDown()
        {
            client.Dispose();
            server.Dispose();
        }
    }
}
