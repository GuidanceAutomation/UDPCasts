using GATools;
using System;

namespace UDPCasts.DllConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DllGenerator generator = new DllGenerator();

            generator.AddProjects(new string[]
            {
                "UDPCasts"
            });

            generator.Generate();

            Console.WriteLine("Press <any> key to terminate...");
            Console.ReadKey(true);
        }
    }
}