using System;
using System.Net;
using System.Text;
using Mina.Core.Buffer;
using Mina.Core.Session;
using Mina.Transport.Socket;

namespace mina_echo_server
{
    class Program
    {
        private const string Ip = "127.0.0.1";
        private const int Port = 19556;
        static void Main(string[] args)
        {
            //tcp
//            var acceptor = new AsyncSocketAcceptor();
//            acceptor.FilterChain.AddLast("logger", new LoggingFilter());

            //udp
            var acceptor = new AsyncDatagramAcceptor();
            acceptor.SessionConfig.ReuseAddress = true;


            acceptor.Activated += (s, e) => Console.WriteLine("ACTIVATED");
            acceptor.Deactivated += (s, e) => Console.WriteLine("DEACTIVATED");
            acceptor.SessionCreated += (s, e) => e.Session.Config.SetIdleTime(IdleStatus.BothIdle, 10);
            acceptor.SessionOpened += (s, e) => Console.WriteLine("OPENED");
            acceptor.SessionClosed += (s, e) => Console.WriteLine("CLOSED");
            acceptor.SessionIdle += (s, e) => Console.WriteLine("*** IDLE #" + e.Session.GetIdleCount(IdleStatus.BothIdle) + " ***");
            acceptor.ExceptionCaught += (s, e) => 
            {
                Console.WriteLine(e.Exception);
                e.Session.Close(true);
            };
            acceptor.MessageReceived += (s, e) =>
            {
                var income = (IoBuffer)e.Message;
//                var resvData = income.GetInt64();
//                Console.WriteLine("Received : " + resvData);
//                var outcome = IoBuffer.Allocate(income.Capacity);
//                outcome.PutInt64(resvData);

                var msg = income.Duplicate().GetString(Encoding.UTF8);
                Console.WriteLine("Received : " + msg);
                
                var outcome = IoBuffer.Allocate(income.Capacity);
                outcome.Put(income);
                outcome.Flip();
                e.Session.Write(outcome);
            };

            acceptor.Bind(new IPEndPoint(IPAddress.Any, Port));
            Console.WriteLine("UDPServer listening on port " + Port);
            Console.ReadLine();
        }
    }
}