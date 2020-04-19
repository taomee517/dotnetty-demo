using System;
using System.Net;
using DotNetty.Transport.Channels;
using dotnetty_server.beans;
using dotnetty_server.utils;
using NewLife.Caching;
using Newtonsoft.Json;
using NLog;

namespace dotnetty_server.handler
{
    public class CoreHandler: ChannelHandlerAdapter
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        
        private static ICache cache = MemoryCache.Default;
        private static TimeSpan expire = new TimeSpan(0, 0, 5);

        public override void ChannelActive(IChannelHandlerContext context)
        {
           
            var remoteAddress = context.Channel.RemoteAddress;
            _logger.Info($"连接建立！remote ={remoteAddress}");
            // var endPoint = remoteAddress.ToString();
            var ip = getIpFromEndPoint(remoteAddress);
            // _logger.Info($"缓存确认, ip {ip} exist: {cache.ContainsKey(ip)}, value: {cache.Get<int>(ip)}");
            // if (cache.ContainsKey(ip) && cache.Get<int>(ip) >= 2)
            // {
            //     _logger.Warn($"频繁断连的设备，主动拒绝！{ip}");
            //     context.CloseAsync();
            //     return;
            // }
            base.ChannelActive(context);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            base.ChannelInactive(context);
            var remoteAddress = context.Channel.RemoteAddress;
            _logger.Warn($"连接断开！remote ={remoteAddress}");
            var ip = getIpFromEndPoint(remoteAddress);
            if (cache.ContainsKey(ip))
            {
                cache.Set(ip, cache.Get<int>(ip) + 1, expire);
            }
            else
            {
                cache.Set(ip, 1, expire);
            }
            
        }


        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if(Equals(message,null)) return;
            var msgJson = JsonConvert.SerializeObject(message);
            _logger.Warn($"收到消息,sn = {(message as BaseMessage).Sn} msg = {msgJson}");
            // context.WriteAndFlushAsync(BytesUtil.Hex2Bytes(
            //     "3aa3002a0920200226000701000a2d00d003001a5e959184000100003a980ece3e00000e1014003c000100000000763f"));
        }
        
        

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            _logger.Error($"CoreHandler发生异常！e = {exception}");
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