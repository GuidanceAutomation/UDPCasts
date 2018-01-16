using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace UDPCasts
{
    public class UDPServer<T> : AbstractUDPEndpoint, IDisposable
    {
        private readonly object lockObject = new object();

        private readonly int port;

        private bool isDisposed = false;

        private ReadOnlyObservableCollection<IPAddress> readOnlySubscribers;

        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        private ObservableCollection<IPAddress> subscribers = new ObservableCollection<IPAddress>();

        public UDPServer(int port)
        {
            this.port = port;
            readOnlySubscribers = new ReadOnlyObservableCollection<IPAddress>(subscribers);
        }

        ~UDPServer()
        {
            Dispose(false);
        }

        public bool IsDisposed
        {
            get { return isDisposed; }

            private set
            {
                if (isDisposed != value)
                {
                    isDisposed = value;
                    OnNotifyPropertyChanged();
                }
            }
        }

        public int Port { get { return port; } }

        public ReadOnlyObservableCollection<IPAddress> Subscribers { get { return readOnlySubscribers; } }

        public void AddSubscriber(IPAddress ipAddresss)
        {
            lock (lockObject)
            {
                if (!subscribers.Any(e => e.Equals(ipAddresss)))
                {
                    subscribers.Add(ipAddresss);
                }
            }
        }

        /// <summary>
        /// Broadcast object to all subscribers.
        /// </summary>
        /// <param name="obj">Object to be broadcast. Must be decorated with [Serializable] attribute.</param>
        public void Broadcast(T obj)
        {
            byte[] message;

            if (obj is byte[])
            {
                message = obj as byte[];
            }
            else
            {
                message = ObjectToByteArray((object)obj);
            }

            foreach (IPAddress ipAddress in subscribers.ToList())
            {
                IPEndPoint endpoint = new IPEndPoint(ipAddress, Port);
                socket.SendTo(message, endpoint);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void RemoveSubscriber(IPAddress ipAddress)
        {
            lock (lockObject)
            {
                subscribers.Remove(ipAddress);
            }
        }

        private static byte[] ObjectToByteArray(object obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        private void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            socket.Close();

            IsDisposed = true;
        }
    }
}