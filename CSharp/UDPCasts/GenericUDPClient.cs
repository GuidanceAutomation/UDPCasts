using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace UDPCasts
{
    public class UDPClient<T> : AbstractUDPEndpoint, IDisposable
    {
        private readonly int port;
        private readonly IPAddress serverIPAddress;
        private IPEndPoint endpoint;

        private bool isDisposed = false;

        private Tuple<DateTime, Exception> lastException = null;

        private DateTime lastMessage = DateTime.MinValue;

        private UdpClient listener;

        private bool terminate = false;

        public UDPClient(int port, IPAddress serverIPAddress = default(IPAddress))
        {
            this.port = port;

            this.serverIPAddress = (serverIPAddress == null) ? IPAddress.Loopback : serverIPAddress;

            this.listener = new UdpClient(port);
            this.endpoint = new IPEndPoint(this.serverIPAddress, port);

            Thread listenThread = new Thread(new ThreadStart(ListenThread));
            listenThread.Start();
        }

        ~UDPClient()
        {
            Dispose(false);
        }

        public event Action<T> Received;

        public bool IsDisposed
        {
            get { return isDisposed; }

            set
            {
                if (isDisposed != value)
                {
                    isDisposed = value;
                    OnNotifyPropertyChanged();
                }
            }
        }

        public Tuple<DateTime, Exception> LastException
        {
            get { return lastException; }
        }

        public DateTime LastMessage { get { return lastMessage; } }

        public int Port { get { return port; } }

        public void Dispose()
        {
            Dispose(true);
        }

        private object BytesBoxing(byte[] bytes)
        {
            return bytes;
        }

        private void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            terminate = true;
            listener.Close();

            if (disposing)
            {
            }

            IsDisposed = true;
        }

        private void ListenThread()
        {
            while (!terminate)
            {
                try
                {
                    byte[] bytes = listener.Receive(ref endpoint);

                    T obj;

                    if (typeof(T).Name == "Byte[]")
                    {
                        obj = (T)BytesBoxing(bytes);
                    }
                    else
                    {
                        BinaryFormatter formatter = new BinaryFormatter();

                        using (MemoryStream stream = new MemoryStream(bytes))
                        {
                            obj = (T)formatter.Deserialize(stream);
                        }
                    }

                    lastMessage = DateTime.Now;
                    OnRecevied(obj);
                }
                catch (Exception ex)
                {
                    lastException = new Tuple<DateTime, Exception>(DateTime.Now, ex);
                    listener.Close();
                    Thread.Sleep(5000);
                }
            }
            listener.Close();
        }

        private void OnRecevied(T obj)
        {
            Action<T> handler = Received;
            if (handler != null)
            {
                handler.BeginInvoke(obj, null, null);
            }
        }
    }
}