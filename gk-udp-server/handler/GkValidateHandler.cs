using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace gk_udp_server.handler
{
    public class GkValidateHandler : ChannelHandlerAdapter
    {
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (message is DatagramPacket packet)
            {
                var buffer = packet.Content;
                //暂时省略校验
                if (true)
                {
                    context.FireChannelRead(packet);
                }
            }
        }
    }
}