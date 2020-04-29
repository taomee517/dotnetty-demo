using System;
using System.Text;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace gk_udp_server.handler
{
    public class EchoServerHandler : ChannelHandlerAdapter
    {
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var packet = message as DatagramPacket;
            var buffer = packet.Content;
            if (buffer != null)
            {
                Console.WriteLine("Received from client: " + buffer.ToString(Encoding.UTF8));
            }
            context.WriteAsync(new DatagramPacket(buffer, context.Channel.RemoteAddress));
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}