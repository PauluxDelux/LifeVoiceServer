using System;
using GameServer;

namespace serverLV
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "LifeVoiceServer-0.1";
            Server.Start(10, 26050);

            Console.ReadKey();
        }
    }
}
