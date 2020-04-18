using System;
using System.Net;
using DotNetty.Transport.Channels;
using dotnetty_server.beans;
using dotnetty_server.utils;
using Microsoft.Extensions.Logging;
using NewLife.Caching;
using Newtonsoft.Json;

namespace dotnetty_server.handler
{
    public class CoreHandler: ChannelHandlerAdapter
    {
        // private static ILogger _logger = new LoggerFactory().CreateLogger<CoreHandler>();
        
        private static ICache cache = MemoryCache.Default;
        private static TimeSpan expire = new TimeSpan(0, 0, 5);

        public override void ChannelActive(IChannelHandlerContext context)
        {
           
            var remoteAddress = context.Channel.RemoteAddress;
            // _logger.LogError("连接建立！remote ={}", remoteAddress.ToString());
            // var endPoint = remoteAddress.ToString();
            var ip = getIpFromEndPoint(remoteAddress);
            Console.WriteLine("缓存确认, ip {0} exist: {1}, value: {2}", ip, cache.ContainsKey(ip), cache.Get<int>(ip));
            if (cache.ContainsKey(ip) && cache.Get<int>(ip) >= 2)
            {
                Console.WriteLine("频繁断连的设备，主动拒绝！{0}", ip);
                context.CloseAsync();
                return;
            }
            base.ChannelActive(context);
            Console.WriteLine("连接建立！remote ={0}", remoteAddress.ToString());
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            base.ChannelInactive(context);
            var remoteAddress = context.Channel.RemoteAddress;
            // _logger.LogWarning("连接断开！remote ={}", remoteAddress.ToString());
            // var endPoint = remoteAddress.ToString();
            var ip = getIpFromEndPoint(remoteAddress);
            if (cache.ContainsKey(ip))
            {
                cache.Set(ip, cache.Get<int>(ip) + 1, expire);
            }
            else
            {
                cache.Set(ip, 1, expire);
            }
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

        private string getIpFromEndPoint(EndPoint endPoint)
        {
            var end = endPoint.ToString();
            var startIndex = end.IndexOf("ffff") + 5;
            var endIndex = end.IndexOf("]");
            var ip = end.Substring(startIndex, endIndex-startIndex);
            return ip;
        }
    }
}