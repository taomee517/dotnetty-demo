// 创建人：taomee
// 创建时间：2020/06/03 10:03

using System;
using System.Threading.Tasks;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Lora4G_Server.Handler;
using Microsoft.Extensions.Logging;

namespace Lora4G_Server.Server
{
    public class Lora4GServer
    {
        private int port;

        public Lora4GServer(int port)
        {
            this.port = port;
        }

        public async Task ServerStart()
        {
            IEventLoopGroup boss = new MultithreadEventLoopGroup();
            IEventLoopGroup worker = new MultithreadEventLoopGroup();
            try
            {
                var bootstrap = new ServerBootstrap();
                bootstrap.Group(boss, worker)
                    .Channel<TcpServerSocketChannel>()
                    .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                    {
                        var pipeline = channel.Pipeline;
                        pipeline.AddLast("split", new FrameSplitDecoder());
                        pipeline.AddLast("head-decode", new HeadDecoder());
//                        pipeline.AddLast("body-decode", new GkBodyDecoder());
//                        pipeline.AddLast("core", new GkCoreHandler());
                    }));
                var boundChannel = await bootstrap.BindAsync(port);
                Console.WriteLine($"server启动成功，监听端口:{port}");
                Console.ReadLine();
                await boundChannel.CloseAsync();
            }
            finally
            {
                await Task.WhenAll(
                    boss.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                    worker.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
            }
        }
    }
}