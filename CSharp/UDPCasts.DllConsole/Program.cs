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

#if DEBUG
            generator.Generate();
#else
            generator.Generate();
#endif



            Console.WriteLine("Press <any> key to terminate...");
            Console.ReadKey(true);
        }
    }
}