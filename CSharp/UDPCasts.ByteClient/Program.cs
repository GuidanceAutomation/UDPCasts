using System;
using System.Net;

namespace UDPCasts.ByteClient
{
    internal class Program
    {
        public static IPAddress IPAddress = IPAddress.Parse("127.0.0.1");

        public static int Port = 41916;

        private static void Client_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                default:
                    {
                        break;
                    }
            }
        }

        private static void Client_Received(byte[] bytes)
        {
            Console.WriteLine("Received {0}", bytes.ToHexString());
        }

        private static void Main(string[] args)
        {
            Console.Title = "ByteClient";

            Console.WriteLine("Listening on port: {0}", Port);

            using (BytesUDPClient client = new BytesUDPClient(Port, IPAddress))
            {
                client.Received += Client_Received;
                client.PropertyChanged += Client_PropertyChanged;

                bool terminate = false;

                while (!terminate)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.Q:
                            {
                                terminate = true;
                                client.Received -= Client_Received;
                                break;
                            }
                    }
                }
            }

            Console.WriteLine("Press <any> key to quit...");
            Console.ReadKey(true);
        }
    }
}