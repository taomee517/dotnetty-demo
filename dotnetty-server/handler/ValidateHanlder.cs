using System;
using DotNetty.Transport.Channels;
using dotnetty_server.utils;

namespace dotnetty_server.handler
{
    public class ValidateHandler: ChannelHandlerAdapter
    {
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if(!ValidateUtil.validate(message as byte[])) return;
            context.FireChannelRead(message);
               
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("发生异常！");
        }
    }
}