using System;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using dotnetty_server.handler;
using dotnetty_server.logger;
using NLog;

namespace dotnetty_server
{
    class Program
    {
        static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        
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
                    .ChildOption(ChannelOption.SoKeepalive, true)    // 设置保持连接
                    .ChildOption(ChannelOption.SoSndbuf, 32 * 1024)
                    .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    var pipeline = channel.Pipeline;
                    pipeline.AddLast(new LoggingHandler("STATIC-NETTY-SRV-CONN"));
                    pipeline.AddLast("split", new FrameSplitHandler());
                    // pipeline.AddLast("validator", new ValidateHandler());
                    pipeline.AddLast("head-decode", new HeadDecoder());
                    pipeline.AddLast("core", new CoreHandler());
                }));
                const int port = 19014;
                var boundChannel = await bootstrap.BindAsync(port);
                // Console.WriteLine("server启动成功，监听端口:{0}", port);
                _logger.Info($"server启动成功，监听端口:{port}");
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

