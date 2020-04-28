using System;
using System.Net;
using System.Threading.Tasks;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace gk_udp_client
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
                    .Channel<SocketDatagramChannel>()
                    .Option(ChannelOption.SoBroadcast, true)
                    .Handler(new ActionChannelInitializer<IChannel>(ch =>
                    {
                        var pipeline = ch.Pipeline;
                        pipeline.AddLast("framing-dec", new FrameSplitDecoder()); 
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