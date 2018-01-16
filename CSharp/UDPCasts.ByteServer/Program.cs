using System;
using System.Net;

namespace UDPCasts.ByteServer
{
    internal class Program
    {
        public static Byte[] payload = ByteFactory.ArrayFromHexString("0F");
        public static int Port = 41916;
        private static IPAddress IPAddressTarget = IPAddress.Parse("127.0.0.1");

        private static void Main(string[] args)
        {
            Console.Title = "ByteServer";
            Console.WriteLine("Broadcasting on port: {0}", Port);

            using (BytesUDPServer server = new BytesUDPServer(Port))
            {
                server.AddSubscriber(IPAddressTarget);

                bool terminate = false;

                while (!terminate)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.Q:
                            {
                                terminate = true;
                                break;
                            }

                        default:
                            {
                                server.Broadcast(payload);
                                Console.WriteLine("Broadcast {0}", payload.ToHexString());
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