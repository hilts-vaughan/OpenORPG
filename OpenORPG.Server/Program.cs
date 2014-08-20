using System;

namespace Server
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = "OpenORPG Server";
            //Console.WindowWidth = 100;

            var server = new GameServer();
            server.Run();
        }
    }
}