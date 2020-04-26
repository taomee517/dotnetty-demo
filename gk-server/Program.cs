using System;
using System.Threading.Tasks;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using gk_server.handler;

namespace gk_server
{
    class Program
    {
        static void Main(string[] args)
        {
            static void Main() => ServerStart().Wait();

            static async Task ServerStart()
            {
                IEventLoopGroup boss = new MultithreadEventLoopGroup();
                IEventLoopGroup worker = new MultithreadEventLoopGroup();
                try
                {
                    var bootstrap = new ServerBootstrap();
                    bootstrap.Group(boss, worker)
                        .Channel<TcpServerSocketChannel>()
                        .Option(ChannelOption.SoBacklog, 1024 * 2)
                        .Option(ChannelOption.SoRcvbuf, 32 * 1024 * 2 * 2)    
                        .ChildOption(ChannelOption.SoKeepalive, true)
                        .ChildOption(ChannelOption.SoSndbuf, 32 * 1024)
                        .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                        {
                            var pipeline = channel.Pipeline;
                            pipeline.AddLast("split", new FrameSplitDecoder());
                            // pipeline.AddLast("validator", new ValidateHandler());
                            pipeline.AddLast("head-decode", new GkHeadDecoder());
                            pipeline.AddLast("core", new GkCoreHandler());
                        }));
                    const int port = 19557;
                    var boundChannel = await bootstrap.BindAsync(port);
                    Console.WriteLine("server启动成功，监听端口:{0}", port);
                    // _logger.Info($"server启动成功，监听端口:{port}");
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
}