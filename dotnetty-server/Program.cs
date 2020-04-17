using System;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using dotnetty_server.handler;

namespace dotnetty_server
{
    class Program
    {
        // static Logger logger = LogManager.GetCurrentClassLogger();
        
        static void Main() => ServerStart().Wait();

        static async Task ServerStart()
        {
            IEventLoopGroup boss = new MultithreadEventLoopGroup(1);
            IEventLoopGroup worker = new MultithreadEventLoopGroup();
            try
            {
                var bootstrap = new ServerBootstrap();
                bootstrap.Group(boss, worker)
                    .Channel<TcpServerSocketChannel>()
                    .Option(ChannelOption.Allocator, UnpooledByteBufferAllocator.Default)
                    .ChildOption(ChannelOption.SoKeepalive, true)
                    .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    var pipeline = channel.Pipeline;
                    pipeline.AddLast("split", new FrameSplitHandler());
                    pipeline.AddLast("validator", new ValidateHandler());
                    pipeline.AddLast("head-decode", new HeadDecoder());
                    pipeline.AddLast("core", new CoreHandler());
                }));
                const int port = 19014;
                var boundChannel = await bootstrap.BindAsync(port);
                Console.WriteLine("server启动成功，监听端口:{0}", port);
                // logger.Info("server启动成功，监听端口:{}", port);
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

