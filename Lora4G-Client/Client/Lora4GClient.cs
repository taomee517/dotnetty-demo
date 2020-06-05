// 创建人：taomee
// 创建时间：2020/06/02 14:12

using System;
using System.Net;
using System.Threading.Tasks;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Lora4G_Client.Handler;

namespace Lora4G_Client.Client
{
    public class Lora4GClient
    {

        private string Host;
        private int Port;

        public Lora4GClient(string host, int port)
        {
            Host = host;
            Port = port;
        }

        public async Task Start()
        {
            var bootstrap = new Bootstrap();
            var workers = new MultithreadEventLoopGroup();
            try
            {
                bootstrap.Group(workers)
                    .Channel<TcpSocketChannel>()
                    .Handler(new ActionChannelInitializer<ISocketChannel>(ch =>
                    {
                        var pipeline = ch.Pipeline;
                        pipeline.AddLast("frame-dec", new FrameSplitDecoder());
                        pipeline.AddLast("idle", new IdleStateHandler(0,30,0));
                        pipeline.AddLast("device", new Lora4GDevice());
                    }));
                var addr = IPAddress.Parse(Host);
                var endPoint = new IPEndPoint(addr, Port);
                var channel =  await bootstrap.ConnectAsync(endPoint);
                Console.WriteLine($"client started, connect to : {endPoint}");
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