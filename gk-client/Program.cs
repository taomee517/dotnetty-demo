using System;
using System.Net;
using System.Threading.Tasks;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using gk_client.handler;

namespace gk_client
{
    class Program
    {
        const string Host = "127.0.0.1";
        //基康端口
        const int Port = 19557;
        
        static void Main(string[] args)
        {
            Start().Wait();
        }

        static async Task Start()
        {
            var bootstrap = new Bootstrap();
            var workers = new MultithreadEventLoopGroup();
            try
            {
                bootstrap.Group(workers)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                    .Option(ChannelOption.SoBacklog, 1024 * 2)
                    .Option(ChannelOption.SoRcvbuf, 32 * 1024 * 2 * 2)
                    .Handler(new ActionChannelInitializer<ISocketChannel>(ch =>
                    {
                        var pipeline = ch.Pipeline;
                        pipeline.AddLast("framing-dec", new FrameSplitDecoder());
                        pipeline.AddLast("idle", new IdleStateHandler(new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 3), new TimeSpan(0, 0, 0)));
                        pipeline.AddLast("device", new GkDevice());
                    }));
                var addr = IPAddress.Parse(Host);
                var endPoint = new IPEndPoint(addr, Port);
                var channel =  await bootstrap.ConnectAsync(endPoint);
                Console.ReadLine();
                await channel.CloseAsync();
            }
            catch(Exception e)
            {
                await Task.WhenAll(workers.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
            }
        }
    }
}