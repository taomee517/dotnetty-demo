using DotNetty.Buffers;
using DotNetty.Transport.Channels;

namespace gk_server.handler
{
    public class GkValidateHandler : ChannelHandlerAdapter
    {
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (message is IByteBuffer buffer)
            {
                //暂时省略校验
                context.FireChannelRead(buffer);
            }
        }
    }
}