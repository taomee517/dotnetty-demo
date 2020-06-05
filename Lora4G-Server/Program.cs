using System;
using Lora4G_Server.Server;

namespace Lora4G_Server
{
    class Program
    {
        private const int Port = 18568;
        static void Main(string[] args)
        {
            
            var server = new Lora4GServer(Port);
            Console.WriteLine("server start!");
            server.ServerStart().Wait();
        }
    }
}