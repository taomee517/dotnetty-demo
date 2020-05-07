using System;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using gk_common.utils;

namespace gk_udp_server.handler
{
    public class EchoServerHandler : ChannelHandlerAdapter
    {
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var packet = message as DatagramPacket;
            var buffer = packet.Content;
            var resp = Unpooled.Buffer(buffer.ReadableBytes);
            buffer.ReadBytes(resp);
            if (buffer != null)
            {
                Console.WriteLine("Received from client: " + BytesUtil.BytesToHex(resp.Array));
            }
            var respPacket = new DatagramPacket(resp, packet.Sender);
            context.WriteAndFlushAsync(respPacket);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}