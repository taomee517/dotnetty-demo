using System;
using DotNetty.Transport.Channels;
using dotnetty_server.utils;
using NLog;

namespace dotnetty_server.handler
{
    public class ValidateHandler: ChannelHandlerAdapter
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();
        
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if(!ValidateUtil.validate(message as byte[])) return;
            context.FireChannelRead(message);
               
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            _logger.Error($"发生异常！{exception}");
            context.CloseAsync();
        }
    }
}