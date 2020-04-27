using System;
using System.Threading.Tasks;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using gk_server.handler;
using NLog;

namespace gk_server
{
    class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
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
                   .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                   {
                       var pipeline = channel.Pipeline;
                       pipeline.AddLast("split", new FrameSplitDecoder());
                       pipeline.AddLast("validator", new GkValidateHandler());
                       pipeline.AddLast("head-decode", new GkHeadDecoder());
                       pipeline.AddLast("body-decode", new GkBodyDecoder());
                       pipeline.AddLast("core", new GkCoreHandler());
                   }));
               const int port = 19557;
               var boundChannel = await bootstrap.BindAsync(port);
               Logger.Info($"server启动成功，监听端口:{port}");
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