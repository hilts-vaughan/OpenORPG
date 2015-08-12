using System;

namespace Server
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = "OpenORPG Server";
            var server = new GameServer();
            server.Run();
        }
    }
}