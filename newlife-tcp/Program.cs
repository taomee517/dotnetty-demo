using System;
using System.Net;
using System.Threading;
using dotnetty_echo.client;

namespace newlife_tcp
{
    class Program
    {
        static void Main(string[] args)
        {
            var tcpForward = new TcpForward();
            var endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9000);
            tcpForward.SetEndPoint(endpoint);
            for (int i = 0; i < 100; i++)
            {
                if (i!=0 && i%3==0)
                {
                    tcpForward.Dispose();
                }
                tcpForward.Forward("hello".GetBytes());
                Thread.Sleep(2000);
            }
        }
    }
}