using System.Net;

namespace UDPCasts
{
    public class BytesUDPClient : UDPClient<byte[]>
    {
        public BytesUDPClient(int port, IPAddress ipAddress = default(IPAddress))
            : base(port, ipAddress)
        {
        }

        ~BytesUDPClient()
        {
            Dispose(false);
        }

        private void Dispose(bool isDisposing)
        {
            base.Dispose();
        }
    }
}