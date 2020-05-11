using System;
using System.Net;
using System.Text;
using System.Threading;
using Mina.Core.Buffer;
using Mina.Core.Future;
using Mina.Transport.Socket;

namespace mina_echo_client
{
    class Program
    {
        private const int Port = 19556;
        static void Main(string[] args)
        {
            var connector =  new AsyncDatagramConnector();

            connector.ExceptionCaught += (s, e) =>
            {
                Console.WriteLine(e.Exception);
            };
            connector.MessageReceived += (s, e) =>
            {
//                var resvData = ((IoBuffer) e.Message).GetInt64();
                var income = ((IoBuffer) e.Message);
                var msg = income.GetString(Encoding.UTF8);
                Console.WriteLine("Session recv: {0}",msg);
            };
            connector.MessageSent += (s, e) =>
            {
                Console.WriteLine("Session sent...");
            };
            connector.SessionCreated += (s, e) =>
            {
                Console.WriteLine("Session created...");
            };
            connector.SessionOpened += (s, e) =>
            {
                Console.WriteLine("Session opened...");
            };
            connector.SessionClosed += (s, e) =>
            {
                Console.WriteLine("Session closed...");
            };
            connector.SessionIdle += (s, e) =>
            {
                Console.WriteLine("Session idle...");
            };

            var connFuture = connector.Connect(new IPEndPoint(IPAddress.Loopback, Port));
            connFuture.Await();

            connFuture.Complete += (s, e) =>
            {
                var f = (IConnectFuture)e.Future;
                if (f.Connected)
                {
                    Console.WriteLine("...connected");
                    var session = f.Session;
                    const string msg = "$00023EFE3605200509163000+907.109+4371.93+905.424+4394.13+868.363+4399.90+828.729+4345.94+891.702+4356.73+816.664+4409.86+731.857+4321.99+869.236+4354.41+842.243+4415.91+886.125+4417.95+896.128+4451.56+810.825+4399.13+803.707+4436.86+823.760+4424.20+0.00000+4503.69+0.00000+0999090+0.00000+0999090+0.00000+0999090+0.00000+0999090+0.00000+0999090+0.00000+0999090+0.00000+0999090+0.00000+0999090+0.00000+0999090E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000E1000000\n";
                    for (var i = 0; i < 1; i++)
                    {
//                        var memory = GC.GetTotalMemory(false);
//                        Console.WriteLine("gc memory: {0}, index: {1}", memory, i+1);
//                        var buffer = IoBuffer.Allocate(8);
//                        buffer.PutInt64(memory);

                        var buffer = IoBuffer.Allocate(msg.Length);
                        buffer.PutString(msg);
                        buffer.Flip();
                        session.Write(buffer);

                        try
                        {
                            Thread.Sleep(3000);
                        }
                        catch (ThreadInterruptedException)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Not connected...exiting");
                }
            };

            Console.ReadLine();
        }
    }
}