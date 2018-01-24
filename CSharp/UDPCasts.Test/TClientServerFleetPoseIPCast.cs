using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Threading;
using System.Net;

namespace UDPCasts.Test
{
    public class TClientServerFleetPoseIPCast : AbstractClientServerTest<FleetPoseIPCast>
    {
        private TimeSpan timeout = TimeSpan.FromMilliseconds(10000);

        private PoseIPStruct alpha = new PoseIPStruct(IPAddress.Parse("192.168.0.10"), 1, 1, Math.PI / 2);

        private PoseIPStruct beta = new PoseIPStruct(IPAddress.Parse("192.168.0.11"), -1, -1, -Math.PI / 2);

        [Test]
        [Order(1)]
        public void Serialization()
        {
            byte tick = 66;
            FleetPoseIPCast cast = new FleetPoseIPCast(tick, new[] { alpha, beta });

            AutoResetEvent messageReceived = new AutoResetEvent(false);
            FleetPoseIPCast received = null;

            Client.Received += delegate(FleetPoseIPCast receivedCast)
            {
                received = receivedCast;
                messageReceived.Set();
            };

            Server.Broadcast(cast);

            if (!messageReceived.WaitOne(timeout))
            {
                throw new TimeoutException("Failed to receive a message");
            }

            CollectionAssert.IsNotEmpty(received.DataSet);           
        }
    }
}
