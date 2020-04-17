using System;
using System.Net;
using DotNetty.Transport.Channels;
using dotnetty_server.beans;
using dotnetty_server.utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace dotnetty_server.handler
{
    public class CoreHandler: ChannelHandlerAdapter
    {
        // private static ILogger _logger = new LoggerFactory().CreateLogger<CoreHandler>();

        public override void ChannelActive(IChannelHandlerContext context)
        {
            base.ChannelActive(context);
            var remoteAddress = context.Channel.RemoteAddress;
            // _logger.LogError("连接建立！remote ={}", remoteAddress.ToString());
            Console.WriteLine("连接建立！remote ={0}", remoteAddress.ToString());
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            base.ChannelInactive(context);
            var remoteAddress = context.Channel.RemoteAddress;
            // _logger.LogWarning("连接断开！remote ={}", remoteAddress.ToString());
            Console.WriteLine("连接断开！remote ={0}", remoteAddress.ToString());
        }


        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var msgJson = JsonConvert.SerializeObject(message);
            // _logger.LogWarning("收到消息, msg = {}", msgJson);
            Console.WriteLine("收到消息, msg = {0}", msgJson);
            context.WriteAndFlushAsync(BytesUtil.Hex2Bytes(
                "3aa3002a0920200226000701000a2d00d003001a5e959184000100003a980ece3e00000e1014003c000100000000763f"));
        }
        
        

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            // _logger.LogError("CoreHandler发生异常！");
            Console.WriteLine("CoreHandler发生异常！");
            context.CloseAsync();
        }
    }
}