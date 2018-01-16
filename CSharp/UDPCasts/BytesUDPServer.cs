namespace UDPCasts
{
    public class BytesUDPServer : UDPServer<byte[]>
    {
        public BytesUDPServer(int port)
            : base(port)
        {
        }

        ~BytesUDPServer()
        {
            Dispose(false);
        }

        private void Dispose(bool isDisposing)
        {
            base.Dispose();
        }
    }
}